using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eat_sound : MonoBehaviour
{
    public static eat_sound Instance;

    public AudioSource pop;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        pop = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            pop.PlayOneShot(clip);
        }
    }
}
