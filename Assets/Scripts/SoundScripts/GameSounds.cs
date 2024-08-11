using UnityEngine;


[System.Serializable]
public class GameSounds 
{

    public string nameOfSound;
    public AudioClip clipOfSound;

    [Range(0f, 1f)]
    public float soundVolumeValue;
    [Range(.1f,3f)]
    public float soundPitchValue;
    public bool soundLoopBool=false;

    [Range(0f, 1f)]
    public float SoundSpatialBlendValue;
    
    [HideInInspector]
    public AudioSource audioSource;

}
