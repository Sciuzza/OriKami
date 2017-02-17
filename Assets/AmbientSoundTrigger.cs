using UnityEngine;
using System.Collections;

public class AmbientSoundTrigger : MonoBehaviour {

   // private AudioClip audioRef;
    private SoundManager soundRef;
    private AudioSource audioSourceRef;
    public float timer = 10;



	// Use this for initialization
	void Awake () {

        soundRef = GameObject.FindGameObjectWithTag("GameController").GetComponent<SoundManager>();
        audioSourceRef = GameObject.FindGameObjectWithTag("GameController").GetComponent<SoundManager>().PersistendAudio[2].AudioSourceRef;
    }
	
    void OnTriggerEnter(Collider other)
    {
       
        if (other.gameObject.tag == "Player")
        {

            StartCoroutine(FadeInCO());

        }

     
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            StartCoroutine(FadeOutCO());
        }

    }

    IEnumerator FadeOutCO()
    {
        soundRef.PlaySound(2, Random.Range(0, 2));
        float elapsedTime = 0.0f;
        float startVol = audioSourceRef.volume;
        while (elapsedTime < 1f)
        {
            audioSourceRef.volume = Mathf.Lerp(startVol, 0, (elapsedTime / 1.0f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        audioSourceRef.volume = 0;
    }

    IEnumerator FadeInCO()
    {
        soundRef.PlaySound(2, Random.Range(0, 2));
        float elapsedTime = 0.0f;
        float startVol = audioSourceRef.volume;
        while (elapsedTime < 1f)
        {
            audioSourceRef.volume = Mathf.Lerp(startVol, 1, (elapsedTime/1.0f));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        audioSourceRef.volume = 1;
    }


}
