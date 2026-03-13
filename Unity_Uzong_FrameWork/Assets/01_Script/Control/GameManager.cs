using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : SingletonBehabiour<GameManager>
{
    public static PlayerData PData;
    SCENE scene;
    public SCENE Scene => scene;

    [SerializeField] GameObject TitleScene;
    [SerializeField] GameObject GameScene;
    [SerializeField] GameObject GalleryScene;

    protected override void Awake()
    {
        base.Awake();

        Initialize();
    }

    void Initialize()
    {
        SData.Instance.Initialize();

        LoadData();
        
        Open();

        TitleControl.Instance.Initialize();
        SetScene(SCENE.TITLE);
    }

    void Open()
    {
        SoundManager.Instance.Open(PData);
        ResourceManager.Instance.Open(PData);
        TitleControl.Instance.Open(PData);
        StoryControl.Instance.Open(PData);
    }

    public void SaveData()
    {
        string jsonString = JsonUtility.ToJson(PData);
        PlayerPrefs.SetString("PData", jsonString);
    }

    public bool LoadData()
    {
        if(PlayerPrefs.HasKey("PData"))
        {
            string jsonString = PlayerPrefs.GetString("PData");
            PData = JsonUtility.FromJson<PlayerData>(jsonString);
            return true;
        }
        else
        {
            UnityEngine.Debug.Log("<color=red>Create New Data</color>");
            PData = new PlayerData();
            PData.Initialize();
            SaveData();
            return false;
        }
    }

    public void ChangeScene(SCENE _scene, Action _before = null, Action _after = null)
    {
        if(Time.timeScale < 1) Time.timeScale = 1;
        SoundManager.Instance.StopBGM(false);
        UIControl.Instance.FadeEffect(() => { if(_before!=null)_before.Invoke(); SetScene(_scene); }, _after, EnviData.FadeSpeed);
    }

    public void SetScene(SCENE _scene)
    {
        scene = _scene;

        TitleScene.SetActive(SCENE.TITLE == _scene);
        GameScene.SetActive(SCENE.GAME == _scene);
        GalleryScene.SetActive(SCENE.GALLERY == _scene);
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
}
