using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StoryControl : ControlBase<StoryControl>
{
    int nowStoryID;
    public int NowStoryID { get { return nowStoryID; } }

    [Header("기본")]
    [SerializeField] TextMeshProUGUI txt_Name;
    [SerializeField] TextMeshProUGUI txt_Club;
    [SerializeField] TextMeshProUGUI txt_Dialogue;
    [SerializeField] Image img_BG;
    [SerializeField] Image[] img_Character;
    [SerializeField] Button btn_Next;

    [SerializeField] GameObject obj_TextBox;
    [SerializeField] GameObject[] obj_NameTextBox;

    [SerializeField] GameObject obj_SecondTextBox;
    [SerializeField] TextMeshProUGUI txt_SecondDialogue;

    [Header("효과")]
    Color activeColor = new Color(1, 1, 1, 1);
    Color deActiveColor = new Color(0.5f, 0.5f, 0.5f, 1);
    Coroutine cor_StoryEffect;
    bool typingText;
    public bool TypingText 
    { 
        get { return typingText;} 
        set 
        { 
            typingText = value;  
            if(isAuto && !typingText)
            {
                if (cor_Auto != null) StopCoroutine(cor_Auto);
                cor_Auto = StartCoroutine(AutoCoroutine());
            }
        } 
    } 
    Coroutine typingCoroutine;

    bool useEffect;

    [Header("옵션")]
    [SerializeField] Button btn_InGameSetting;
    [SerializeField] Button btn_Log;
    [SerializeField] Button btn_Save;
    [SerializeField] Button btn_Load;

    [SerializeField] Button btn_SPD;
    [SerializeField] GameObject obj_SPD_Active;
    bool isSPD;

    [SerializeField] Button btn_Auto;
    [SerializeField] GameObject obj_Auto_DeActive;
    [SerializeField] GameObject obj_Auto_Active;
    bool isAuto;
    Coroutine cor_Auto;

    [Header("선택지")]
    bool isChoice;
    [SerializeField] SelectBox selectBox;

    protected override void Start()
    {
        base.Start();

        btn_SPD.onClick.AddListener(() => SoundManager.Instance.PlaySFX((int)ENVI.BUTTON_SFX));
        btn_Auto.onClick.AddListener(() => SoundManager.Instance.PlaySFX((int)ENVI.BUTTON_SFX));
        btn_Log.onClick.AddListener(() => SoundManager.Instance.PlaySFX((int)ENVI.BUTTON_SFX));
        btn_InGameSetting.onClick.AddListener(() => SoundManager.Instance.PlaySFX((int)ENVI.BUTTON_SFX));
        btn_Save.onClick.AddListener(() => SoundManager.Instance.PlaySFX((int)ENVI.BUTTON_SFX));
        btn_Load.onClick.AddListener(() => SoundManager.Instance.PlaySFX((int)ENVI.BUTTON_SFX));

        btn_Next.onClick.AddListener(ClickNextButton);
        btn_Log.onClick.AddListener(() => UIControl.Instance.ActiveLog(true));
        btn_InGameSetting.onClick.AddListener(() => UIControl.Instance.ActiveSetting(true));
        btn_Save.onClick.AddListener(() => UIControl.Instance.ActiveSaveLoad(true, true));
        btn_Load.onClick.AddListener(() => UIControl.Instance.ActiveSaveLoad(true, false));
        btn_SPD.onClick.AddListener(OnClickSpeed);
        btn_Auto.onClick.AddListener(OnClickAuto);
    }

    public override void Open(PlayerData _pData)
    {
        base.Open(_pData);
    }
    
    public void Initialize(int _StoryID)
    {
        TypingText = false;
        useEffect = false;
        PData.StoryHistory.Clear();

        nowStoryID = _StoryID;
        PData.NowStoryID = _StoryID;
        var data = SData.GetStoryData(nowStoryID);
        SoundManager.Instance.PlayBGM(data.BGM);

        obj_TextBox.SetActive(false);

        SetImage();
    }

    public void StartGame()
    {
        obj_TextBox.SetActive(true);

        SetDialogue();
        TypingEffect();

        SetSound();

        isSPD = false;
        isAuto = false;
        SetSpeed();
        SetAuto();

        isChoice = false;
        selectBox.gameObject.SetActive(false);
        obj_TextBox.gameObject.SetActive(true);
    }

    public void ClickNextButton()
    {
        if (useEffect) return;
        if (isChoice) return;

        if (TypingText)
        {
            StopCoroutine(typingCoroutine);
            txt_Dialogue.maxVisibleCharacters = txt_Dialogue.text.Length;
            txt_SecondDialogue.maxVisibleCharacters = txt_Dialogue.text.Length;

            TypingText = false;
            return;
        }

        var data = SData.GetStoryData(PData.NowStoryID);

        if(data.Nextid == 0)
        {
            if (data.Select1 == 0)
            {
                // 끝났다
                foreach (var item in PData.Parameter.ParamDict)
                {
                    UnityEngine.Debug.Log($"{item.Key} : {item.Value}");
                }
                UnityEngine.Debug.Log("대화가 종료되었습니다.");
                return;
            }

            isChoice = true;
            obj_TextBox.gameObject.SetActive(false);
            selectBox.ActiveBox(data);
        }
        else
        {
            PData.NowStoryID = data.Nextid;
            nowStoryID = data.Nextid;

            SetEffect();
            SetSound();
        }
    }

    public void SetEffect()
    {
        var data = SData.GetStoryData(nowStoryID);

        switch ((STORY_EFFECT)data.Effect)
        {
            case STORY_EFFECT.NONE:
                {
                    SetImage();
                    SetDialogue();
                    TypingEffect();
                }
                break;
            case STORY_EFFECT.CAMERA_SHAKE:
                {
                    if (cor_StoryEffect != null) StopCoroutine(cor_StoryEffect);
                    cor_StoryEffect = StartCoroutine(cameraShakeEffectCoroutine());
                }
                break;
            case STORY_EFFECT.FADE:
                {
                    obj_TextBox.gameObject.SetActive(false);
                    UIControl.Instance.FadeEffect(SetImage, () => { obj_TextBox.gameObject.SetActive(true); EndEffect(); }, EnviData.FadeSpeed);
                }
                break;
            default:
                break;
        }
    }

    IEnumerator cameraShakeEffectCoroutine()
    {
        UIControl.Instance.DontTouchMe.SetActive(true);
        useEffect = true;
        float time = 0;

        SetImage();

        while (time < 0.5f)
        {
            float x = Random.Range(-1f, 1f) * 30;
            float y = Random.Range(-1f, 1f) * 5;

            img_BG.rectTransform.anchoredPosition = new Vector2(x, y);

            time += Time.unscaledDeltaTime;
            yield return null;
        }
        img_BG.rectTransform.anchoredPosition = new Vector2(0, 0);
        EndEffect();
        UIControl.Instance.DontTouchMe.SetActive(false);
    }

    void EndEffect()
    {
        useEffect = false;
        SetDialogue();
        TypingEffect();
    }

    public void SetImage()
    {
        //BG
        var storyData = SData.GetStoryData(nowStoryID);
        if(storyData.BG == 0)
        {
            obj_SecondTextBox.SetActive(true);
            txt_SecondDialogue.maxVisibleCharacters = 0;
        }
        else
        {
            obj_SecondTextBox.SetActive(false);
            var BGData = SData.GetPlaceData(storyData.BG);
            img_BG.sprite = ResourceManager.Instance.GetImage(BGData.Resource);
            txt_Dialogue.maxVisibleCharacters = 0;
        }


        //Character
        for (int i = 0; i < img_Character.Length; i++)
        {

            SetCharacterImage(i);
        }
    }

    public void SetDialogue()
    {
        var data = SData.GetStoryData(nowStoryID);
        //Character
        int characterID = data.Character;
        int dialogueID = data.Text;

        if(characterID == 0)
        {
            obj_NameTextBox[0].SetActive(false);
            obj_NameTextBox[1].SetActive(false);
        }
        else
        {
            obj_NameTextBox[0].SetActive(true);
            obj_NameTextBox[1].SetActive(true);
            txt_Name.text = SData.GetLocalizeData(SData.GetCharacterData(characterID).Name);
            txt_Club.text = SData.GetLocalizeData(SData.GetCharacterData(characterID).Club);
        }
        txt_Dialogue.text = SData.GetLocalizeStoryData(dialogueID);
        txt_SecondDialogue.text = txt_Dialogue.text;
    }

    void TypingEffect()
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypingCoroutine());
    }

    IEnumerator TypingCoroutine()
    {
        TypingText = true;
        txt_Dialogue.maxVisibleCharacters = 0;
        txt_SecondDialogue.maxVisibleCharacters = 0;

        int totalTextLength = txt_Dialogue.text.Length;

        for (int i = 0; i <= totalTextLength; i++)
        {
            txt_Dialogue.maxVisibleCharacters = i;
            txt_SecondDialogue.maxVisibleCharacters = i;
            if(isSPD)
                yield return Util.WaitGet(EnviData.TypingSpeed_Fast);
            else
                yield return Util.WaitGet(EnviData.TypingSpeed_Default);
        }

        TypingText = false;
    }

    void SetCharacterImage(int _index)
    {
        StoryData data = SData.GetStoryData(PData.NowStoryID);

        switch (_index)
        {
            case 0:
                if (data.Imagel.Length == 0)
                {
                    img_Character[_index].gameObject.SetActive(false);
                }
                else
                {
                    img_Character[_index].gameObject.SetActive(true);
                    img_Character[_index].sprite = ResourceManager.Instance.GetImage(data.Imagel[0]);
                    img_Character[_index].color = data.Imagel[1] == 0 ? deActiveColor : activeColor;
                    if (data.Imagel[1] > 1)
                        StartCoroutine(CharacterEffectCoroutine(img_Character[_index], data.Imagel[1]));
                }
                break;
            case 1:
                if (data.Imager.Length == 0)
                {
                    img_Character[_index].gameObject.SetActive(false);
                }
                else
                {
                    img_Character[_index].gameObject.SetActive(true);
                    img_Character[_index].sprite = ResourceManager.Instance.GetImage(data.Imager[0]);
                    img_Character[_index].color = data.Imager[1] == 0 ? deActiveColor : activeColor;
                    if (data.Imager[1] > 1)
                        StartCoroutine(CharacterEffectCoroutine(img_Character[_index], data.Imager[1]));
                }
                break;
            case 2:
                if (data.Imagelm.Length == 0)
                {
                    img_Character[_index].gameObject.SetActive(false);
                }
                else
                {
                    img_Character[_index].gameObject.SetActive(true);
                    img_Character[_index].sprite = ResourceManager.Instance.GetImage(data.Imagelm[0]);
                    img_Character[_index].color = data.Imagelm[1] == 0 ? deActiveColor : activeColor;
                    if (data.Imagelm[1] > 1)
                        StartCoroutine(CharacterEffectCoroutine(img_Character[_index], data.Imagelm[1]));
                }
                break;
            case 3:
                if (data.Imagerm.Length == 0)
                {
                    img_Character[_index].gameObject.SetActive(false);
                }
                else
                {
                    img_Character[_index].gameObject.SetActive(true);
                    img_Character[_index].sprite = ResourceManager.Instance.GetImage(data.Imagerm[0]);
                    img_Character[_index].color = data.Imagerm[1] == 0 ? deActiveColor : activeColor;
                    if (data.Imagerm[1] > 1)
                        StartCoroutine(CharacterEffectCoroutine(img_Character[_index], data.Imagerm[1]));
                }
                break;
            case 4:
                if (data.Imagem.Length == 0)
                {
                    img_Character[_index].gameObject.SetActive(false);
                }
                else
                {
                    img_Character[_index].gameObject.SetActive(true);
                    img_Character[_index].sprite = ResourceManager.Instance.GetImage(data.Imagem[0]);
                    img_Character[_index].color = data.Imagem[1] == 0 ? deActiveColor : activeColor;
                    if (data.Imagem[1] > 1)
                        StartCoroutine(CharacterEffectCoroutine(img_Character[_index], data.Imagem[1]));
                }
                break;
            default:
                break;
        }
    }

    void SetSound()
    {
        var data = SData.GetStoryData(nowStoryID);

        SoundManager.Instance.PlayBGM(data.BGM);
        SoundManager.Instance.PlaySFX(data.SFX);
        SoundManager.Instance.PlayVoice(data.Voice);
    }

    // 스토리 옵션
    void OnClickAuto()
    {
        isAuto = !isAuto;
        SetAuto();

        if(isAuto && !TypingText)
        {
            if (cor_Auto != null) StopCoroutine(cor_Auto);
            cor_Auto = StartCoroutine(AutoCoroutine());
        }
        if(!isAuto) if (cor_Auto != null) StopCoroutine(cor_Auto);
    }
    void SetAuto()
    {
        obj_Auto_Active.SetActive(isAuto);
        obj_Auto_DeActive.SetActive(!isAuto);
    }
    IEnumerator AutoCoroutine()
    {
        if(isSPD)
            yield return Util.WaitGet(EnviData.AutoSpeed_Fast);
        else
            yield return Util.WaitGet(EnviData.AutoSpeed_Default);

        ClickNextButton();
    }

    void OnClickSpeed()
    {
        isSPD = !isSPD;
        SetSpeed();

    }
    void SetSpeed()
    {
        obj_SPD_Active.SetActive(isSPD);
    }

    public void SelectChoice(int _nextID)
    {
        isChoice = false;

        selectBox.gameObject.SetActive(false);
        obj_TextBox.gameObject.SetActive(true);

        PData.NowStoryID = _nextID;
        nowStoryID = _nextID;

        SetEffect();
        SetSound();
    }

    // 캐릭터 흔들림 연출
    IEnumerator CharacterEffectCoroutine(Image _img, int _type)
    {
        Vector2 originalPosition = _img.rectTransform.anchoredPosition;
        float elapsedRealTime = 0f;

        while (elapsedRealTime < 0.5f) // 흔들림 시간
        {
            elapsedRealTime += Time.unscaledDeltaTime;

            float offset = Mathf.Sin(Time.unscaledTime * 70f) * 10f; // 속도 * 이동거리

            Vector2 newPosition = originalPosition;

            switch (_type)
            {
                case 2: // 2: 좌우 흔들림
                    newPosition.x += offset;
                    break;
                case 3: // 3: 상하 흔들림
                    newPosition.y += offset;
                    break;
                default:
                    break;
            }
            _img.rectTransform.anchoredPosition = newPosition;

            yield return null;
        }

        _img.rectTransform.anchoredPosition = originalPosition;
    }
}

