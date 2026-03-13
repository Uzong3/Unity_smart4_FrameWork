using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleControl : ControlBase<TitleControl>
{
    [SerializeField] TitleMenu titleMenu;
    [SerializeField] Image img_Title;

    protected override void Awake()
    {
        base.Awake();
    }

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
        img_Title.sprite = ResourceManager.Instance.GetImage((int)ENVI.TITLE_BG); // 타이틀 BG
        SoundManager.Instance.ChangeBGM((int)ENVI.TITLE_BGM); // 타이틀 사운드 변경
        RefreshUI();

        base.Initialize();
    }

    public void RefreshUI()
    {
        titleMenu.Initialize();
    }
}
