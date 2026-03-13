using UnityEngine;

[CreateAssetMenu]
public class EnviData : ScriptableObject
{
    private static EnviData _instance;

    public static EnviData Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<EnviData>("GameConfig");

#if UNITY_EDITOR
                if (_instance == null)
                {
                    string[] guids = UnityEditor.AssetDatabase.FindAssets("t:EnviData");
                    if (guids.Length > 0)
                    {
                        string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
                        _instance = UnityEditor.AssetDatabase.LoadAssetAtPath<EnviData>(path);
                    }
                }
#endif
            }

            if (_instance == null)
            {
                Debug.LogError("EnviData 에셋 파일을 찾을 수 없습니다! Resources 폴더에 넣거나 에셋을 생성해 주세요.");
            }
            return _instance;
        }
    }

    [Tooltip("페이드 아웃 속도")]
    [SerializeField] float fadeSpeed = 1.5f;
    public static float FadeSpeed => Instance.fadeSpeed;

    [Header("텍스트 속도")]
    [Tooltip("기본 속도")]
    [SerializeField] float typingSpeed_Default = 0.05f;
    public static float TypingSpeed_Default => Instance.typingSpeed_Default;

    [Tooltip("빠른 속도")]
    [SerializeField] float typingSpeed_Fast = 0.01f;
    public static float TypingSpeed_Fast => Instance.typingSpeed_Fast;

    [Header("자동진행 속도")]
    [Tooltip("기본 속도")]
    [SerializeField] float autoSpeed_Default = 1f;
    public static float AutoSpeed_Default => Instance.autoSpeed_Default;

    [Tooltip("빠른 속도")]
    [SerializeField] float autoSpeed_Fast = 0.5f;
    public static float AutoSpeed_Fast => Instance.autoSpeed_Fast;

    [Header("보이스 사용 여부")]
    [SerializeField] bool useVoice = true;
    public static bool UseVoice => Instance.useVoice;

    [Header("진동 사용 여부")]
    [SerializeField] bool useHaptic = true;
    public static bool UseHaptic => Instance.useHaptic;

    [Header("저장 파일 수")]
    [SerializeField] int saveCount = 10;
    public static int SaveCount => Instance.saveCount;

}
