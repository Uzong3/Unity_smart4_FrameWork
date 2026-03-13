using UnityEngine;
using UnityEngine.UI;

public class PopupUI : MonoBehaviour
{
    [SerializeField] Button[] btn_Close;
    [SerializeField] Animator anicon;

    protected virtual void Awake()
    {
        transform.position = UIControl.Instance.PopupPos.position;
        if (btn_Close.Length != 0)
        {
            foreach (var _btn in btn_Close)
            {
                _btn.onClick.AddListener(() => SoundManager.Instance.PlaySFX((int)ENVI.BUTTON_SFX));

                _btn.onClick.AddListener(Close);
            }
        }
        this.gameObject.SetActive(false);
    }

    public virtual void Initialize()
    {
        ActivePopUp();
    }

    public virtual void ActivePopUp()
    {
        this.gameObject.SetActive(true);
        if(anicon != null) anicon.SetBool(AnimString.ACTIVE, true);

    }

    protected virtual void Close()
    {
        if(anicon == null)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            anicon.SetBool(AnimString.ACTIVE, false);
        }
    }

    public virtual void DeActivePopup()
    {
        this.gameObject.SetActive(false);
    }
}
