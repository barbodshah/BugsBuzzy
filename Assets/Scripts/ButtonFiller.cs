using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonFiller : MonoBehaviour
{
    public Image img;
    public Button btn;

    public float fillTime;

    private void Awake()
    {
        img.fillAmount = 0;
        StartCoroutine(fillImage());
    }

    public void Empty()
    {
        img.fillAmount = 0;
    }

    public void Refill()
    {
        btn.interactable = false;
        StartCoroutine(fillImage());
    }

    IEnumerator fillImage()
    {
        float f = 0;
        fillTime = LevelManager.BACKUP_COOLDOWN;

        while(f <= fillTime)
        {
            f += Time.deltaTime;
            img.fillAmount = f / fillTime;

            yield return null;
        }
        img.fillAmount = 1;
        btn.interactable = true;
    }
}
