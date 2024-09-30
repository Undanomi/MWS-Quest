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
    private LogViewController _logViewController;
    private ClueViewController _clueViewController;
    private StartDialogueButtonController _startDialogueButtonController;
    private SoundManager _soundManager;
    
    private bool _isAutoMoving;
    private Vector2 _autoMoveDirection;
    private float _autoMoveSpeed;
    private bool _canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        _soundManager = FindObjectOfType<SoundManager>();
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _dialogueRunner = FindObjectOfType<DialogueRunner>();
        _logViewController = FindObjectOfType<LogViewController>();
        _clueViewController = FindObjectOfType<ClueViewController>();
        _startDialogueButtonController = FindObjectOfType<StartDialogueButtonController>();
    }

    // Update is called once per frame
    void Update()
    {
        // YarnSpinnerのDialogueRunnerが動いているときはプレイヤーの移動を受け付けない
        if (_dialogueRunner.IsDialogueRunning || 
            _logViewController.isLogViewRunning ||
            _clueViewController.isClueViewRunning
            )
        {
            StopAnimation();
            return;
        }
        
        // プレイヤーの入力を受け付ける
        _horizontal = Input.GetAxis("Horizontal"); 
        _vertical = Input.GetAxis("Vertical");
        
        // プレイヤーの向きを変更
        SyncAnimationAndSE();
    }
    
    void FixedUpdate()
    {
        // YarnSpinnerのDialogueRunnerが動いているときはプレイヤーの移動を受け付けない
        if (_dialogueRunner.IsDialogueRunning || 
            _logViewController.isLogViewRunning ||
            _clueViewController.isClueViewRunning
            )
        {
            // 現在のフレームでの移動をキャンセル
            _rb.velocity = Vector2.zero;
            return;
        }
        Move();
    }
    public void SetCanMove(bool flag)
    {
        _canMove = flag;
    }
    /// <summary>
    /// 動くアニメーションを開始
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    private void StartAnimation(float x, float y)
    {
        _anim.SetFloat(LookX, x);
        _anim.SetFloat(LookY, y);
        _anim.SetBool(IsMoving, true);
        _soundManager.PlayFootStep();
    }
    
    /// <summary>
    /// 動くアニメーションを停止
    /// </summary>
    private void StopAnimation()
    {
        _anim.SetBool(IsMoving, false);
        // フェードアウトが伴うため、コルーチンを利用
        StartCoroutine(_soundManager.StopFootStep());
    }

    /// <summary>
    /// プレイヤーの向きと移動アニメーションの同期を行う
    /// </summary>
    private void SyncAnimationAndSE()
    {
        if (!_canMove) return;
        
        Vector2 moveDirection = _isAutoMoving ? _autoMoveDirection : new Vector2(_horizontal, _vertical);
        if (moveDirection.sqrMagnitude > 0)
        {
            StartAnimation(moveDirection.x, moveDirection.y);
        }
        else
        {
            StopAnimation();
        }
        
    }
    
    /// <summary>
    /// 移動
    /// </summary>
    private void Move()
    {
        if (!_canMove) return;
        
        // direction unit-vector
        Vector2 moveDirection = _isAutoMoving ? _autoMoveDirection : new Vector2(_horizontal, _vertical);
        _rb.velocity = moveDirection * (_isAutoMoving ? _autoMoveSpeed : moveSpeed);
    }
  
    /// <summary>
    /// プレイヤーの自動移動速度を設定
    /// </summary>
    /// <param name="speed"></param>
    public void SetAutoMoveSpeed(float speed)
    {
        _autoMoveSpeed = speed;
    }
    
    /// <summary>
    /// プレイヤーの自動移動目的地を設定
    /// </summary>
    /// <param name="direction"></param>
    public void SetAutoMoveDirection(Vector2 direction)
    {
        _autoMoveDirection = direction;
    }
   
    /// <summary>
    /// プレイヤーの自動移動を開始
    /// </summary>
    public void StartAutoMove()
    {
        _isAutoMoving = true;
        _logViewController.SetLogViewAvailable(false);
        _clueViewController.SetClueViewAvailable(false);
        _startDialogueButtonController.SetStartDialogueButtonAvailable(false);
    }
    
    /// <summary>
    /// プレイヤーの自動移動を停止
    /// </summary>
    public void StopAutoMove()
    {
        _isAutoMoving = false;
        _logViewController.SetLogViewAvailable(true);
        _clueViewController.SetClueViewAvailable(true);
        _startDialogueButtonController.SetStartDialogueButtonAvailable(true);
    }
    
}
