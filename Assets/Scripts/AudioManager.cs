using UnityEngine;
using System.Collections;


public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicScore;
    [SerializeField] AudioSource SFXSource;
    public static AudioManager instance;
    public AudioClip background;
    public AudioClip hover;
    public AudioClip pressed;
    public AudioClip tick;
    public AudioClip fogPop;

    private void Start()
    {
        musicScore.clip = background;
        musicScore.loop = true;
        musicScore.Play();
        instance = this;
    }

    public void PlayHover() 
    {
        SFXSource.PlayOneShot(hover);
    }
    public void PlayPress()
    {
        SFXSource.PlayOneShot(pressed);
    }
    public void PlayClock()
    {
        SFXSource.PlayOneShot(tick);
    }
    public void PlayFogPop()
    {
        SFXSource.PlayOneShot(fogPop);
    }
}
