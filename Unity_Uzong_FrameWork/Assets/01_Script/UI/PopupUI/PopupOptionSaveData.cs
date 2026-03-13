using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PopupOptionSaveData : MonoBehaviour
{
    bool isSaveData;    

    int saveID;
    public int SaveID { get { return saveID; } }
    [SerializeField] TextMeshProUGUI txt_saveID;
    
    [SerializeField] Image img_bg;
    [SerializeField] TextMeshProUGUI txt_place;
    [SerializeField] TextMeshProUGUI txt_saveDate;
    [SerializeField] TextMeshProUGUI txt_story;

    [SerializeField] GameObject[] infoDatas;

    [SerializeField] Button btn_save;

    void Awake()
    {
        btn_save.onClick.AddListener(() => SoundManager.Instance.PlaySFX((int)ENVI.BUTTON_SFX));

        btn_save.onClick.AddListener(OnClick);
    }

    public void Initialize(int _id)
    {
        saveID = _id;
    }

    public void RefreshUI(bool _isSaveData)
    {
        isSaveData = _isSaveData;

        var data = GameManager.PData.SaveDatas[saveID];

        if(data.isActive)
        {
            var storyData = SData.GetStoryData(data.NowStoryID);
            var BGData = SData.GetPlaceData(storyData.BG);

            txt_saveID.text = $"#{saveID+1:D2}";

            img_bg.sprite = ResourceManager.Instance.GetImage(BGData.Resource);
            txt_place.text = SData.GetLocalizeData(BGData.Name);
            txt_saveDate.text = data.SaveDate.ToString("yyyy-MM-dd HH:mm:ss");
            txt_story.text = SData.GetLocalizeStoryData(storyData.Text);

            infoDatas[0].SetActive(true);
            infoDatas[1].SetActive(false);
        }
        else
        {
            infoDatas[0].SetActive(false);
            infoDatas[1].SetActive(true);
        }
    }

    void OnClick()
    {
        if(isSaveData)
        {
            // 저장팝업 & 저장
            UIControl.Instance.MessagePopup.Initialize(
                SaveData, 
                SData.GetLocalizeEnviData((int)LOCALIZE.SAVE), 
                SData.GetLocalizeEnviData((int)LOCALIZE.SAVEINFO), 
                SData.GetLocalizeEnviData((int)LOCALIZE.SAVE)
                );
        }
        else
        {
            // 불러오기 팝업 & 불러오기
            if (GameManager.PData.SaveDatas[saveID].isActive)
            {
                UIControl.Instance.MessagePopup.Initialize(
                    LoadData,
                    SData.GetLocalizeEnviData((int)LOCALIZE.LOAD),
                    SData.GetLocalizeEnviData((int)LOCALIZE.LOADINFO),
                    SData.GetLocalizeEnviData((int)LOCALIZE.LOAD)
    );
            }

        }
    }

    void SaveData()
    {
        GameManager.PData.SaveDatas[saveID].Save();
        RefreshUI(true);
    }

    public void LoadData()
    {
        UIControl.Instance.SaveLoadPopup.ClosePopup();

        GameManager.Instance.ChangeScene(SCENE.GAME,
            () =>
            {
                GameManager.PData.Load(saveID);
                StoryControl.Instance.Initialize(GameManager.PData.NowStoryID);
            },
            StoryControl.Instance.StartGame
            );
    }
}
