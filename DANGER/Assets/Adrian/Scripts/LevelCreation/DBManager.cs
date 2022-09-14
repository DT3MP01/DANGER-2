using Firebase;
using Firebase.Firestore;
using Firebase.Storage;
using Firebase.Auth;
using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System;
using System.Collections;
using System.Net.Http;
using System.Linq;
using System.Text;
using TMPro;


public class DBManager : MonoBehaviour
{


    public string partida;
    public DependencyStatus dependencyStatus;
    public ObjectCreation game;
    public timer timer;
    private FirebaseFirestore database;
    private FirebaseStorage storage;
    private FirebaseAuth auth;
    private FirebaseUser User;

    [Header("Login")]
    public GameObject login;
    public TMP_InputField loginEmail;
    public TMP_InputField loginPassword;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;
    public GameObject logoutButton;


    [Header("Register")]
    public GameObject register;
    public TMP_InputField registerUsername;
    public TMP_InputField registerEmail;
    public TMP_InputField registerPassword;
    public TMP_InputField registerPasswordVerify;
    public TMP_Text warningRegisterText;

    [Header("Upload")]
    public GameObject upload;
    public TMP_InputField uploadName;


    void Start()
    {
        //Check that all of the necessary dependencies for Firebase are present on the system
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
            }
            else
            {
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
    }
    // Start is called before the first frame update
    void InitializeFirebase()
    {
        // Get the root reference location of the database.
        //reference = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;
        
        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
        database = FirebaseFirestore.DefaultInstance;
        storage= FirebaseStorage.DefaultInstance;

    }
    void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser != User)
        {
            bool signedIn = User != auth.CurrentUser && auth.CurrentUser != null;
            if (!signedIn && User != null)
            {
                Debug.Log("Signed out " + User.UserId);
                logoutButton.SetActive(false);
            }
            User = auth.CurrentUser;
            if (signedIn)
            {
                Debug.Log("Signed in" + User.UserId);
                logoutButton.SetActive(true);
            }
        }
    }



    public void Logout()
    {
        auth.SignOut();
    }

    IEnumerator UploadData()
    {
        yield return StartCoroutine(game.RecordFrame());

        // DocumentReference docData = database.Collection("GameData").Document();
        // docData.SetAsync(new Dictionary<string, object> {{"Raw",game.json}});

        DocumentReference docRefInfo = database.Collection("GameInfo").Document();

        StorageReference docData = storage.RootReference.Child("GameInfo-Data").Child(docRefInfo.Id+".dataRoom");
        docData.PutBytesAsync(Encoding.Unicode.GetBytes(game.json));

        StorageReference docDataImage = storage.RootReference.Child("GameInfo-Data").Child(docRefInfo.Id+"-Image.png");
        docDataImage.PutBytesAsync(Convert.FromBase64String(game.SaveRoomData.image));

        StatsRoom statsRoom = new StatsRoom(game.meters, game.extinguishers, game.windows, game.doors, game.countScans);
        Dictionary<string, object>data =  new Dictionary<string, object> {
            {"Image",docDataImage.Path},
            {"PlayerName", auth.CurrentUser.DisplayName},
            {"RoomName", uploadName.text},
            {"Meters",game.SaveRoomData.statsRoom.meters},
            {"Extinguishers",game.SaveRoomData.statsRoom.extinguishers},
            {"Windows",game.SaveRoomData.statsRoom.windows},
            {"Doors",game.SaveRoomData.statsRoom.doors},
            {"CountScans",game.SaveRoomData.statsRoom.countScans},
            {"Reference",docData.Path}
            };
        docRefInfo.SetAsync(data);

    }

    private IEnumerator Login(string _email, string _password)
    {
        //Call the Firebase auth signin function passing the email and password
        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

        if (LoginTask.Exception != null)
        {
            //If there are errors handle them
            Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
            FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

            string message = "Login Failed!";
            switch (errorCode)
            {
                case AuthError.MissingEmail:
                    message = "Missing Email";
                    break;
                case AuthError.MissingPassword:
                    message = "Missing Password";
                    break;
                case AuthError.WrongPassword:
                    message = "Wrong Password";
                    break;
                case AuthError.InvalidEmail:
                    message = "Invalid Email";
                    break;
                case AuthError.UserNotFound:
                    message = "Account does not exist";
                    break;
            }
            warningLoginText.text = message;
        }
        else
        {
            //User is now logged in
            //Now get the result
            User = LoginTask.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
            warningLoginText.text = "";
            confirmLoginText.text = "Logged In";

            login.SetActive(false);
            loginEmail.text = "";
            loginPassword.text = "";
            warningRegisterText.text = "";
        }
    }

    private IEnumerator Register(string _email, string _password, string _username)
    {
        if (_username == "")
        {
            //If the username field is blank show a warning
            warningRegisterText.text = "Missing Username";
        }
        else if (registerPassword.text != registerPasswordVerify.text)
        {
            //If the password does not match show a warning
            warningRegisterText.text = "Password Does Not Match!";
        }
        else
        {
            //Call the Firebase auth signin function passing the email and password
            var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

            if (RegisterTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Register Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WeakPassword:
                        message = "Weak Password";
                        break;
                    case AuthError.EmailAlreadyInUse:
                        message = "Email Already In Use";
                        break;
                }
                warningRegisterText.text = message;
            }
            else
            {
                //User has now been created
                //Now get the result
                User = RegisterTask.Result;

                if (User != null)
                {
                    //Create a user profile and set the username
                    UserProfile profile = new UserProfile { DisplayName = _username };

                    //Call the Firebase auth update user profile function passing the profile with the username
                    var ProfileTask = User.UpdateUserProfileAsync(profile);
                    //Wait until the task completes
                    yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                    if (ProfileTask.Exception != null)
                    {
                        //If there are errors handle them
                        Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                        FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                        AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                        warningRegisterText.text = "Username Set Failed!";
                    }
                    else
                    {
                        //Username is now set
                        //Now return to login screen
                        register.SetActive(false);
                        registerUsername.text = "";
                        registerPassword.text = "";
                        registerEmail.text = "";
                        registerPasswordVerify.text = "";
                        login.SetActive(false);
                        loginEmail.text = "";
                        loginPassword.text = "";
                        warningRegisterText.text = "";
                    }
                }
            }
        }
    }

    public void LoginButton()
    {
        //Call the login coroutine passing the email and password
        StartCoroutine(Login(loginEmail.text, loginPassword.text));
    }
    //Function for the register button
    public void RegisterButton()
    {
        //Call the register coroutine passing the email, password, and username
        StartCoroutine(Register(registerEmail.text, registerPassword.text, registerUsername.text));
    }

    public void UploadRoom()
    {
        StartCoroutine(UploadData());

        
    }
    public void LoginStep()
    {
        if (auth.CurrentUser != null)
        {
            upload.SetActive(true);
        }
        else
        {
            game.disableInput();
            login.SetActive(true);
        }
    }
    public void inicializarBD()
    {

        // Debug.Log(text);
        // Debug.Log(this.game.json);
        
    }

    
}
