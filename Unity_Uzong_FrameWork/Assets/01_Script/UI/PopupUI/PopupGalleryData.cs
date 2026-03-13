using System;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class PopupGalleryData : MonoBehaviour
{
    [SerializeField] Image img_Gallery;
    [SerializeField] Button btn_Gallery;
    [SerializeField] GameObject obj_NotOpen;

    int id;
    bool isOpen;

    private void Awake()
    {
        btn_Gallery.onClick.AddListener(() => SoundManager.Instance.PlaySFX((int)ENVI.BUTTON_SFX));
        btn_Gallery.onClick.AddListener(OnClick);
    }

    public void Initialize(int _id, bool _isOpen = true)
    {
        id = _id;
        isOpen = _isOpen;

        obj_NotOpen.SetActive(!isOpen);

        if(isOpen)
        {
            var data = SData.GetGalleryData(_id);
            img_Gallery.sprite = ResourceManager.Instance.GetImage(SData.GetPlaceData(data.Image).Resource);
        }
    }

    void OnClick()
    {
        if (!isOpen) return;

        var data = SData.GetGalleryData(id);
        UIControl.Instance.ActiveGallery(false);
        GameManager.Instance.ChangeScene(SCENE.GALLERY,
            () => GalleryControl.Instance.Initialize(data.Start, data.End),
            () => GalleryControl.Instance.StartGame()
            );
        ;
    }
}
