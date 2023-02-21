using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioSource> destroyNoise;
    [SerializeField] private AudioSource music;
    public void PlayRandomDestroyNoise(){
        //Choose a random number
        int clipToPlay = Random.Range(0, destroyNoise.Count);
        //stop if play
        destroyNoise[clipToPlay].Stop();
        //play that clip
        destroyNoise[clipToPlay].Play();
    }

}
