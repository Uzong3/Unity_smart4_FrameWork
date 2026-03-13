using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : ControlBase<UIControl>
{
    public Transform PopupPos;

    [Header("Effect")]
    [SerializeField] Image fadeImage;
    [Header("UI")]
    [SerializeField] PopupSetting settingPopup;
    public PopupSetting SettingPopup => settingPopup;
    [SerializeField] PopupMessage messagePopup;
    public PopupMessage MessagePopup => messagePopup;
    [SerializeField] PopupSaveLoad saveLoadPopup;
    public PopupSaveLoad SaveLoadPopup => saveLoadPopup;
    [SerializeField] PopupGallery galleryPopup;
    public PopupGallery GalleryPopup => galleryPopup;
    [Header("Menu")]
    [SerializeField] PopupLog logPopup;
    public PopupLog LogPopup => logPopup;

    [SerializeField] GameObject dontTouchMe;
    public GameObject DontTouchMe => dontTouchMe;

    protected override void Start()
    {
        base.Start();


    }

    public override void Open(PlayerData _pData)
    {
        base.Open(_pData);
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    public void ActiveGallery(bool _isActive)
    {
        if(_isActive)
        {
            galleryPopup.Initialize();
        }
        else
        {
            galleryPopup.gameObject.SetActive(false);
        }
    }

    public void ActiveLog(bool _isActive)
    {
        if(_isActive)
            logPopup.Initialize();
    }

    public void ActiveSetting(bool _isActive)
    {
        if (_isActive)
            settingPopup.Initialize();
        else
            settingPopup.ClosePopup();
    }

    public void ActiveSaveLoad(bool _isActive, bool _isSaveData)
    {
        SoundManager.Instance.PlaySFX((int)ENVI.BUTTON_SFX);
        saveLoadPopup.ActiveMenu(_isActive, _isSaveData);
    }

    public void FadeEffect(bool _isActive)
    {
        fadeImage.gameObject.SetActive(_isActive);
    }

    public void FadeEffect(Action _action, Action _afterAction, float _fadeSPD = 1f)
    {
        if(cor_FadeEffect != null) StopCoroutine(cor_FadeEffect);
        cor_FadeEffect = StartCoroutine(FadeEffectCoroutine(_action, _afterAction, _fadeSPD));
    }

    Coroutine cor_FadeEffect;
    IEnumerator FadeEffectCoroutine(Action _action, Action _afterAction, float _fadeSPD)
    {
        dontTouchMe.SetActive(true);

        fadeImage.gameObject.SetActive(true);
        float time = 0;
        while (time < 1)
        {
            fadeImage.color = new Color(0, 0, 0, time);
            time += Time.fixedDeltaTime * _fadeSPD;
            yield return new WaitForFixedUpdate();
        }
        fadeImage.color = new Color(0, 0, 0, 1);

        if(_action != null) _action.Invoke();
        yield return Util.WaitGet(0.5f);
        time = 1;

        while (0 < time)
        {
            fadeImage.color = new Color(0, 0, 0, time);
            time -= Time.fixedDeltaTime * _fadeSPD;
            yield return new WaitForFixedUpdate();
        }
        fadeImage.color = new Color(0, 0, 0, 0);

        if (_afterAction != null) _afterAction.Invoke();
        fadeImage.gameObject.SetActive(false);

        dontTouchMe.SetActive(false);
    }
}
