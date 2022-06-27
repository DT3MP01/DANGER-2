using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
[ExecuteInEditMode]

public class Sensor : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Player")]
    public bool isPlayer;

    public float distance = 10;
    public float angle = 30;
    public float height = 1;
    public Color meshColor = Color.blue;

    public int  scanFrequency = 2 ;
    public LayerMask layer;
    public LayerMask OcclusionLayer;
    Collider[] colliders = new Collider[100];
    int count;
    public List<GameObject> Objects = new List<GameObject>();
    float scanInterval;
    private float scanTimer;
    [Header("Detections")]
    public bool nearbySmoke=false;
    public bool fireAlarm = false;
    public bool fireNearby = false;
    public bool fireInSight=false;
    public bool touchingFire=false;
    public bool isTerrified=false;
    public bool followPlayer = false;
    public bool isCrawling = false;

    
    public GameObject extinguisher;
    public ParticleSystem fog;

    public Animator animator;

    public double playerHealth=100f;

    public double MaxHealth =100f;
    public double playerStress =0f;
    public double MaxStress =100f;

    private List<Items.ItemType> ItemList = new List<Items.ItemType>();

    

    public float extinguisherCapacity=0f;
    public float extinguisherDepletionRate = 4f;
    public bool usingExtinguisher;

    public float stressModifier=0.1f;
    public float healthModifier = 0.1f;


    public float currWeight;
    private float yVelocity = 0.1F;

    Mesh mesh;
    void Start()
    {
        playerHealth = MaxHealth;
        playerStress = 0;
        mesh = CreateWedgeMesh();
        scanInterval = 1f / scanFrequency;
        usingExtinguisher = false;
        currWeight = 0;
    }


    // Update is called once per frame
    void Update()
    {
        scanTimer -= Time.deltaTime;
        if (scanTimer <= 0)
        {
            scanTimer = scanInterval;
            Scan();
            CalculateHealth();
            if(usingExtinguisher){
                extinguisherCapacity= Mathf.Max(0,extinguisherCapacity- extinguisherDepletionRate);
            }
            if(isPlayer && gameObject.GetComponent<BehaviourTreeRunner>().enabled == true){
            gameObject.GetComponent<CharacterMovment>().underControl = true;
            gameObject.GetComponent<CharacterMovment>().enabled = true;
            gameObject.GetComponent<BehaviourTreeRunner>().enabled = false;
            }
            else if (!isPlayer){
                gameObject.GetComponent<CharacterMovment>().underControl = false;
                gameObject.GetComponent<CharacterMovment>().enabled = false;
                gameObject.GetComponent<BehaviourTreeRunner>().enabled = true;
            }
        }
        

        if(extinguisherCapacity==0){
            extinguisher.SetActive(false);
        }
        else{
            extinguisher.SetActive(true);
        }

        if(Input.GetKeyDown(KeyCode.Space) )
        {
            Debug.Log("Pressed Space");
            fog.Play();
            usingExtinguisher=true;
            StartCoroutine(LerpFunction(animator.GetLayerWeight(1),1,0.3f,currWeight));
            
        }
        if(Input.GetKeyUp(KeyCode.Space)||extinguisherCapacity <=0)
        {
            fog.Stop();
            usingExtinguisher=false;
            StartCoroutine(LerpFunction(animator.GetLayerWeight(1),0,0.3f,currWeight));
        }

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrawling = !animator.GetBool("isCrouching");
            animator.SetBool("isCrouching",!animator.GetBool("isCrouching"));
            
        }
        
    }

    public double GetMaxHp()
    {
        return MaxHealth;
    }


    public double GetCurrentHp()
    {
        return playerHealth;
    }

    public double GetMaxStress()
    {
        return MaxStress;
    }

    public double GetCurrentStress()
    {
        return playerStress;
    }

    public List<Items.ItemType> GetItemList()
    {
        return ItemList;
    }


    public void AddHp(double i)
    {
        playerHealth += i;
    }

    public void LossHp(double i)
    {
        playerHealth -= i;
    }

    public void AddStress(double i)
    {
        playerStress += i;
    }

    public void LossStress(double i)
    {
        playerStress -= i;
    }

    public void addItem(Items.ItemType it)
    {
        ItemList.Add(it);
    }

    public void LostItem(Items.ItemType it)
    {
        if (ItemList.Contains(it))
            ItemList.Remove(it);
    }

    IEnumerator LerpFunction(float startValue,float endValue, float lerpDuration,float valueToLerp)
    {
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            valueToLerp = Mathf.Lerp(startValue, endValue, timeElapsed / lerpDuration);
            animator.SetLayerWeight(1, valueToLerp);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        valueToLerp = endValue;
    }


    IEnumerator ActivateBehaviourTree()
    {
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<BehaviourTreeRunner>().enabled = true;
    }

    IEnumerator PauseBehaviourTree()
    {
        yield return new WaitForSeconds(1);
        gameObject.GetComponent<BehaviourTreeRunner>().enabled = true;
    }


    private void  CalculateHealth(){
        if(nearbySmoke && !animator.GetBool("isCrouching")){
            playerHealth -= 10*healthModifier;
            playerStress += 5*stressModifier;
        }
        if(touchingFire)
        {
            playerHealth -= 40*healthModifier;
            playerStress += 10*stressModifier;
        }
        if(isTerrified){
            playerStress -= 30*stressModifier;
        }

        if(playerHealth<=0){
            playerHealth = 0;
        }
        if(playerStress<=0){
            playerStress = 0;
        }
        if(playerHealth>=MaxHealth){
            playerHealth = MaxHealth;
        }
        if(playerStress>=MaxStress){
            playerStress = MaxStress;
        }
    }
    private void Scan(){
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, colliders, layer,QueryTriggerInteraction.Collide);
        Objects.Clear();
        fireInSight = false;
        fireNearby = false;
        nearbySmoke = false;
        int smokeCount = 0;

        for (int i = 0; i < count; i++)
        {
            GameObject obj = colliders[i].gameObject;
            if(obj.tag == "Smoke"){
                smokeCount++;
            }
            if (obj.tag == "Fire")
            {
                fireNearby = true;
            }
            if (IsInSight(obj))
            {
                Objects.Add(obj);
                if (obj.tag == "Fire")
                {
                    fireInSight = true;
                }
            }
        }
        if (smokeCount>=15)
        {
            nearbySmoke=true;
        }

        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Fire")
        {
            Debug.Log("AY ME QUEMO");
            touchingFire=true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Fire")
        {
            Debug.Log("YANO");
            touchingFire = false;
        }
    }

    private bool IsInSight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;

        Vector3 direction = dest - origin;
        Debug.DrawRay(origin, direction, Color.red);
        if (direction.y < 0 || direction.y > height)
        {
            return false;
        }
        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if (deltaAngle > angle)
        {
            return false;
        }

        origin.y += height / 2;
        dest.y = origin.y;
        if (Physics.Linecast(origin, dest, OcclusionLayer))
        {
            return false;
        }
        return true;
    }

    public bool getDetections(string detection){
        switch(detection){
            case "nearbySmoke":
                return nearbySmoke;
            case "fireInSight":
                return fireInSight;
            case "isTerrified":
                return isTerrified;
            case "followPlayer":
                return followPlayer;
            default:
                return false;
        }
    }



    Mesh CreateWedgeMesh(){
        Mesh mesh = new Mesh();
        int segments = 10;
        int numTriangles = (segments * 4) + 2 + 2;
        int numVertices = 3 * numTriangles;

        Vector3[] vertices = new Vector3[numVertices]; 
        int[] triangles = new int[numTriangles * 3];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward*distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward*distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topRight =  bottomRight + Vector3.up * height;
        Vector3 topLeft =  bottomLeft + Vector3.up * height;

        int vert = 0 ;

        //left Side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = topLeft;

        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;

        //right side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;

        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;
    
        float currentAngle= -angle;
        float deltaAngle = (angle * 2) / segments;

        for (int i=0; i < segments; i++){


            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward*distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward*distance;

            topLeft = bottomLeft + Vector3.up * height;
            topRight = bottomRight + Vector3.up * height;

            //far side
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            //top
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            //bottom
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;

            currentAngle += deltaAngle;

        }

        for(int i = 0; i < numVertices; i++){
            triangles[i]=i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        return mesh;
    }

    private void OnValidate(){
        mesh = CreateWedgeMesh();
        scanInterval = 1 / scanFrequency;
    }

    private void OnDrawGizmos()
    {
        if(mesh){
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation, transform.lossyScale);
        }
        Gizmos.DrawWireSphere(transform.position, distance);
        for (int i = 0; i < count; i++)
        {
            Gizmos.DrawSphere(colliders[i].transform.position, 0.2f);
        }

        Gizmos.color = Color.green;
        foreach (var obj in Objects)
        {
            Gizmos.DrawSphere(obj.transform.position, 0.2f);
        }
    }
}
