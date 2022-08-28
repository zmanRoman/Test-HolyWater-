using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioListener))]
public class MusicHandler : MonoBehaviour
{/// <summary>
 /// music state handling
 /// </summary>
    [SerializeField] private AudioClip music;
    [SerializeField] private AudioClip fx;

    [field: SerializeField] public float FXVolume { get; private set; } = 1f;
    [field: SerializeField] public float MusicVolume { get; private set; } = 1f;
    [SerializeField]private AudioSource _audioSourceMusic;
    [SerializeField]private AudioSource _audioSourceFX;
    public static MusicHandler Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        SetMusicVolume(SaveLoadSceneData.Instance.LoadMusicVolume);
        SetFXVolume(SaveLoadSceneData.Instance.LoadFXVolume);
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
        
        if(_audioSourceMusic.volume != 0f)
            _audioSourceMusic.Play();
    }
    // Start is called before the first frame update
    public void PlayFX()
    {
        _audioSourceFX.PlayOneShot(fx, FXVolume);
    }

    public void SwitcherFx()
    {
        FXVolume = FXVolume == 1f ? 0f : 1f;
    }
    
    public void SwitcherMusic()
    {
        if (MusicVolume >= 0.5f)
        {
            StartCoroutine(SmoothChangeVolume(0f, 0.7f));
        }
        else
        {
            StartCoroutine(SmoothChangeVolume(1f, 0.7f));
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
