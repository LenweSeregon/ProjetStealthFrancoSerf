using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

    private int indexMusicPlayed;
    public AudioSource[] musicJukebox;

    public AudioSource buttonClicked;
    public AudioSource pickaxe;
    public AudioSource axe;
    public AudioSource footSand;
    public AudioSource doorOpen;
    public AudioSource alarm;

    public void Start()
    {
        if(musicJukebox != null && musicJukebox.Length > 0)
        {
            indexMusicPlayed = 0;
            musicJukebox[indexMusicPlayed].Play();
            StartCoroutine(StartMusicJukebox());
        }

    }

    private void OnDestroy()
    {
        StopCoroutine(StartMusicJukebox());
    }

    public void PlayButtonClick()
    {
        buttonClicked.Play();
    }

    public void PlayFootSand()
    {
        footSand.Play();
    }

    public void PlayPickaxe()
    {
        pickaxe.Play();
    }
    public void StopPickaxe()
    {
        pickaxe.Stop();
    }

    public void PlayAxe()
    {
        axe.Play();
    }
    public void StopAxe()
    {
        axe.Stop();
    }

    public void PlayDoorOpen()
    {
        doorOpen.Play();
    }
    public void StopDoorOpen()
    {
        doorOpen.Stop();
    }

    public void PlayAlarm()
    {
        alarm.Play();
    }
    public void StopAlarm()
    {
        alarm.Stop();
    }

    private IEnumerator StartMusicJukebox()
    {
        while(true)
        {
            if (!musicJukebox[indexMusicPlayed].isPlaying)
            {
                indexMusicPlayed++;
                if(indexMusicPlayed >= musicJukebox.Length)
                {
                    indexMusicPlayed = 0;
                }
                musicJukebox[indexMusicPlayed].Play();
            }
            yield return new WaitForSeconds(2.0f);
        }

    }
}
