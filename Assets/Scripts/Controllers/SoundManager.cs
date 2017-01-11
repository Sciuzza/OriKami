using UnityEngine;
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

    private void Awake()
    {
        if (this.gameObject.tag != "GameController") return;

        GameController gcTempLink = this.GetComponent<GameController>();

        gcTempLink.ngpInitializer.AddListener(this.EnablingAudioSources);

        this.PersistendAudio[0].AudioSourceRef.enabled = true;
        this.PersistendAudio[1].AudioSourceRef.enabled = true;



        var mmTempLink = this.gameObject.GetComponent<MenuManager>();

        mmTempLink.soundRequest.AddListener(this.PlaySound);

    }

    private void EnablingAudioSources()
    {
        AudioSource[] ci = this.gameObject.GetComponents<AudioSource>();

        foreach (var d in ci)
        {
            d.enabled = true;
        }

        if (!this.PersistendAudio[0].AudioSourceRef.isActiveAndEnabled)
            Debug.Log("Still not enabled");

    }

}
