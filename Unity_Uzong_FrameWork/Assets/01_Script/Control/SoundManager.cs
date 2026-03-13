using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class SoundManager : ControlBase<SoundManager>
{
    Coroutine cor_ChangeBGM;
    Coroutine cor_StopBGM;

    [Header("플레이어")]
    [SerializeField] Transform bgm;
    AudioSource bgmPlayer;
    [SerializeField] Transform sfx;
    AudioSource[] sfxPlayer;
    [SerializeField] Transform voice;
    AudioSource voicePlayer;

    int nowPlayBGM;

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

        bgmPlayer = bgm.GetComponent<AudioSource>();
        sfxPlayer = sfx.GetComponents<AudioSource>();
        voicePlayer = voice.GetComponent<AudioSource>();

        SetBGMVol(GameManager.PData.Option.BGMVol);
        SetSFXVol(GameManager.PData.Option.SFXVol);
        SetVoiceVol(GameManager.PData.Option.VoiceVol);
    }

    public void PlayBGM(int _soundID, bool _isDirect = false)
    {
        if (bgmPlayer.isPlaying && nowPlayBGM == _soundID) return;

        AudioClip clip = ResourceManager.Instance.GetSound(_soundID);
        if (clip == null)
        {
            return;
        }

        if (_isDirect)
        {
            nowPlayBGM = _soundID;
            bgmPlayer.clip = clip;
            bgmPlayer.Play();
        }
        else
        {
            ChangeBGM(_soundID);
        }
    }

    public void ChangeBGM(int _soundID)
    {
        float vol = PData.Option.BGMVol / 100f;
        nowPlayBGM = _soundID;

        if (cor_StopBGM != null) StopCoroutine(cor_StopBGM);
        if (cor_ChangeBGM != null) StopCoroutine(cor_ChangeBGM);
        cor_ChangeBGM = StartCoroutine(ChangeBGMCoroutine(vol));
    }

    IEnumerator ChangeBGMCoroutine(float _vol)
    {
        float time = _vol;
        while (0 < time)
        {
            time -= Time.fixedDeltaTime;
            bgmPlayer.volume = time;
            yield return new WaitForFixedUpdate();
        }

        bgmPlayer.clip = ResourceManager.Instance.GetSound(nowPlayBGM);
        bgmPlayer.Play();

        time = 0;
        while (time < _vol)
        {
            time += Time.fixedDeltaTime;
            bgmPlayer.volume = time;
            yield return new WaitForFixedUpdate();
        }

        bgmPlayer.volume = _vol;
    }

    public void StopBGM(bool _isDirect)
    {
        if(_isDirect)
        {
            bgmPlayer.Stop();
        }
        else
        {
            if (cor_StopBGM != null) StopCoroutine(cor_StopBGM);
            cor_StopBGM = StartCoroutine(StopBGMCoroutine());
        }
    }

    IEnumerator StopBGMCoroutine()
    {
        float vol = bgmPlayer.volume;
        float time = vol;

        while (0 < time)
        {
            time -= Time.fixedDeltaTime;
            bgmPlayer.volume = time;
            yield return new WaitForFixedUpdate();
        }

        bgmPlayer.Stop();
        bgmPlayer.volume = bgmPlayer.volume;
    }

    public AudioSource PlaySFX(int _soundID, int _startSE = 0)
    {
        AudioClip clip = ResourceManager.Instance.GetSound(_soundID);
        if(clip == null)
        {
            return null;
        }

        for (int x = _startSE; x < sfxPlayer.Length; x++)
        {
            if (!sfxPlayer[x].isPlaying)
            {
                sfxPlayer[x].clip = ResourceManager.Instance.GetSound(_soundID);
                sfxPlayer[x].Play();
                return sfxPlayer[x];
            }
        }
        UnityEngine.Debug.Log("모든 SFX 플레이어가 사용중입니다!!");
        return null;
    }

    public void PlayVoice(int _soundID)
    {
        if (!EnviData.UseVoice) return;

        AudioClip clip = ResourceManager.Instance.GetSound(_soundID);
        if (clip == null)
        {
            return;
        }

        voicePlayer.clip = clip;
        voicePlayer.Play();
    }

    // 볼륨 옵션 조절
    public void SetBGMVol(float _vol)
    {
        bgmPlayer.volume = _vol;
    }

    // 볼륨 옵션 조절
    public void SetSFXVol(float _vol)
    {
        foreach (var sfx in sfxPlayer)
        {
            sfx.volume = _vol;
        }
    }

    // 볼륨 옵션 조절
    public void SetVoiceVol(float _vol)
    {
        voicePlayer.volume = _vol;
    }

    // 진동
    public void SetVibration(bool _isActive)
    {
        PData.Option.Haptic = _isActive;
    }

    public void PlayHaptic()
    {
        if (!EnviData.UseHaptic) return;

    }
}