using UnityEngine;

public class BackgroundAudio : MonoBehaviour {

    public BackgroundAudio()
    {
        Singles.BackgroundAudio = this;
    }

    private float _backgroundAudioSourceVolume;

    public AudioSource BackgroundAudioSource;

    public void Start()
    {
        _backgroundAudioSourceVolume = BackgroundAudioSource.volume;
    }

    public void PlayBackground(AudioClip clip)
    {
        if (BackgroundAudioSource.clip != clip)
        {
            BackgroundAudioSource.clip = clip;

            if (clip != null)
            {
                BackgroundAudioSource.time = Random.Range(0f, clip.length - 0.1f);
                BackgroundAudioSource.volume = 0;
                BackgroundAudioSource.Play();
            }
        }
    }

    private void Update()
    {
        //Fade in background sound
        if(BackgroundAudioSource.volume < _backgroundAudioSourceVolume)
        {
            BackgroundAudioSource.volume = BackgroundAudioSource.volume + 0.005f;
        }

        ChooseBackground();
    }

    private void ChooseBackground()
    {
        if (Singles.PlayerController == null) return;

        if (Singles.PlayerController.SelectedBiome == null)
        {
            Singles.BackgroundAudio.PlayBackground(Singles.Audio.DesertWind);
        }
        else
        {
            Singles.BackgroundAudio.PlayBackground(Singles.PlayerController.SelectedBiome.Spec.BackgroundAudioClip);
        }
    }
}
