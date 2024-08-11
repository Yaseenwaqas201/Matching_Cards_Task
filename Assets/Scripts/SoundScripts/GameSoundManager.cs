using System;
using UnityEngine;

public class GameSoundManager : MonoBehaviour
{

    public GameSounds[] soundsOfGame;
    public  static GameSoundManager InsSoundManager;
    

    // Start is called before the first frame update
    void Awake()
    {
        if(InsSoundManager==null)
        {
            InsSoundManager = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        foreach(GameSounds s in soundsOfGame)
        {
            s.audioSource = gameObject.AddComponent<AudioSource>();
            s.audioSource.clip = s.clipOfSound;
            s.audioSource.volume = s.soundVolumeValue;
            s.audioSource.pitch = s.soundPitchValue;
            s.audioSource.spatialBlend = s.SoundSpatialBlendValue;
            s.audioSource.loop = s.soundLoopBool;
        }
        
    }


    public void PlaySound(string name)
    {
        GameSounds s= Array.Find(soundsOfGame, sound => sound.nameOfSound == name);
        if (s == null)
        {
            Debug.LogWarning("Game Sound: " + name + " not found");
        }
        else
        {
            s.audioSource.Play();
        }
    }

    public void StopSound(string name)
    {
        GameSounds s= Array.Find(soundsOfGame, sound => sound.nameOfSound == name);
        if (s == null)
        {
            Debug.LogWarning("Game Sound: " + name + " not found");
        }
        else
        {
            s.audioSource.Stop();
        }
    }


}


