using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource musicScore;
    [SerializeField] AudioSource SFXSource;

    public AudioClip background;
    public AudioClip hover;
    public AudioClip pressed;

    private void Start()
    {
        musicScore.clip = background;
        musicScore.loop = true;
        musicScore.Play();
    }

    public void PlaySFX(AudioClip clip) 
    {
        SFXSource.PlayOneShot(clip);
    }






}
