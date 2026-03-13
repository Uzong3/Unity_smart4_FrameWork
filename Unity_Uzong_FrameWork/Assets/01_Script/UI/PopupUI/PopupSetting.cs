using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupSetting : PopupUI
{
    [SerializeField] TextMeshProUGUI txt_DefaultSetting;

    [SerializeField] GameObject ReturnObj;
    [SerializeField] Button btn_ReturnTitle;


    [Header("언어설정")]
    [SerializeField] TextMeshProUGUI txt_LanguageSetting;
    [SerializeField] Button btn_KR;
    [SerializeField] GameObject Obj_KR;
    [SerializeField] Button btn_EN;
    [SerializeField] GameObject Obj_EN;
    [SerializeField] Button btn_JP;
    [SerializeField] GameObject Obj_JP;


    [Header("프레임")]
    [SerializeField] TextMeshProUGUI txt_FrameSetting;
    [SerializeField] Button btn_30;
    [SerializeField] GameObject Obj_30;
    [SerializeField] Button btn_60;
    [SerializeField] GameObject Obj_60;

    [Header("배경음")]
    [SerializeField] TextMeshProUGUI txt_BGM;
    [SerializeField] Slider BGMBar;
    [SerializeField] TextMeshProUGUI txt_BGMValue;

    [Header("효과음")]
    [SerializeField] TextMeshProUGUI txt_SFX;
    [SerializeField] Slider SFXBar;
    [SerializeField] TextMeshProUGUI txt_SFXValue;

    [Header("보이스")]
    [SerializeField] GameObject voiceMenu;
    [SerializeField] TextMeshProUGUI txt_Voice;
    [SerializeField] Slider voiceBar;
    [SerializeField] TextMeshProUGUI txt_VoiceValue;

    [Header("진동")]
    [SerializeField] GameObject hapticMenu;
    [SerializeField] TextMeshProUGUI txt_Haptic;
    [SerializeField] Button btn_HapticOn;
    [SerializeField] GameObject Obj_HapticOn;
    [SerializeField] Button btn_HapticOff;
    [SerializeField] GameObject Obj_HapticOff;

    protected override void Awake()
    {
        base.Awake();

        btn_KR.onClick.AddListener(() => SoundManager.Instance.PlaySFX((int)ENVI.BUTTON_SFX));
        btn_EN.onClick.AddListener(() => SoundManager.Instance.PlaySFX((int)ENVI.BUTTON_SFX));
        btn_JP.onClick.AddListener(() => SoundManager.Instance.PlaySFX((int)ENVI.BUTTON_SFX));
        btn_30.onClick.AddListener(() => SoundManager.Instance.PlaySFX((int)ENVI.BUTTON_SFX));
        btn_60.onClick.AddListener(() => SoundManager.Instance.PlaySFX((int)ENVI.BUTTON_SFX));
        btn_HapticOn.onClick.AddListener(() => SoundManager.Instance.PlaySFX((int)ENVI.BUTTON_SFX));
        btn_HapticOff.onClick.AddListener(() => SoundManager.Instance.PlaySFX((int)ENVI.BUTTON_SFX));
        btn_ReturnTitle.onClick.AddListener(() => SoundManager.Instance.PlaySFX((int)ENVI.BUTTON_SFX));


        btn_KR.onClick.AddListener(() => ChangeLanguage(0));
        btn_EN.onClick.AddListener(() => ChangeLanguage(1));
        btn_JP.onClick.AddListener(() => ChangeLanguage(2));

        btn_30.onClick.AddListener(() => { ChangeFrame(false); Application.targetFrameRate = 30; } );
        btn_60.onClick.AddListener(() => { ChangeFrame(true); Application.targetFrameRate = 60; });
        BGMBar.onValueChanged.AddListener(newValue => ChangeBGM(newValue));
        voiceBar.onValueChanged.AddListener(newValue => ChangeVoice(newValue));
        SFXBar.onValueChanged.AddListener(newValue => ChangeSFX(newValue));
        btn_HapticOn.onClick.AddListener(() => ChangeHaptic(true));
        btn_HapticOff.onClick.AddListener(() => ChangeHaptic(false));

        btn_ReturnTitle.onClick.AddListener(ReturnTitle);
    }

    public override void ActivePopUp()
    {
        base.ActivePopUp();

    }

    public override void Initialize()
    {
        var option = GameManager.PData.Option;
        RefreshLanguageUI(option.Localize);
        RefreshFrameUI(option.use60Frame);
        RefreshBGMUI(option.BGMVol);
        RefreshVoiceUI(option.VoiceVol);
        RefreshSFXUI(option.SFXVol);
        RefreshHapticUI(option.Haptic);
        RefreshUI();

        voiceMenu.SetActive(EnviData.UseVoice);
        hapticMenu.SetActive(EnviData.UseHaptic);

        base.Initialize();

        ReturnObj.SetActive(GameManager.Instance.Scene == SCENE.GAME);
        Time.timeScale = 0;
    }

    public void RefreshUI()
    {
        txt_DefaultSetting.text = SData.GetLocalizeEnviData((int)LOCALIZE.DEFAULT_SETTING);

        txt_LanguageSetting.text = SData.GetLocalizeEnviData((int)LOCALIZE.LANGUAGE);
        txt_FrameSetting.text = SData.GetLocalizeEnviData((int)LOCALIZE.FRAMERATE);
        txt_BGM.text = SData.GetLocalizeEnviData((int)LOCALIZE.BGM);
        txt_SFX.text = SData.GetLocalizeEnviData((int)LOCALIZE.SFX);
        txt_Voice.text = SData.GetLocalizeEnviData((int)LOCALIZE.VOICE);
        txt_Haptic.text = SData.GetLocalizeEnviData((int)LOCALIZE.HAPTIC);


    }

    void ChangeLanguage(int _value)
    {
        GameManager.PData.Option.Localize = _value;
        RefreshLanguageUI(_value);
        RefreshUI();
        UIControl.Instance.SettingPopup.RefreshUI();
    }

    void RefreshLanguageUI(int _value)
    {
        switch (_value)
        {
            case 0:
                Obj_KR.SetActive(true);
                Obj_EN.SetActive(false);
                Obj_JP.SetActive(false);
                break;
            case 1:
                Obj_KR.SetActive(false);
                Obj_EN.SetActive(true);
                Obj_JP.SetActive(false);
                break;
            case 2:
                Obj_KR.SetActive(false);
                Obj_EN.SetActive(false);
                Obj_JP.SetActive(true);
                break;
            default:
                break;
        }

        switch (GameManager.Instance.Scene)
        {
            case SCENE.TITLE:
                TitleControl.Instance.RefreshUI();
                break;
            case SCENE.GAME:
                StoryControl.Instance.SetDialogue();
                break;
            default:
                break;
        }
    }

    void ChangeFrame(bool _use60Frame)
    {
        GameManager.PData.Option.use60Frame = _use60Frame;
        RefreshFrameUI(_use60Frame);
    }

    void RefreshFrameUI(bool _use60Frame)
    {
        Obj_30.SetActive(!_use60Frame);
        Obj_60.SetActive(_use60Frame);
    }

    void ChangeBGM(float _value)
    {
        SoundManager.Instance.SetBGMVol(_value);

        int value = Mathf.RoundToInt(_value * 100);
        GameManager.PData.Option.BGMVol = value;
        RefreshBGMUI(value);
    }

    public void RefreshBGMUI(float _value)
    {
        BGMBar.value = _value / 100;
        txt_BGMValue.text = _value.ToString();
    }

    void ChangeSFX(float _value)
    {
        SoundManager.Instance.SetSFXVol(_value);
        int value = Mathf.RoundToInt(_value * 100);
        GameManager.PData.Option.SFXVol = value;
        RefreshSFXUI(value);
    }
    public void RefreshSFXUI(float _value)
    {
        SFXBar.value = _value / 100;
        txt_SFXValue.text = _value.ToString();
    }

    void ChangeVoice(float _value)
    {
        SoundManager.Instance.SetVoiceVol(_value);

        int value = Mathf.RoundToInt(_value * 100);
        GameManager.PData.Option.VoiceVol = value;
        RefreshVoiceUI(value);
    }
    public void RefreshVoiceUI(float _value)
    {
        voiceBar.value = _value / 100;
        txt_VoiceValue.text = _value.ToString();
    }

    void ChangeHaptic(bool _isActive)
    {
        GameManager.PData.Option.Haptic = _isActive;
        RefreshHapticUI(_isActive);
    }
    void RefreshHapticUI(bool _isActive)
    {
        if (_isActive)
        {
            Obj_HapticOn.SetActive(true);
            Obj_HapticOff.SetActive(false);
        }
        else
        {
            Obj_HapticOn.SetActive(false);
            Obj_HapticOff.SetActive(true);
        }
    }

    void ReturnTitle()
    {
        UIControl.Instance.MessagePopup.Initialize(() =>
        {
            GameManager.Instance.ChangeScene(SCENE.TITLE, TitleControl.Instance.Initialize);
            ClosePopup();
        },
        SData.GetLocalizeEnviData((int)LOCALIZE.MAINMENU),
        SData.GetLocalizeEnviData((int)LOCALIZE.MAINMENU_INFO),
        SData.GetLocalizeEnviData((int)LOCALIZE.MAINMENU)
        );
    }

    public void ClosePopup()
    {
        Close();
    }

    protected override void Close()
    {
        Time.timeScale = 1;

        base.Close();
    }

    public override void DeActivePopup()
    {
        base.DeActivePopup();
    }
}
