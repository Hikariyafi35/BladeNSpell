using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public Sound[] musicSound, sfxSound;
    public AudioSource musicSource, sfxSource;
    
    private void Awake() {
        if(Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else{
            Destroy(gameObject);
        }
    }
    
    private void Start() {
        PlayMusicForScene();
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        PlayMusicForScene();
    }

    public void PlayMusicForScene() {
        string sceneName = SceneManager.GetActiveScene().name;

        if (sceneName == "InGame") {
            PlayMusic("InGame");
        } 
        else if (sceneName == "MainMenu") {
            PlayMusic("MainMenu");
        } 
        // Tambahkan else if lain jika ada scene tambahan dengan musik spesifik
    }

    public void PlayMusic(string name){
        Sound s = Array.Find(musicSound, x => x.name == name);
        if(s == null){
            Debug.Log("Sound not found: " + name);
        }
        else{
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name){
        Sound s = Array.Find(sfxSound, x => x.name == name);
        if(s == null){
            Debug.Log("SFX not found: " + name);
        }
        else{
            sfxSource.PlayOneShot(s.clip);
        }
    }
}
