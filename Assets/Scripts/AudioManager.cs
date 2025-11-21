using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public float volume = 1;

    public AudioClip automaticClip;
    public AudioClip smgClip;
    public AudioClip pistolClip;
    public AudioClip revolverClip;
    public AudioClip minigunClip;
    public AudioClip uziClip;
    public AudioClip akClip;
    public AudioClip magOut;
    public AudioClip magIn;
    public AudioClip cocking;
    public AudioClip pistolCocking;
    public AudioClip revolverOut;
    public AudioClip revolverOffLoad;
    public AudioClip revolverOnLoad;
    public AudioClip revolverIn;
    public AudioClip minigunCooldown;
    public AudioClip beep1;
    public AudioClip beep2;
    public AudioClip playerDeath;
    public AudioClip zombieAttack;

    public AudioClip UIBoom;

    public AudioClip[] fleshHitClips;
    public AudioClip[] deathClips;
    public AudioClip[] footsteps;
    public AudioClip[] playerPain;
    public AudioClip[] explosionSounds;
    public AudioClip[] grenadeSounds;
    public AudioClip[] metalHitSounds;

    public AudioSource playerAudioSource;

    public int maxSimultaneousMoans = 3;
    private int currentMoans = 0;

    public float firstDelay;
    public float secondDelay;

    private void Awake()
    {
        instance = this;
    }

    public void PlayerDeath()
    {
        playerAudioSource.PlayOneShot(playerDeath, volume);
    }
    public void PlayUIBoom()
    {
        playerAudioSource.PlayOneShot(UIBoom, volume);
    }
    public void Boom1()
    {
        if (InGameToturialManager.Instance.tutStage != -1) return;
        if (FindObjectOfType<InGameBoosterTutorialManager>() != null) return;

        playerAudioSource.PlayOneShot(beep1, volume);
    }
    public void Boom2()
    {
        if (InGameToturialManager.Instance.tutStage != -1) return;
        if (FindObjectOfType<InGameBoosterTutorialManager>() != null) return;

        playerAudioSource.PlayOneShot(beep2, volume);
    }

    public void PlayZombieAttack()
    {
        playerAudioSource.PlayOneShot(zombieAttack, volume);
    }
    public void PlayAutomaticFire(AudioSource source)
    {
        source.PlayOneShot(automaticClip, volume);
    }
    public void PlaySMGFire(AudioSource source)
    {
        source.PlayOneShot(smgClip, volume);
    }
    public void PlayPistolClip(AudioSource source)
    {
        source.PlayOneShot(pistolClip, volume);
    }
    public void PlayRevolverClip(AudioSource source)
    {
        source.PlayOneShot(revolverClip, volume);
    }
    public void PlayMinigunClip(AudioSource source)
    {
        source.PlayOneShot(minigunClip, volume);
    }
    public void PlayUZIClip(AudioSource source)
    {
        source.PlayOneShot(uziClip, volume);
    }
    public void AK47Clip(AudioSource source)
    {
        source.PlayOneShot(akClip, volume);
    }
    public void MetalHitClip(AudioSource source)
    {
        source.PlayOneShot(metalHitSounds[Random.Range(0, metalHitSounds.Length)], volume);
    }
    public void PlayExplosionSound()
    {
        playerAudioSource.PlayOneShot(explosionSounds[Random.Range(0, explosionSounds.Length)], volume);
    }
    public void PlayGrenadeSound()
    {
        playerAudioSource.PlayOneShot(grenadeSounds[Random.Range(0, grenadeSounds.Length)], volume);
    }
    public void PlayMinigunCooldown()
    {
        playerAudioSource.PlayOneShot(minigunCooldown, volume);
    }
    public void PlayFleshHitClip(AudioSource source)
    {
        playerAudioSource.PlayOneShot(fleshHitClips[Random.Range(0, fleshHitClips.Length)], volume);
    }
    public void PlayDeathSound(AudioSource source)
    {
        playerAudioSource.PlayOneShot(deathClips[Random.Range(0, deathClips.Length)], volume);
    }
    public void SMGReload(AudioSource source)
    {
        StartCoroutine(smgReload(source));
    }
    IEnumerator smgReload(AudioSource source)
    {
        yield return new WaitForSeconds(0.3f);
        source.PlayOneShot(magOut, volume);
        yield return new WaitForSeconds(0.1f);
        source.PlayOneShot(magIn, volume);
    }
    public void AutomaticReload(AudioSource source)
    {
        StartCoroutine(automaticReload(source));
    }
    IEnumerator automaticReload(AudioSource source)
    {
        yield return new WaitForSeconds(0.4f);
        source.PlayOneShot(magOut, volume);
        yield return new WaitForSeconds(0.2f);
        source.PlayOneShot(magIn, volume);
    }
    public void PistolReload(AudioSource source)
    {
        StartCoroutine(pistolReload(source));
    }
    IEnumerator pistolReload(AudioSource source)
    {
        yield return new WaitForSeconds(0.35f);
        source.PlayOneShot(magOut, volume);
        yield return new WaitForSeconds(0.25f);
        source.PlayOneShot(magIn, volume);
        yield return new WaitForSeconds(0.7f);
        source.PlayOneShot(pistolCocking, volume);
    }
    public void RevolverReload(AudioSource source)
    {
        StartCoroutine(revolverReload(source));
    }
    IEnumerator revolverReload(AudioSource source)
    {
        yield return new WaitForSeconds(0.15f);
        source.PlayOneShot(revolverOut, volume);
        yield return new WaitForSeconds(0.7f);
        source.PlayOneShot(revolverOffLoad, volume);
        yield return new WaitForSeconds(1.5f);
        source.PlayOneShot(revolverOnLoad, volume);
        yield return new WaitForSeconds(0.25f);
        source.PlayOneShot(revolverIn, volume);
    }
    public void UZIReload(AudioSource source)
    {
        StartCoroutine(uziReload(source));
    }
    IEnumerator uziReload(AudioSource source)
    {
        yield return new WaitForSeconds(0.15f);
        source.PlayOneShot(magOut, volume);
        yield return new WaitForSeconds(0.1f);
        source.PlayOneShot(magIn, volume);
    }
    public void AKReload(AudioSource source)
    {
        StartCoroutine(akReload(source));
    }
    IEnumerator akReload(AudioSource source)
    {
        yield return new WaitForSeconds(0.25f);
        source.PlayOneShot(magOut, volume);
        yield return new WaitForSeconds(0.25f);
        source.PlayOneShot(magIn, volume);
    }
    public void PlayFootstep(AudioSource source)
    {
        source.PlayOneShot(footsteps[Random.Range(0, footsteps.Length)], volume);
    }
    public void PlayPlayerPain()
    {
        playerAudioSource.PlayOneShot(playerPain[Random.Range(0, playerPain.Length)], volume);
    }

    public bool CanPlayMoan()
    {
        return currentMoans < maxSimultaneousMoans;
    }
    public void NotifyMoanStarted(float duration)
    {
        currentMoans++;
        StartCoroutine(ResetMoanAfter(duration));
    }
    private IEnumerator ResetMoanAfter(float time)
    {
        yield return new WaitForSeconds(time);
        currentMoans--;
    }
}
