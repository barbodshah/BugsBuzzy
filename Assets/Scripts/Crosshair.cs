using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    public static Crosshair instance;

    public GameObject marker;
    public Animator markerAnimator;
    public float markerTimer;

    public Image[] markerIMG;

    Coroutine deactivate;
    Coroutine colorChange;

    private void Awake()
    {
        instance = this;
    }
    public void Kill()
    {
        print("Kill");
        foreach(Image image in markerIMG)
        {
            image.color = Color.red;
        }
        if(colorChange != null)
        {
            StopCoroutine(colorChange);
        }
        colorChange = StartCoroutine(ColorTimer());
    }
    public void NoKillHit()
    {
        print("NoKillHit");
        foreach (Image image in markerIMG)
        {
            image.color = Color.white;
        }
        if (colorChange != null)
        {
            StopCoroutine(colorChange);
        }
    }
    IEnumerator ColorTimer()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (Image image in markerIMG)
        {
            image.color = Color.white;
        }
    }
    public void ActivateMarker()
    {
        marker.SetActive(true);

        if (markerAnimator.GetCurrentAnimatorStateInfo(0).IsTag("marker"))
        {
            markerAnimator.Play("marker 0");
        }
        else
        {
            markerAnimator.Play("marker");
        }

        if (deactivate != null)
        {
            StopCoroutine(deactivate);
        }
        deactivate = StartCoroutine(MarkerTimer());
    }
    IEnumerator MarkerTimer()
    {
        yield return new WaitForSeconds(markerTimer);
        marker.SetActive(false);
    }
}
