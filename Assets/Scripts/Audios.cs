using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows.Speech;
using Random = UnityEngine.Random;

public class Audios : MonoBehaviour
{
    public AudioClip[] audiosTaco;
    public AudioClip[] audiosBolas;
    public AudioClip[] audiosBuraco;
    public AudioSource audio;

    

    private int random;
    // Start is called before the first frame update
    void Start()
    {
       
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Ball"))
        {
            random = Random.Range(0, audiosBolas.Length);
            GetComponent<AudioSource>().clip = audiosBolas[random];
            audio.Play();
        }
        if (collision.gameObject.CompareTag("Taco"))
        {
            random = Random.Range(0, audiosTaco.Length);
            GetComponent<AudioSource>().clip = audiosTaco[random];
            audio.Play();
        }
        if (collision.gameObject.CompareTag("Buraco"))
        {
            random = Random.Range(0, audiosBuraco.Length);
            GetComponent<AudioSource>().clip = audiosBuraco[random];
            audio.Play();
        }
    }
}
