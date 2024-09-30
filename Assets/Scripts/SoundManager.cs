using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yarn.Unity;

public class SoundManager : MonoBehaviour
{
    [Header("ダイアログの有無")]
    public bool hasDialogue;
    [Header("SE Name: ダイアログ送り")]
    [Tooltip("拡張子を除いたものを指定")]
    public string dialogueForwardSound;
    [Header("SE Name: 決定音")]
    [Tooltip("拡張子を除いたものを指定")]
    public string decisionSound;
    [Header("SE Name: キャンセル音")]
    [Tooltip("拡張子を除いたものを指定")]
    public string cancelSound;
    [Header("SE Name: 歩行音")]
    [Tooltip("拡張子を除いたものを指定")]
    public string walkSound;
    [Header("BGM Name:メインテーマ")]
    [Tooltip("拡張子を除いたものを指定")]
    public string bgm;
    
    private AudioSource _soundEffectSource;
    private AudioSource _bgmSource;
    private DialogueRunner _dialogueRunner;
    
    // SEのキャッシュ
    private readonly Dictionary<string, AudioClip> _seCache = new Dictionary<string, AudioClip>();
    // BGMのキャッシュ
    private readonly Dictionary<string, AudioClip> _bgmCache = new Dictionary<string, AudioClip>();

    private void Awake()
    {
        AudioSource[] audioSources = GetComponents<AudioSource>();
        if (audioSources.Length != 2)
        {
            Debug.LogError("AudioSourceの数が2つではありません");
            return;
        }
        _soundEffectSource = audioSources[0];
        _bgmSource = audioSources[1];
    }
    
    private void Start()
    {
        if (hasDialogue)
        {
            _dialogueRunner = FindObjectOfType<DialogueRunner>();
            _dialogueRunner.onDialogueStart.AddListener(() => { PlaySE(decisionSound); });
        }
    }

    public void PlaySE(string seName)
    {
        if (!_seCache.ContainsKey(seName))
        {
            AudioClip clip = Resources.Load<AudioClip>($"Sounds/SE/{seName}");
            if (!clip) // clipがnullの場合
            {
                Debug.LogError($"SE not found: {seName}");
                return;
            }
            _seCache.Add(seName, clip);
        }
        // SE再生
        _soundEffectSource.PlayOneShot(_seCache[seName]);
    }
    
    public void PlayBGM(string bgmName, float fadeInTime=3.0f, bool resume=false)
    {
        // 一時停止解除
        // すでに再生中のBGMと同じ場合は一時停止を解除して音量をフェードイン
        if (resume && _bgmSource.clip && _bgmSource.clip.name == bgmName)
        {
            _bgmSource.UnPause();
            StartCoroutine(FadeInBGM(fadeInTime));
            return;
        }
        
        if (!_bgmCache.ContainsKey(bgmName))
        {
            AudioClip clip = Resources.Load<AudioClip>($"Sounds/BGM/{bgmName}");
            if (!clip)
            {
                Debug.LogError($"BGM not found: {bgmName}");
                return;
            }
            _bgmCache.Add(bgmName, clip);
        }
        
        // BGM再生
        _bgmSource.clip = _bgmCache[bgmName];
        _bgmSource.loop = true;
        _bgmSource.Play();
        StartCoroutine(FadeInBGM(fadeInTime));
    }
    
    private IEnumerator FadeInBGM(float fadeInTime = 0.0f)
    {
        float elapsedTime = 0f;
        float volume = _bgmSource.volume;
        while (elapsedTime < fadeInTime)
        {
            elapsedTime += Time.deltaTime;
            _bgmSource.volume = Mathf.Lerp(0, volume, elapsedTime / fadeInTime);
            yield return null;
        }
    }
    
    public void StopBGM(float fadeOutTime = 0.0f, bool pause=false)
    {
        StartCoroutine(FadeOutBGM(fadeOutTime, pause));
    }
    
    private IEnumerator FadeOutBGM(float fadeOutTime, bool pause)
    {
        float elapsedTime = 0f;
        float volume = _bgmSource.volume;
        while (elapsedTime < fadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            _bgmSource.volume = Mathf.Lerp(volume, 0, elapsedTime / fadeOutTime);
            yield return null;
        }
        if (pause)
        {
            _bgmSource.Pause();
        }
        else
        {
            _bgmSource.Stop();
        }
    }
    
    /// <summary>
    /// BGMのボリュームを設定
    /// </summary>
    /// <param name="volume"></param>
    public void SetBGMVolume(float volume)
    {
        _bgmSource.volume = volume;
    }
    
    /// <summary>
    /// SEのボリュームを設定
    /// </summary>
    /// <param name="volume"></param>
    public void SetSEVolume(float volume)
    {
        _soundEffectSource.volume = volume;
    }
    
    
}
