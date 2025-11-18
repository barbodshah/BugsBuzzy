using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboController : MonoBehaviour
{
    public static ComboController instance;

    public int currentCombo = 0;
    public int currentFastCombo = 0;

    public float comboBreaker;
    public float fastComboBreaker;

    Coroutine comboCoroutine;
    Coroutine fastComboCoroutine;

    public bool inCombo = false;
    public bool inFastCombo = false;


    private void Awake()
    {
        instance = this;
    }

    public void ZombieKilled()
    {
        inCombo = true;
        inFastCombo = true;

        currentCombo++;
        currentFastCombo++;

        if (currentCombo >= 2) UIController.instance.UpdateCombo(currentCombo);
        if (currentFastCombo >= 2) UIController.instance.FastCombo(currentFastCombo);

        if(comboCoroutine != null) StopCoroutine(comboCoroutine);
        if(fastComboCoroutine != null) StopCoroutine(fastComboCoroutine);

        comboCoroutine = StartCoroutine(ComboTimer());
        fastComboCoroutine = StartCoroutine(FastComboTimer());
    }
    IEnumerator ComboTimer()
    {
        yield return new WaitForSeconds(comboBreaker);

        currentCombo = 0;
        inCombo = false;
    }
    IEnumerator FastComboTimer()
    {
        yield return new WaitForSeconds(fastComboBreaker);

        currentFastCombo = 0;
        inFastCombo = false;
    }
}
