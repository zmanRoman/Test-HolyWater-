using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class SaveLoadSceneData : MonoBehaviour
{
    private const string KeySaveData = "saveData";
    private const int On = 1;

    [SerializeField] private List<CardData> cardData;
    [SerializeField] private int currentItemSlider;
    [SerializeField] private int loadVolumeMusic;
    [SerializeField] private int loadVolumeFx;
    private MusicHandler _musicHandler;

    private SceneData _sceneData;
    private SlideUpHandler _slideUpHandler;

    private SaveLoadSceneData()
    {
    }

    public int CurrentItemSlider
    {
        get
        {
            GetSaveData();
            return currentItemSlider;
        }
    }

    public int LoadMusicVolume
    {
        get
        {
            GetSaveData();
            return loadVolumeMusic;
        }
    }

    public int LoadFXVolume
    {
        get
        {
            GetSaveData();
            return loadVolumeFx;
        }
    }

    private void Start()
    {
        _slideUpHandler = FindObjectOfType<SlideUpHandler>();
        _slideUpHandler.LoadSaveCurrentItem(this);
        _musicHandler = FindObjectOfType<MusicHandler>();
        _musicHandler.LoadSaveVolumeData(this);
    }

    private void OnDestroy()
    {
        _sceneData = new SceneData
        {
            cardData = cardData.ToArray(),
            currentItemInSliderUp = _slideUpHandler.Current,
            volumeMusic = (int)_musicHandler.MusicVolume,
            volumeFx = (int)_musicHandler.FXVolume
        };

        var json = JsonUtility.ToJson(_sceneData);
        PlayerPrefs.SetString(KeySaveData, json);
    }

    public SceneData GetSaveData()
    {
        if (_sceneData == null)
        {
            var data = PlayerPrefs.GetString(KeySaveData);

            if (data != string.Empty) _sceneData = JsonUtility.FromJson<SceneData>(data);
        }

        if (_sceneData == null)
        {
            loadVolumeFx = On;
            loadVolumeMusic = On;
        }
        else
        {
            loadVolumeFx = _sceneData.volumeFx;
            loadVolumeMusic = _sceneData.volumeMusic;
            currentItemSlider = _sceneData.currentItemInSliderUp;
        }

        return _sceneData;
    }

    public void SetCardData(ref CardData data)
    {
        cardData.Add(data);
    }
}

[Serializable]
public sealed class SceneData
{
    public int volumeMusic;
    public int volumeFx;
    public int currentItemInSliderUp;
    public CardData[] cardData;
}


[Serializable]
public sealed class CardData
{
    public int rowNum;
    public string cardImageName;
    public int indexInList;
    public int isDestroy;
}