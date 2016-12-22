using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Button))]
public class SoundManager : MonoBehaviour {

    public AudioClip sound;

    private Button buttons { get { return GetComponent<Button>(); } }
    public AudioSource source { get { return GetComponent<AudioSource>(); } }
    
    

    void Start()
    {
     
        
        //gameObject.AddComponent<AudioSource>();
        source.clip = sound;
        source.playOnAwake = false;

        buttons.onClick.AddListener(() => PlaySound());
        
       

    }

    void PlaySound()
    {
        source.PlayOneShot(sound);
    }
    void Awake()
    {
       // this.GetComponent<GameController>().initializer.AddListener();
    }
}
