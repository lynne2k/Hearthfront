using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperAudioManager : MonoBehaviour
{
    public AudioSource audioRewind;


    public void PlayRewind()
    {
        audioRewind.Play();
    }
}
