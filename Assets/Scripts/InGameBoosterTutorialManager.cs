using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class InGameBoosterTutorialManager : MonoBehaviour
{
    private const string BuyBoosterTutorial = "BuyBoosterTutorial";
    private const string InGameBoosterTutorial = "InGameBoosterTutorial";

    public GameObject tutorialPanel;
    public GameObject timeFreezeTutorial;
    public GameObject bombTutorial;
    public GameObject nextButton;
    public GameObject closeButton;

    public VideoPlayer videoPlayer;

    private void Awake()
    {
        StartCoroutine(startTutorial());
    }
    IEnumerator startTutorial()
    {
        yield return null;

        if(PlayerPrefs.GetInt(BuyBoosterTutorial, 0) == 0 || PlayerPrefs.GetInt(InGameBoosterTutorial, 0) != 0)
        {
            tutorialPanel.SetActive(false);
            Destroy(this);
        }
        else
        {
            tutorialPanel.SetActive(true);
            FindObjectOfType<SpawnController>().enabled = false;

            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;

            stage1();
        }
    }
    void stage1()
    {
        nextButton.SetActive(true);
        closeButton.SetActive(false);

        timeFreezeTutorial.SetActive(true);
        bombTutorial.SetActive(false);
    }
    public void stage2()
    {
        videoPlayer.Play();

        nextButton.SetActive(false);
        closeButton.SetActive(true);

        timeFreezeTutorial.SetActive(false);
        bombTutorial.SetActive(true);
    }
    public void stage3()
    {
        tutorialPanel.SetActive(false);

        PlayerPrefs.SetInt(InGameBoosterTutorial, 1);
        PlayerPrefs.Save();

        FindObjectOfType<SpawnController>().enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
