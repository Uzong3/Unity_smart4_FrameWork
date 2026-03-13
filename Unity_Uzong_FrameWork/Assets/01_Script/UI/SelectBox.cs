using UnityEngine;
using UnityEngine.UI;

public class SelectBox : MonoBehaviour
{
    [SerializeField] SelectButton[] selectButton;

    public void ActiveBox(StoryData _data)
    {
        this.gameObject.SetActive(true);

        if (_data.Select1 > 0)
        {
            selectButton[0].Initialize(SData.GetLocalizeStoryData(_data.Select1), _data.Select1nextid, _data.Select1effect[0], _data.Select1effect[1]);
        }
        else
        {
            selectButton[0].gameObject.SetActive(false);
        }

        if (_data.Select2 > 0)
        {
            selectButton[1].Initialize(SData.GetLocalizeStoryData(_data.Select2), _data.Select2nextid, _data.Select2effect[0], _data.Select2effect[1]);
        }
        else
        {
            selectButton[1].gameObject.SetActive(false);
        }

        if (_data.Select3 > 0)
        {
            selectButton[2].Initialize(SData.GetLocalizeStoryData(_data.Select3), _data.Select3nextid, _data.Select3effect[0], _data.Select3effect[1]);
        }
        else
        {
            selectButton[2].gameObject.SetActive(false);
        }
    }
}
