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


public class SoundManager : MonoBehaviour {

   public AudioRepository[] PersistendAudio;
  
   public void PlaySound(int catIndex, int clipIndex)
    {
        this.PersistendAudio[catIndex].AudioSourceRef.clip = this.PersistendAudio[catIndex].PossibleSounds[clipIndex];
        this.PersistendAudio[catIndex].AudioSourceRef.Play();
    }

   public void StopSound(int catIndex, int clipIndex)
    {
        this.PersistendAudio[catIndex].AudioSourceRef.clip = this.PersistendAudio[catIndex].PossibleSounds[clipIndex];
        this.PersistendAudio[catIndex].AudioSourceRef.Stop();
    }

}
