using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioListener))]
public class MusicHandler : MonoBehaviour
{
    private const float On = 1, Off = 0, Duration = 0.7f;

    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip fx;
    [SerializeField] private AudioSource _audioSourceMusic;
    [SerializeField] private AudioSource _audioSourceFX;

    private MusicHandler()
    {
    }

    public float FXVolume { get; private set; } = 1f;
    public float MusicVolume { get; private set; } = 1f;

    public void LoadSaveVolumeData(SaveLoadSceneData saveLoadSceneData)
    {
        SetMusicVolume(saveLoadSceneData.LoadMusicVolume);
        SetFXVolume(saveLoadSceneData.LoadFXVolume);
        PlayMusic();
    }

    private void SetMusicVolume(float volume)
    {
        MusicVolume = volume;
        _audioSourceMusic.volume = volume;
    }

    private void SetFXVolume(float volume)
    {
        FXVolume = volume;
    }

    private void PlayMusic()
    {
        _audioSourceMusic.clip = music;
        _audioSourceMusic.volume = MusicVolume;

        if (_audioSourceMusic.volume != Off) _audioSourceMusic.Play();
    }

    // Start is called before the first frame update
    public void PlayFX()
    {
        _audioSourceFX.PlayOneShot(fx, FXVolume);
    }

    public void SwitcherFx()
    {
        FXVolume = FXVolume == On ? Off : On;
    }

    public void SwitcherMusic()
    {
        if (MusicVolume >= 0.5f)
        {
            StartCoroutine(SmoothChangeVolume(Off, Duration));
        }
        else
        {
            StartCoroutine(SmoothChangeVolume(On, Duration));
            _audioSourceMusic.Play();
        }
    }

    private IEnumerator SmoothChangeVolume(float endVolume, float time)
    {
        float currTime = 0;
        do
        {
            _audioSourceMusic.volume = Mathf.Lerp(MusicVolume, endVolume, currTime / time);
            currTime += Time.deltaTime;
            yield return null;
        } while (currTime <= time);

        MusicVolume = endVolume;
        _audioSourceMusic.volume = endVolume;
    }
}