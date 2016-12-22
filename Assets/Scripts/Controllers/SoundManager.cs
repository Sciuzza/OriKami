using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;


[System.Serializable]
public class AudioRepository
{
    public string AudioCatName;

    public AudioSource AudioSourceRef;

    public AudioClip[] PossibleSounds;
}



public class SoundManager : MonoBehaviour
{

    public AudioRepository[] PersistentAudio;




    public void PlaySound(int catIndex, int clipIndex)
    {
        this.PersistentAudio[catIndex].AudioSourceRef.clip = this.PersistentAudio[catIndex].PossibleSounds[clipIndex];
        this.PersistentAudio[catIndex].AudioSourceRef.Play();
    }
}
