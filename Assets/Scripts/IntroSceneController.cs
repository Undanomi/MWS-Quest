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
    public Vector2 destination;

    [Header("プレイヤーの移動速度")] public float moveSpeed;
    private readonly float _stopDistance = 0.1f;
    
    private PlayerController _playerController;

    private void Start()
    {
        _playerController = player.GetComponent<PlayerController>();

        StartCoroutine(MovePlayerToDestination());
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private IEnumerator MovePlayerToDestination()
    {
        Vector2 direction = (destination - (Vector2)player.transform.position).normalized;
        _playerController.StartAutoMove(direction, moveSpeed);
        
        // プレイヤーが目的地に到達するまでループ
        while (Vector2.Distance(player.transform.position, destination) > _stopDistance)
        {
            yield return null;
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
