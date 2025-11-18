using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAudio : MonoBehaviour
{
    public AudioClip[] moanClips;
    public float minDelay = 5f;
    public float maxDelay = 15f;

    AudioSource audioSource;

    Camera mainCam;

    void Start()
    {
        mainCam = Camera.main;

        audioSource = GetComponent<AudioSource>();
        audioSource.spatialBlend = 1f;
        StartCoroutine(MoanRoutine());
    }
    IEnumerator MoanRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(waitTime);

            if (AudioManager.instance.CanPlayMoan() && 
                Vector3.Distance(transform.position, mainCam.transform.position) <= audioSource.maxDistance)
            {
                AudioClip clip = moanClips[Random.Range(0, moanClips.Length)];
                audioSource.PlayOneShot(clip, AudioManager.instance.volume);
                AudioManager.instance.NotifyMoanStarted(clip.length);
            }
        }
    }
    public void PlayFootStep()
    {
        AudioManager.instance.PlayFootstep(audioSource);
    }
    public void DeathEvent()
    {
        Destroy(this);
    }
}
