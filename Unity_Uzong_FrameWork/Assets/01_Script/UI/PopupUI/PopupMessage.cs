using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupMessage : PopupUI
{
    [SerializeField] TextMeshProUGUI txt_Title;
    [SerializeField] TextMeshProUGUI txt_Info;
    [SerializeField] Button btn_OK;
    [SerializeField] TextMeshProUGUI txt_OK;
    [SerializeField] TextMeshProUGUI txt_Cancel;

    Action action;

    protected override void Awake()
    {
        base.Awake();

        btn_OK.onClick.AddListener(() => SoundManager.Instance.PlaySFX((int)ENVI.BUTTON_SFX));

        btn_OK.onClick.AddListener(OnClickOK);
    }

    public void Initialize(Action _action, string _title, string _info, string _OK)
    {
        base.Initialize();

        txt_Title.text = _title;
        txt_Info.text = _info;
        txt_OK.text = _OK;
        txt_Cancel.text = SData.GetLocalizeEnviData((int)LOCALIZE.CANCEL);

        action = _action;
    }

    public override void ActivePopUp()
    {
        base.ActivePopUp();
    }

    void OnClickOK()
    {
        action.Invoke();
        Close();
    }

    protected override void Close()
    {
        base.Close();
    }
}
