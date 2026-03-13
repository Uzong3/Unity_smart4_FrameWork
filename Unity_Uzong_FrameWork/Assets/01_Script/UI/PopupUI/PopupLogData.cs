using TMPro;
using UnityEngine;

public class PopupLogData : MonoBehaviour
{
    [SerializeField] GameObject defaultObject;
    [SerializeField] TextMeshProUGUI default_Name;
    [SerializeField] TextMeshProUGUI default_Log;

    [SerializeField] GameObject selectObject;
    [SerializeField] TextMeshProUGUI select_Name;
    [SerializeField] TextMeshProUGUI select_Log;

    public void Initialize(string _name, string _log, bool _isSelect = false)
    {
        defaultObject.gameObject.SetActive(!_isSelect);
        selectObject.gameObject.SetActive(_isSelect);

        if (_isSelect)
        {
            select_Name.text = _name;
            select_Log.text = _log;
        }
        else
        {
            default_Name.text = _name;
            default_Log.text = _log;
        }

        gameObject.SetActive(true);
    }
}
