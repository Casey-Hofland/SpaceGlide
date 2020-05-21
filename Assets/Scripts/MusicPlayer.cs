using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour 
{
    private static MusicPlayer instance = null;
    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (instance != null)
        {
            AudioClip instanceClip = instance.audioSource.clip;
            AudioClip thisClip = audioSource.clip;

            if (instanceClip == thisClip)
            {
                Destroy(gameObject);
                return;
            }
            else
            {
                Destroy(instance.gameObject);
            }
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
