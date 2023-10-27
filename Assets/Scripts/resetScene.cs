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

    private static float startTime;
    private static bool timerActive = false;
    private float pausedTime;

    void Start()
    {
        if (timerText != null)
        {
            timerText.gameObject.SetActive(timerActive);
        }

        if (videoDisplay != null)
        {
            videoDisplay.gameObject.SetActive(timerActive);
        }

        if (timerActive)
        {
            startTime = Time.time;
            if (videoPlayer != null)
            {
                videoPlayer.Play();
            }
        }
        else
        {
            if (videoPlayer != null)
            {
                videoPlayer.Pause();
            }
        }
    }

    void Update()
    {
        // Reset Scene and Timer
        if (Input.GetKeyDown(KeyCode.O))
        {
            timerActive = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Reset Scene
        if (Input.GetKeyDown(KeyCode.R))
        {
            timerActive = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // Update Timer
        if (timerText != null && timerActive)
        {
            float timeSinceStart = Time.time - startTime;
            timerText.text = "Time: " + timeSinceStart.ToString("F2") + "s";
        }

        if (gorb != null && !gorb.activeInHierarchy)
        {
            pausedTime = Time.time - startTime;
            if (videoPlayer != null)
            {
                videoPlayer.Pause();
            }
        }
    }
}
