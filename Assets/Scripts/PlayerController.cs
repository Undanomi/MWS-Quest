using DefaultNamespace;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("プレイヤーの移動速度")] public float moveSpeed;
    
    private Rigidbody2D _rb;
    
    private float _horizontal;
    private float _vertical;

    private Animator _anim;
    private static readonly int LookX = Animator.StringToHash("Look X");
    private static readonly int LookY = Animator.StringToHash("Look Y");
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    private DialogueRunner _dialogueRunner;
    
    private bool _isAutoMoving;
    private Vector2 _autoMoveDestination;
    private float _autoMoveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _dialogueRunner = FindObjectOfType<DialogueRunner>();
    }

    // Update is called once per frame
    void Update()
    {
        // YarnSpinnerのDialogueRunnerが動いているときはプレイヤーの移動を受け付けない
        if (_dialogueRunner.IsDialogueRunning || _dialogueRunner.GetComponent<LogView>().isLogViewEnable)
        {
            Debug.Log("Dialogue is running. Player cannot move.");
            return;
        }
        
        // プレイヤーの入力を受け付ける
        _horizontal = Input.GetAxis("Horizontal"); 
        _vertical = Input.GetAxis("Vertical");
        
        // プレイヤーの向きを変更
        SyncMoveAnimation();
    }
    
    void FixedUpdate()
    {
        // YarnSpinnerのDialogueRunnerが動いているときはプレイヤーの移動を受け付けない
        if (_dialogueRunner.IsDialogueRunning || _dialogueRunner.GetComponent<LogView>().isLogViewEnable)
        {
            // 現在のフレームでの移動をキャンセル
            _rb.velocity = Vector2.zero;
            // AnimationをStoppingに変更
            _anim.SetBool(IsMoving, false);
            return;
        }
        Move();
    }

    /// <summary>
    /// プレイヤーの向きと移動アニメーションの同期を行う
    /// </summary>
    private void SyncMoveAnimation()
    {
        Vector2 moveDirection = _isAutoMoving ? _autoMoveDestination : new Vector2(_horizontal, _vertical);
        if (moveDirection.sqrMagnitude > 0)
        {
            _anim.SetFloat(LookX, moveDirection.x);
            _anim.SetFloat(LookY, moveDirection.y);
            _anim.SetBool(IsMoving, true);
        }
        else
        {
            _anim.SetBool(IsMoving, false);
        }
        
    }
    
    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        // direction unit-vector
        Vector2 moveDirection = _isAutoMoving ? _autoMoveDestination : new Vector2(_horizontal, _vertical);
        _rb.velocity = moveDirection * (_isAutoMoving ? _autoMoveSpeed : moveSpeed);
    }
   
    public void SetAutoMoveSpeed(float speed)
    {
        _autoMoveSpeed = speed;
    }
    
    public void StartAutoMove(Vector2 destination)
    {
        _isAutoMoving = true;
        _autoMoveDestination = destination;
    }
    
    public void StopAutoMove()
    {
        Debug.Log("moveSpeed: " + moveSpeed);
        _isAutoMoving = false;
    }
    
}
