using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class underwater_mute : MonoBehaviour
{
    public AudioSource audioSource;
    public Button muteButton;
    private bool isMuted = false;

    private void Start()
    {
        muteButton.onClick.AddListener(ToggleMute);
        UpdateButtonText();
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        audioSource.mute = isMuted;

        UpdateButtonText();
    }

    private void UpdateButtonText()
    {
        Text buttonText = muteButton.GetComponentInChildren<Text>();
        if (buttonText != null)
        {
            buttonText.text = isMuted ? "Unmute" : "Mute";
        }
    }

    // Dont let spacebar mute or unmute the audio
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}


