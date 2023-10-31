using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using UnityEngine.UI;
using TMPro;

public class resetScene : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public VideoPlayer videoPlayer;
    public RawImage videoDisplay;
    public GameObject gorb;

    private float startTime;
    private static bool timerActive = false;
    private float pausedTime = -1f;
    private bool gorbDefeated = false;


    void Start()
    {
        if (timerActive)  // If timer should be active, start it
        {
            StartTimer();
        }
        UpdateUIVisibility();
        Debug.Log("Script Started");
    }

    void Update()
    {
        // Reset Scene and Timer
        if (Input.GetKeyDown(KeyCode.O))
        {
            timerActive = true;
            gorbDefeated = false; // Reset gorbDefeated flag
            StartTimer();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Reset Scene
        if (Input.GetKeyDown(KeyCode.R))
        {
            timerActive = false;
            gorbDefeated = false; // Reset gorbDefeated flag
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Update Timer
        if (timerText != null && timerActive && !gorbDefeated)
        {
            float timeSinceStart = Time.time - startTime;
            timerText.text = "Time: " + timeSinceStart.ToString("F2") + "s";
        }

        if (gorb == null || !gorb.activeInHierarchy)
        {
            if (!gorbDefeated)
            {
                pausedTime = Time.time - startTime;
                gorbDefeated = true; // Set gorbDefeated flag to true
            }

            timerText.text = "Time: " + pausedTime.ToString("F2") + "s";

            if (videoPlayer != null)
            {
                videoPlayer.Pause();
            }
        }
    }



    private void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void StartTimer()
    {
        startTime = Time.time;
        timerActive = true;
        UpdateUIVisibility();
        if (videoPlayer != null)
        {
            videoPlayer.Play();
        }
    }

    private void PauseTimer()
    {
        timerActive = false;
        UpdateUIVisibility();
        if (videoPlayer != null)
        {
            videoPlayer.Pause();
        }
    }

    private void UpdateTimer()
    {
        float timeSinceStart = Time.time - startTime;
        timerText.text = "Time: " + timeSinceStart.ToString("F2") + "s";
    }

    private void ShowPausedTime()
    {
        timerText.text = "Time: " + pausedTime.ToString("F2") + "s";
    }

    private bool IsGorbDefeated()
    {
        return gorb == null || !gorb.activeInHierarchy;
    }

    private void UpdateUIVisibility()
    {
        if (timerText != null)
        {
            timerText.gameObject.SetActive(timerActive);
        }

        if (videoDisplay != null)
        {
            videoDisplay.gameObject.SetActive(timerActive);
        }
    }
}
