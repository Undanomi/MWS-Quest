using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
    
    private readonly float _stopDistance = 0.1f;
    private PlayerController _playerController;

    private void Start()
    {
        _playerController = player.GetComponent<PlayerController>();
        StartCoroutine(HandleIntroSequence());
    }
    
    private IEnumerator HandleIntroSequence()
    {
        // 俯瞰
        yield return StartCoroutine(SwitchMapCamera());
        
        // プレイヤーを目的地に移動
        yield return StartCoroutine(MovePlayerToDestination());
        
    }

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
