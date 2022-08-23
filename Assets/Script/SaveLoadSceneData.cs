using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class SaveLoadSceneData : MonoBehaviour
{
    private const string KeySaveData = "saveData";
    private SceneData _sceneData;
    [SerializeField]private List<CardData> cardData;
    [SerializeField] private int currentItemSlider;
    [SerializeField] private int loadVolumeMusic;
    [SerializeField] private int loadVolumeFx;
    public static SaveLoadSceneData Instance { get; private set; }

    public int CurrentItemSlider
    {
        get
        {
            GetSaveData();
            return  currentItemSlider;
        }
    }

    public int LoadMusicVolume
    {
        get
        {
            GetSaveData();
            return  loadVolumeMusic;
        }
    }
    
    public int LoadFXVolume
    {
        get
        {
            GetSaveData();
            return  loadVolumeFx;
        }
    }
    
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

    public SceneData GetSaveData()
    {
        if (_sceneData == null)
        {
            var data = PlayerPrefs.GetString(KeySaveData);

            if (data != string.Empty)
            {
                _sceneData = JsonUtility.FromJson<SceneData>(data);
            }
        }

        if (_sceneData == null)
        {
            loadVolumeFx = 1;
            loadVolumeMusic = 1;
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
    
    private void OnDestroy()
    {
       
        _sceneData = new SceneData()
        {
            cardData = cardData.ToArray(),
            currentItemInSliderUp =  SlideUpHandler.Instance.Current,
            volumeMusic = (int)MusicHandler.Instance.MusicVolume,
            volumeFx = (int)MusicHandler.Instance.FXVolume
            
        };
        
        var json = JsonUtility.ToJson( _sceneData);
        PlayerPrefs.SetString(KeySaveData, json);
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
    public float localPositionX;
    public float localPositionY;
}
