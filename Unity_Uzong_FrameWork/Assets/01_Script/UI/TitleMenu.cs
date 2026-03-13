using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TitleMenu : MonoBehaviour
{
    [SerializeField] Button btn_Continue;
    [SerializeField] TextMeshProUGUI txt_Continue;
    [SerializeField] Button btn_NewStory;
    [SerializeField] TextMeshProUGUI txt_NewStory;
    [SerializeField] Button btn_LoadStory;
    [SerializeField] TextMeshProUGUI txt_LoadStory;
    [SerializeField] Button btn_Gallery;
    [SerializeField] TextMeshProUGUI txt_Gallery;
    [SerializeField] Button btn_Option;
    [SerializeField] TextMeshProUGUI txt_Option;
    [SerializeField] Button btn_Quit;
    [SerializeField] TextMeshProUGUI txt_Quit;

    void Awake()
    {
        btn_Continue.onClick.AddListener(() => SoundManager.Instance.PlaySFX((int)ENVI.BUTTON_SFX));
        btn_NewStory.onClick.AddListener(() => SoundManager.Instance.PlaySFX((int)ENVI.BUTTON_SFX));
        btn_LoadStory.onClick.AddListener(() => SoundManager.Instance.PlaySFX((int)ENVI.BUTTON_SFX));
        btn_Gallery.onClick.AddListener(() => SoundManager.Instance.PlaySFX((int)ENVI.BUTTON_SFX));
        btn_Option.onClick.AddListener(() => SoundManager.Instance.PlaySFX((int)ENVI.BUTTON_SFX));
        btn_Quit.onClick.AddListener(() => SoundManager.Instance.PlaySFX((int)ENVI.BUTTON_SFX));

        btn_Continue.onClick.AddListener(OnClickContinue);
        btn_NewStory.onClick.AddListener(OnClickNew);
        btn_LoadStory.onClick.AddListener(() => UIControl.Instance.SaveLoadPopup.ActiveMenu(true, false));
        btn_Gallery.onClick.AddListener(OnClickGallery);
        btn_Option.onClick.AddListener(UIControl.Instance.SettingPopup.Initialize);
        btn_Quit.onClick.AddListener(OnClickExit);
    }

    public void Initialize()
    {
        txt_Continue.text = SData.GetLocalizeEnviData((int)LOCALIZE.MAIN_CONTINUE);
        txt_NewStory.text = SData.GetLocalizeEnviData((int)LOCALIZE.MAIN_NEW_GAME);
        txt_LoadStory.text = SData.GetLocalizeEnviData((int)LOCALIZE.MAIN_LOAD_GAME);
        txt_Gallery.text = SData.GetLocalizeEnviData((int)LOCALIZE.MAIN_GALLERY);
        txt_Option.text = SData.GetLocalizeEnviData((int)LOCALIZE.MAIN_OPTION);
        txt_Quit.text = SData.GetLocalizeEnviData((int)LOCALIZE.MAIN_QUIT);

        btn_Continue.gameObject.SetActive(UIControl.Instance.SaveLoadPopup.LoadRecentData() != null);
    }


    void OnClickContinue()
    {
        UIControl.Instance.SaveLoadPopup.LoadRecentData().LoadData();
    }

    void OnClickNew()
    {
        GameManager.Instance.ChangeScene(
            SCENE.GAME,
            () => StoryControl.Instance.Initialize(10001),
            StoryControl.Instance.StartGame);
    }
    
    void OnClickGallery()
    {
        UIControl.Instance.GalleryPopup.Initialize();
    }

    void OnClickExit()
    {
        UIControl.Instance.MessagePopup.Initialize(Application.Quit,
            SData.GetLocalizeEnviData((int)LOCALIZE.MAIN_QUIT),
            SData.GetLocalizeEnviData((int)LOCALIZE.MAIN_QUIT_LOG),
            SData.GetLocalizeEnviData((int)LOCALIZE.MAIN_QUIT_SELECT));

    }
}
