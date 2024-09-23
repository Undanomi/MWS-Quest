using DefaultNamespace;
using UnityEngine;
using Yarn.Unity;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("プレイヤーの移動速度")] public float moveSpeed;
    private float _prevMoveSpeed;
    
    private Rigidbody2D _rb;
    
    private float _horizontal;
    private float _vertical;

    private Animator _anim;
    private static readonly int LookX = Animator.StringToHash("Look X");
    private static readonly int LookY = Animator.StringToHash("Look Y");
    private static readonly int IsMoving = Animator.StringToHash("IsMoving");
    public DialogueRunner dialogueRunner;
    
    private bool _isAutoMoving;
    private Vector2 _autoMoveDestination;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_isAutoMoving)
        {
            _horizontal = _autoMoveDestination.x;
            _vertical = _autoMoveDestination.y;
        }
        else
        {
            _horizontal = Input.GetAxis("Horizontal");
            _vertical = Input.GetAxis("Vertical");
        }
        // YarnSpinnerのDialogueRunnerが動いているときはプレイヤーの移動を受け付けない
        if (dialogueRunner.IsDialogueRunning || dialogueRunner.GetComponent<LogView>().isLogViewEnable)
        {
            Debug.Log("Dialogue is running. Player cannot move.");
            return;
        }
        // プレイヤーの向きを変更
        SyncMoveAnimation();
    }
    
    void FixedUpdate()
    {
        // YarnSpinnerのDialogueRunnerが動いているときはプレイヤーの移動を受け付けない
        if (dialogueRunner.IsDialogueRunning || dialogueRunner.GetComponent<LogView>().isLogViewEnable)
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
        Vector2 moveDirection = new Vector2(_horizontal, _vertical);
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
        Vector2 moveDirection = new Vector2(_horizontal, _vertical).normalized;
        _rb.velocity = moveDirection * moveSpeed;
    }
    
    public void StartAutoMove(Vector2 destination, float autoMoveSpeed)
    {
        Debug.Log("moveSpeed: " + moveSpeed);
        _prevMoveSpeed = moveSpeed;
        Debug.Log("autoMoveSpeed: " + autoMoveSpeed);
        Debug.Log("_prevMoveSpeed: " + _prevMoveSpeed);
        moveSpeed = autoMoveSpeed;
        _isAutoMoving = true;
        _autoMoveDestination = destination;
    }
    
    public void StopAutoMove()
    {
        moveSpeed = _prevMoveSpeed;
        Debug.Log("moveSpeed: " + moveSpeed);
        _isAutoMoving = false;
        _anim.SetFloat(LookY, -1);
    }
    
}
