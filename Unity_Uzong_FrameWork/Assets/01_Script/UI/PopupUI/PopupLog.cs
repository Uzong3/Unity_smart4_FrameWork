using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupLog : PopupUI
{
    [SerializeField] TextMeshProUGUI txt_title;

    [SerializeField] PopupLogData logData;
    List<PopupLogData> logDatas = new List<PopupLogData>();

    [SerializeField] ScrollRect scrollRect;


    protected override void Awake()
    {
        base.Awake();
    }

    public override void Initialize()
    {
        foreach (var data in logDatas)
        {
            data.gameObject.SetActive(false);
        }

        txt_title.text = SData.GetLocalizeEnviData((int)LOCALIZE.LOG);

        var history = GameManager.PData.StoryHistory;
        int count = 0;
        for (int i = history.Count - 1; i >= 0; i--)
        {
            count++;
            var storyData = SData.GetStoryData(history[i]);

            if (storyData.Select1 > 0)
            {
                if(count > 1)
                {
                    if(history[i + 1] == storyData.Select1nextid)
                    {
                        SetLog(count, SData.GetLocalizeEnviData((int)LOCALIZE.LOG_SELECT), SData.GetLocalizeStoryData(storyData.Select1), true);
                        count++;
                    }
                    else if(history[i + 1] == storyData.Select2nextid)
                    {
                        SetLog(count, SData.GetLocalizeEnviData((int)LOCALIZE.LOG_SELECT), SData.GetLocalizeStoryData(storyData.Select2), true);
                        count++;

                    }
                    else if(history[i + 1] == storyData.Select3nextid)
                    {
                        SetLog(count, SData.GetLocalizeEnviData((int)LOCALIZE.LOG_SELECT), SData.GetLocalizeStoryData(storyData.Select3), true);
                        count++;
                    }
                }

                SetLog(count, SData.GetLocalizeData(storyData.Character), SData.GetLocalizeStoryData(storyData.Text), false);
            }
            else
            {
                SetLog(count, SData.GetLocalizeData(storyData.Character), SData.GetLocalizeStoryData(storyData.Text), false);
            }


        }

        base.Initialize();
    }

    public override void ActivePopUp()
    {
        base.ActivePopUp();

        StartCoroutine(MoveToBottom());
    }

    IEnumerator MoveToBottom()
    {
        yield return new WaitForEndOfFrame();
        scrollRect.verticalNormalizedPosition = 0f;

        Time.timeScale = 0;
    }

    protected override void Close()
    {
        Time.timeScale = 1;

        base.Close();
    }

    void SetLog(int _index ,string _name, string _log, bool _type)
    {
        if(logDatas.Count <= _index)
        {
            CreateLogData().Initialize(_name, _log, _type);
        }
        else
        {
            logDatas[_index].Initialize(_name, _log, _type);
        }
    }

    PopupLogData CreateLogData()
    {
        var newData = GameObject.Instantiate(logData, logData.transform.parent);
        logDatas.Add(newData);
        return newData;
    }
}
