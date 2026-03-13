using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupGallery : PopupUI
{
    [SerializeField] PopupGalleryData galleryData;
    [SerializeField] TextMeshProUGUI txt_title;
    List<PopupGalleryData> galleryDatas = new List<PopupGalleryData>();
    [SerializeField] ScrollRect scrollRect;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void ActivePopUp()
    {
        base.ActivePopUp();

        StartCoroutine(ResetScroll());
    }

    public override void Initialize()
    {
        txt_title.text = SData.GetLocalizeEnviData((int)LOCALIZE.MAIN_GALLERY);

        var data = SData.GalleryData;

        for (int i = 0; i < galleryDatas.Count; i++)
            galleryDatas[i].gameObject.SetActive(false);

        int count = 0;
        foreach (var item in data)
        {
            bool isOpen = false;
            if (GameManager.PData.Gallery.TryGetValue(item.Key, out bool value))
                isOpen = value;

            if (galleryDatas.Count <  count + 1)
            {
                var newData = GameObject.Instantiate(galleryData, galleryData.transform.parent);
                galleryDatas.Add(newData);


                newData.Initialize(item.Key, isOpen);
                newData.gameObject.SetActive(true);
            }
            else
            {
                galleryDatas[count].Initialize(item.Key, isOpen);
                galleryDatas[count].gameObject.SetActive(true);
            }
            count++;
        }

        base.Initialize();
    }

    IEnumerator ResetScroll()
    {
        yield return new WaitForEndOfFrame();
        Canvas.ForceUpdateCanvases();
        scrollRect.verticalNormalizedPosition = 1f;
    }

    protected override void Close()
    {
        base.Close();
    }

}
