using UnityEngine;
using System.Collections;


public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicScore;
    [SerializeField] AudioSource SFXSource;

    public AudioClip background;
    public AudioClip hover;
    public AudioClip pressed;
    public AudioClip tick;

    private void Start()
    {
        musicScore.clip = background;
        musicScore.loop = true;
        musicScore.Play();
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

}
