using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupSaveLoad : PopupUI
{
    [SerializeField] TextMeshProUGUI txt_Title;
    [SerializeField] ScrollRect scrollRect;
    [SerializeField] PopupOptionSaveData saveData;
    List<PopupOptionSaveData> saveDatas = new List<PopupOptionSaveData>();

    protected override void Awake()
    {
        base.Awake();
    }

    public override void ActivePopUp()
    {
        base.ActivePopUp();
    }

    public void ActiveMenu(bool _isActive, bool _isSaveData)
    {
        txt_Title.text = _isSaveData ? SData.GetLocalizeEnviData((int)LOCALIZE.SAVE) : SData.GetLocalizeEnviData((int)LOCALIZE.LOAD);

        if (_isActive)
        {
            var data = GameManager.PData.SaveDatas;

            for (int i = saveDatas.Count; i < data.Length; i++)
            {
                var newData = GameObject.Instantiate(saveData, saveData.transform.parent);
                newData.Initialize(i);
                saveDatas.Add(newData);
            }

            foreach (var saveData in saveDatas)
            {
                if (saveData.SaveID < data.Length)
                {
                    saveData.RefreshUI(_isSaveData);
                    saveData.gameObject.SetActive(true);
                }
                else
                {
                    saveData.gameObject.SetActive(false);
                }
            }

            scrollRect.verticalNormalizedPosition = 1f;
        }

        this.gameObject.SetActive(_isActive);
        Time.timeScale = 0;
    }

    public PopupOptionSaveData LoadRecentData()
    {
        var data = GameManager.PData.SaveDatas;

        int index = -1;
        DateTime dateTime = DateTime.MinValue;
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i].isActive)
            {
                if(dateTime < data[i].SaveDate)
                {
                    dateTime = data[i].SaveDate;
                    index = i;
                }
            }
        }

        if(index > 0)
            return saveDatas[index];
        
        return null;
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
