using System.Data.Common;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectButton : MonoBehaviour
{
    [SerializeField] Button btn_Select;
    [SerializeField] TextMeshProUGUI txt_Select;

    int type;
    int effect;
    int nextID;

    private void Awake()
    {
        btn_Select.onClick.AddListener(() => SoundManager.Instance.PlaySFX((int)ENVI.BUTTON_SFX));

        btn_Select.onClick.AddListener(Effect);
    }

    public void Initialize(string _text, int _nextID, int _type = 0, int _effect = 0)
    {
        txt_Select.text = _text;
        type = _type;
        effect = _effect;
        nextID = _nextID;
        this.gameObject.SetActive(true);
    }

    void Effect()
    {
        GameManager.PData.Parameter.SetParameter(type, effect);
        StoryControl.Instance.SelectChoice(nextID);
    }
}
