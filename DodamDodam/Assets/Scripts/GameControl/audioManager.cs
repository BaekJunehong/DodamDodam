using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioManager : MonoBehaviour
{
    public List<AudioClip> tutorials;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeAudio(int num)
    {
        stopAudio();
        playAudio(num);
    }
    public void playAudio(int num)
    {
        if(audioSource != null)
        {
            audioSource.clip = tutorials[num];
            audioSource.Play();
        }        
    }

    public void stopAudio()
    {
        audioSource.Stop();
    }
}
