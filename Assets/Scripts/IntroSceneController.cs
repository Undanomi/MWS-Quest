using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroSceneController : MonoBehaviour
{
    [Header("プレイヤーオブジェクト")]
    public GameObject player;
    
    [Header("DialogueRunnerオブジェクト")]
    public Yarn.Unity.DialogueRunner dialogueRunner;
    
    [Header("プレイヤーの目的地")]
    public Vector2[] destinations;

    [Header("プレイヤーの移動速度")] public float moveSpeed;
    
    [Header("VirtualCameraオブジェクト")]
    public Cinemachine.CinemachineVirtualCamera mapOverViewCamera;
    public Cinemachine.CinemachineVirtualCamera playerFollowCamera;
    
    [Header("カメラ切り替えまでの時間")]
    public float cameraSwitchDelay;

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
        yield return StartCoroutine(SwitchCamera());
        
        // プレイヤーを目的地に移動
        yield return StartCoroutine(MovePlayerToDestination());
        
    }

    private IEnumerator SwitchCamera()
    {
        // マップを俯瞰
        mapOverViewCamera.Priority = 1;
        playerFollowCamera.Priority = 0;
        
        yield return new WaitForSeconds(cameraSwitchDelay);
        
        // プレイヤーを追従
        mapOverViewCamera.Priority = 0;
        playerFollowCamera.Priority = 1;
        
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator MovePlayerToDestination()
    {
        foreach (Vector2 destination in destinations)
        {
            Vector2 direction = (destination - (Vector2)player.transform.position).normalized;
            _playerController.StartAutoMove(direction, moveSpeed);
        
            // プレイヤーが目的地に到達するまでループ
            while (Vector2.Distance(player.transform.position, destination) > _stopDistance)
            {
                yield return null;
            }
        }
        
        // プレイヤーの自動移動を停止
        _playerController.StopAutoMove();
        
        if (dialogueRunner)
        {
            // YarnSpinnerのDialogueRunnerを開始
            dialogueRunner.StartDialogue("Start");
        }
    }
}
