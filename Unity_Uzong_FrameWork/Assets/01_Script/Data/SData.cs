using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using UnityEngine;

public class SData : SingletonBehabiour<SData>
{
    public void Initialize()
    {
        if (StoryData == null) StoryData = storyData.dataArray.ToDictionary(data => data.ID); // (초기 세팅) 데이터가 있다면 딕셔너리에 데이터 넣어줌
        if (StoryData == null) StoryData = storyData.dataArray.ToDictionary(data => data.ID);
        if (GalleryData == null) GalleryData = galleryData.dataArray.ToDictionary(data => data.ID);
        if (CharacterData == null) CharacterData = characterData.dataArray.ToDictionary(data => data.ID);
        if (PlaceData == null) PlaceData = placeData.dataArray.ToDictionary(data => data.ID);
        if (LocalizeEnviData == null) LocalizeEnviData = localizeEnviData.dataArray.ToDictionary(data => data.ID);
        if (LocalizeData == null) LocalizeData = localizeData.dataArray.ToDictionary(data => data.ID);
        if (LocalizeStoryData == null) LocalizeStoryData = localizeStoryData.dataArray.ToDictionary(data => data.ID);
        if (ResourceData == null) ResourceData = resourceData.dataArray.ToDictionary(data => data.ID);
    }

    [SerializeField] Story storyData; // [SerializeField] 데이터시트 시트변수명;
    public static Dictionary<int, StoryData> StoryData; // Dictionary<int, 데이터시트의 데이터> 데이터변수명
    public static StoryData GetStoryData(int id) // id 값을 기준으로 데이터를 찾는 함수
    {
        if (StoryData.TryGetValue(id, out var data)) return data;
        return null;
    }

    [SerializeField] Gallery galleryData;
    public static Dictionary<int, GalleryData> GalleryData;
    public static GalleryData GetGalleryData(int id)
    {
        if (GalleryData.TryGetValue(id, out var data)) return data;
        return null;
    }

    [SerializeField] Param paramData;
    public static ParamData[] ParamData { get { return Instance.paramData.dataArray; } }

    [SerializeField] Character characterData;
    public static Dictionary<int, CharacterData> CharacterData;
    public static CharacterData GetCharacterData(int _id)
    {
        if (CharacterData.TryGetValue(_id, out var data)) return data;
        return null;
    }

    [SerializeField] Place placeData;
    public static Dictionary<int, PlaceData> PlaceData;
    public static PlaceData GetPlaceData(int _id)
    {
        if (PlaceData.TryGetValue(_id, out var data)) return data;
        return null;
    }

    [SerializeField] LocalizeEnvi localizeEnviData;
    public static Dictionary<int, LocalizeEnviData> LocalizeEnviData;
    public static string GetLocalizeEnviData(int _id)
    {
        if (LocalizeEnviData.TryGetValue(_id, out var data))
        {
            if (GameManager.PData.Option.Localize == 0)
            {
                return data.KR;
            }
            else if (GameManager.PData.Option.Localize == 1)
            {
                return data.EN;
            }
            else if (GameManager.PData.Option.Localize == 2)
            {
                return data.JP;
            }
        }
        return null;
    }

    [SerializeField] Localize localizeData;
    public static Dictionary<int, LocalizeData> LocalizeData;
    public static string GetLocalizeData(int _id)
    {
        if (LocalizeData.TryGetValue(_id, out var data))
        {
            if (GameManager.PData.Option.Localize == 0)
            {
                return data.KR;
            }
            else if (GameManager.PData.Option.Localize == 1)
            {
                return data.EN;
            }
            else if (GameManager.PData.Option.Localize == 2)
            {
                return data.JP;
            }
        }
        return null;
    }

    [SerializeField] LocalizeStory localizeStoryData;
    public static Dictionary<int, LocalizeStoryData> LocalizeStoryData;

    public static string GetLocalizeStoryData(int _id)
    {
        if (LocalizeStoryData.TryGetValue(_id, out var data))
        {
            if (GameManager.PData.Option.Localize == 0)
            {
                return data.KR;
            }
            else if (GameManager.PData.Option.Localize == 1)
            {
                return data.EN;
            }
            else if (GameManager.PData.Option.Localize == 2)
            {
                return data.JP;
            }
        }
        return null;
    }

    [SerializeField] Resource resourceData;
    public static Dictionary<int, ResourceData> ResourceData;
    public static ResourceData GetResourceData(int _id)
    {
        if (ResourceData.TryGetValue(_id, out var data)) return data;
        return null;
    }
}
