using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Yarn.Unity;

public class EndingManager : MonoBehaviour
{
    
    [Header("暗転画像")]
    public Image fadeImage;
    [Header("暗転フェード時間")] public float fadeTime;
    [Header("解説画像")] public Image descriptionImage;
    
    [Header("マップ俯瞰カメラ")]
    public Cinemachine.CinemachineVirtualCamera mapOverViewCamera;
    [Header("プレイヤーフォローカメラ")]
    public Cinemachine.CinemachineVirtualCamera playerFollowCamera;
    
    public bool skipToEnding;
   
    private DialogueRunner _dialogueRunner;
    private MissionManager _missionManager;
    private Sprite[] _descriptionImages;
    private bool _isEndingStarted;
    
    private InMemoryVariableStorage _variableStorage;
    
    private void Start()
    {
        _dialogueRunner = FindObjectOfType<DialogueRunner>();
        _missionManager = FindObjectOfType<MissionManager>();
        _descriptionImages = Resources.LoadAll<Sprite>("Images");
        descriptionImage.sprite = _descriptionImages[0];
        descriptionImage.gameObject.SetActive(false);
        _variableStorage = _dialogueRunner.VariableStorage as InMemoryVariableStorage;
    }

    private void Update()
    {
        
        _variableStorage.SetValue("$CorrectFinalQuestion", skipToEnding);
        // Yarnの$CorrectFinalQuestion がtrueになったらエンディング処理を開始
        if (_missionManager.isEndingStarted && !_isEndingStarted)
        {
            _isEndingStarted = true;
            StartCoroutine(HandleEndingSequence());
        }
        UpdateDescriptionImage();
    }
    
    private IEnumerator HandleEndingSequence()
    {
        Debug.Log("Ending started");
        // 暗転
        yield return StartCoroutine(FadeImage(fadeImage, 0f, 1f, fadeTime));
        // カメラを俯瞰に切り替えておく
        Debug.Log("Switching to map camera");
        SwitchToMapCamera();
        float elapsedTime = 0f;
        while (elapsedTime < 2f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // フェードイン
        yield return StartCoroutine(FadeImage(fadeImage, 1f, 0f, fadeTime));
        // エンディングダイアログを開始
        _dialogueRunner.StartDialogue("Ending");
        // エンディングダイアログが終わるまで待つ
        while (_missionManager.HasVisitedNode("Ending") == false)
        {
            Debug.Log("Ending dialogue is running");
            yield return null;
        }
        // 解説ダイアログを開始
        Debug.Log("Starting description");
        _dialogueRunner.StartDialogue("Description");
        // 解説画面のフェードイン
        yield return StartCoroutine(FadeImage(descriptionImage, 0f, 1f, fadeTime));
        descriptionImage.gameObject.SetActive(true);
        // 解説画面のコントロール
        //while (_missionManager.HasVisitedNode("Description") == false)
        //{
        //    UpdateDescriptionImage();
        //}
        // 解説画面のフェードアウト
        yield return StartCoroutine(FadeImage(descriptionImage, 1f, 0f, fadeTime));
        descriptionImage.gameObject.SetActive(false);
        
    }

    private void UpdateDescriptionImage()
    {
        string descriptionImageIndex = ((int)_missionManager.GetYarnVariable<float>("$DescriptionProgress")+1).ToString("D2");
        string descriptionImageName = $"Description{descriptionImageIndex}";
        Debug.Log($"Description image name: {descriptionImageName}");
        Sprite descriptionSprite = Array.Find(_descriptionImages, sprite => sprite.name == descriptionImageName);
        descriptionImage.sprite = descriptionSprite;
    }

    private IEnumerator FadeImage(Image image, float startAlpha, float endAlpha, float fadeDuration)
    {
        float elapsedTime = 0f;
        Color imageColor = image.color;
        
        imageColor.a = startAlpha;
        image.color = imageColor;
        image.gameObject.SetActive(true);
        
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            imageColor.a = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            fadeImage.color = imageColor;
            yield return null;
        }
        
        imageColor.a = endAlpha;
        image.color = imageColor;
    }

    private void SwitchToMapCamera()
    {
        // 俯瞰カメラに切り替える
        mapOverViewCamera.Priority = 1;
        playerFollowCamera.Priority = 0;
    }
}
