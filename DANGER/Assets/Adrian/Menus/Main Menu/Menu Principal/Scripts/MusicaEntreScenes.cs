using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class MusicaEntreScenes : MonoBehaviour
{
    private AudioSource _audioSource;
    public  AudioMixer _audioMixer;

    private void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
        _audioSource = GetComponent<AudioSource>();

    }

    void Start()
    {
        if (PlayerPrefs.HasKey("volume"))
        {
            float volumen = PlayerPrefs.GetFloat("volume");
            _audioMixer.SetFloat("volume", volumen);
        }
    }

    public void PlayMusic()
    {
        if (_audioSource.isPlaying) return;
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }
}