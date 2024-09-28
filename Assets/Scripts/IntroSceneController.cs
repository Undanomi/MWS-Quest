using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;

public class IntroSceneController : MonoBehaviour
{
    [Header("プレイヤーオブジェクト")]
    public GameObject player;
    
    [Header("DialogueRunnerオブジェクト")]
    public Yarn.Unity.DialogueRunner dialogueRunner;
    
    [Header("プレイヤーの目的地")]
    public Vector2[] destinations;
    public Vector2 startPosition;

    [Header("自動移動中のプレイヤーの移動速度")] public float autoMoveSpeed;
    
    [Header("VirtualCameraオブジェクト")]
    public Cinemachine.CinemachineVirtualCamera mapOverViewCamera;
    public Cinemachine.CinemachineVirtualCamera playerFollowCamera;
    
    [Header("シナリオタイトル")] public TextMeshProUGUI scenarioTitle;
    [Header("シナリオタイトルの表示時間")] public float scenarioTitleDisplayTime;
    [Header("暗転画像")] public Image fadeImage;
    [Header("タイトルフェード時間")] public float titleFadeTime;
    [Header("画像フェード時間")] public float imageFadeTime;
    [Header("ログ画面")] public LogViewController logViewController;
    [Header("情報画面")] public ClueViewController clueViewController;
    [Header("ダイアログボタン（DialogueRunnerを読ませる）")] public StartDialogueButtonController startDialogueButtonController;
    
    private readonly float _stopDistance = 0.1f;
    private PlayerController _playerController;

    private void Start()
    {
        _playerController = player.GetComponent<PlayerController>();
        logViewController.SetLogViewAvailable(false);
        clueViewController.SetClueViewAvailable(false);
        startDialogueButtonController.SetStartDialogueButtonAvailable(false);
        StartCoroutine(HandleIntroSequence());
    }
    
    private IEnumerator HandleIntroSequence()
    {
        // プレイヤーを移動できないようにする
        _playerController.SetCanMove(false);
        // シナリオタイトルを表示
        yield return StartCoroutine(ShowScenarioTitle());
        
        // 俯瞰
        yield return StartCoroutine(SwitchMapCamera());
        
        // プレイヤーを目的地に移動
        _playerController.SetCanMove(true);
        yield return StartCoroutine(MovePlayerToDestination());
        
    }

    private IEnumerator ShowScenarioTitle()
    {
        float elapsedTime = 0f;
        Color imageColor = fadeImage.color;
        Color textColor = scenarioTitle.color;
        
        
        fadeImage.gameObject.SetActive(true);
        scenarioTitle.gameObject.SetActive(true);
        
        // 最初は暗転したままで2秒待つ
        while (elapsedTime < 2.0f)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        elapsedTime = 0f;
        // タイトルをフェードイン
        while (elapsedTime < titleFadeTime)
        {
            elapsedTime += Time.deltaTime;
            textColor.a = Mathf.Lerp(0, 1, elapsedTime / titleFadeTime);
            scenarioTitle.color = textColor;
            yield return null;
        }
        
        // タイトルを表示
        elapsedTime = 0f;
        while (elapsedTime < scenarioTitleDisplayTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        
        //タイトルをフェードアウト
        elapsedTime = 0f;
        while (elapsedTime < titleFadeTime)
        {
            elapsedTime += Time.deltaTime;
            textColor.a = Mathf.Lerp(1, 0, elapsedTime / titleFadeTime);
            scenarioTitle.color = textColor;
            yield return null;
        }
        scenarioTitle.gameObject.SetActive(false);
        
        // 画面をフェードアウト
        elapsedTime = 0f;
        while (elapsedTime < imageFadeTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / imageFadeTime;
            t = Mathf.SmoothStep(0, 1, t);
            imageColor.a = Mathf.Lerp(1, 0, t);
            fadeImage.color = imageColor;
            yield return null;
        }
        fadeImage.gameObject.SetActive(false);

        yield return new WaitForSeconds(1.0f);
    }
    
    
    /// <summary>
    /// 村全体を俯瞰した後、カメラをプレイヤーに追従させる
    /// </summary>
    /// <returns></returns>
    private IEnumerator SwitchMapCamera()
    {
        // マップを俯瞰
        mapOverViewCamera.Priority = 1;
        playerFollowCamera.Priority = 0;
        
        // ダイアログでStartを開始し、終了するまで待つ
        dialogueRunner.StartDialogue("Start");
        while (dialogueRunner.IsDialogueRunning)
        {
            yield return null;
        }
        
        // プレイヤーを追従
        mapOverViewCamera.Priority = 0;
        playerFollowCamera.Priority = 1;
        
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator MovePlayerToDestination()
    {
        _playerController.StartAutoMove();
        _playerController.SetAutoMoveSpeed(autoMoveSpeed);
        foreach (Vector2 destination in destinations)
        {
            Vector2 direction = (destination - (Vector2)player.transform.position).normalized;
            _playerController.SetAutoMoveDirection(direction);
        
            // プレイヤーが目的地に到達するまでループ
            while (Vector2.Distance(player.transform.position, destination) > _stopDistance)
            {
                yield return null;
            }
        }
        // 村長に話しかける
        dialogueRunner.StartDialogue("VillageChiefGreeting");
        
        // ゲームスタート地点に移動する
        Vector2 directionToStartPos = (startPosition - (Vector2)player.transform.position).normalized; 
        _playerController.SetAutoMoveDirection(directionToStartPos);
        while (Vector2.Distance(player.transform.position, startPosition) > _stopDistance)
        {
            yield return null;
        }
        
        // プレイヤーの移動を停止
        _playerController.StopAutoMove();
        
        // ナレーターのセリフ
        dialogueRunner.StartDialogue("NarratorGreeting");
    }
}
