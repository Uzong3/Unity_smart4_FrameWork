using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : ControlBase<ResourceManager>
{
    [Header("이미지")]
    [SerializeField] Dictionary<int, Sprite> image = new Dictionary<int, Sprite>();

    [Header("사운드")]
    [SerializeField] Dictionary<int, AudioClip> sound = new Dictionary<int, AudioClip>();

    protected override void Awake()
    {
        base.Awake();

    }

    public override void Open(PlayerData _pData)
    {
        base.Open(_pData);
    }

    public override void Initialize()
    {
        base.Initialize();
        SetResource();
    }

    void SetResource()
    {
        var data = SData.ResourceData;

        foreach (var (id, value) in data)
        {
            switch (value.Type)
            {
                case 1:
                    {
                        image.Add(value.ID, Resources.Load<Sprite>(value.Path));
                    }
                    break;
                case 2:
                    {
                        sound.Add(value.ID, Resources.Load<AudioClip>(value.Path));
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public Sprite GetImage(int _id)
    {
        if (image.TryGetValue(_id, out var data)) return data;
        return null;
    }

    public AudioClip GetSound(int _id)
    {
        if (sound.TryGetValue(_id, out var data)) return data;
        return null;
    }
}
