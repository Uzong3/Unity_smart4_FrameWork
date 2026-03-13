using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEditor.Overlays;
using UnityEngine;

[System.Serializable]
public class PlayerData : ISerializationCallbackReceiver
{

    public void Initialize()
    {
        Option = new PlayerOption();
        Parameter = new ParameterData();
        StoryHistory = new List<int>();
        Gallery = new Dictionary<int, bool>();

        foreach (var item in SData.GalleryData)
        {
            Gallery.Add(item.Key, false);
        }

        NowStoryID = 10001;

        SaveDatas = new SaveData[EnviData.SaveCount];
        for (int i = 0; i < SaveDatas.Length; i++)
        {
            SaveDatas[i] = new SaveData();
        }
    }

    public SaveData[] SaveDatas;
    public PlayerOption Option;
    public ParameterData Parameter;

    public Dictionary<int, bool> Gallery = new Dictionary<int, bool>();
    [SerializeField] private List<int> galleryKeys = new List<int>();
    [SerializeField] private List<bool> galleryValues = new List<bool>();

    public void OnBeforeSerialize()
    {
        galleryKeys.Clear();
        galleryValues.Clear();
        foreach (var pair in Gallery)
        {
            galleryKeys.Add(pair.Key);
            galleryValues.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        Gallery = new Dictionary<int, bool>();
        for (int i = 0; i < Math.Min(galleryKeys.Count, galleryValues.Count); i++)
        {
            Gallery[galleryKeys[i]] = galleryValues[i];
        }
    }

    public int nowStoryID;
    public int NowStoryID 
    { 
        get { return nowStoryID; }
        set
        {
            nowStoryID = value;
            StoryHistory.Add(value);
            var data = SData.GetStoryData(nowStoryID).Gallery;
             if (Gallery.TryGetValue(data, out var gallery))
            {
                Gallery[data] = true;
            }
        }
    }
    public List<int> StoryHistory;

    public void Load(int _loadIndex)
    {
        nowStoryID = SaveDatas[_loadIndex].NowStoryID;
        StoryHistory.Clear();
        StoryHistory.AddRange(SaveDatas[_loadIndex].StoryHistory);
    }
}

[System.Serializable]
public class SaveData
{
    public SaveData()
    {
        isActive = false;
        NowStoryID = 0;
        StoryHistory = new List<int>();
    }

    public bool isActive;
    public int NowStoryID;
    public List<int> StoryHistory;

    [SerializeField] private string saveDateStr;

    public DateTime SaveDate
    {
        get => string.IsNullOrEmpty(saveDateStr) ? DateTime.Now : DateTime.Parse(saveDateStr);
        set => saveDateStr = value.ToString();
    }

    public void Save()
    {
        isActive = true;
        NowStoryID = StoryControl.Instance.NowStoryID;
        SaveDate = DateTime.Now;

        StoryHistory.Clear();
        StoryHistory.AddRange(GameManager.PData.StoryHistory);
        GameManager.Instance.SaveData();
    }
}


[System.Serializable]
public class PlayerOption
{
    public PlayerOption()
    {
        Localize = 0;
        use60Frame = true;
        BGMVol = 50;
        VoiceVol = 50;
        SFXVol = 50;
        Haptic = true;
    }

    public int Localize;
    public bool use60Frame;
    public int BGMVol;
    public int VoiceVol;
    public int SFXVol;
    public bool Haptic;
}

[System.Serializable]
public class ParameterData : ISerializationCallbackReceiver
{
    public Dictionary<int, int> ParamDict = new Dictionary<int, int>();

    [SerializeField] private List<int> keys = new List<int>();
    [SerializeField] private List<int> values = new List<int>();

    public void OnBeforeSerialize()
    {
        keys.Clear();
        values.Clear();
        foreach (var pair in ParamDict)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    public void OnAfterDeserialize()
    {
        ParamDict = new Dictionary<int, int>();
        for (int i = 0; i < Math.Min(keys.Count, values.Count); i++)
        {
            ParamDict[keys[i]] = values[i];
        }
    }

    public void SetParameter(int _type, int _value)
    {
        if (ParamDict.TryGetValue(_type, out int value))
        {
            value += _value;
        }
        else
        {
            ParamDict.Add(_type, _value);
        }
    }

    public int GetParameter(int _type)
    {
        if (ParamDict.TryGetValue(_type, out int value)) return value;
        return 0;
    }
}