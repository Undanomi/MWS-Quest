using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Villager11Controller : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 2.0f;
    public float waitTime = 1.0f;

    [Header("L字型移動設定")]
    public Vector2 startPoint;
    public Vector2 midPoint;
    public Vector2 endPoint;

    private Animator _animator;
    private Rigidbody2D _rb;

    private Vector2 _currentTarget;
    private bool _isWaiting;
    
    private static readonly int DirectionX = Animator.StringToHash("DirectionX");
    private static readonly int DirectionY = Animator.StringToHash("DirectionY");

    private enum MovementStep
    {
        StartToMid,
        MidToEnd,
        EndToMid,
        MidToStart,
    }

    private MovementStep _currentStep;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
        
        _currentTarget = midPoint;
        _currentStep = MovementStep.StartToMid;
        transform.position = startPoint;
    }

    void Update()
    {
        if (!_isWaiting)
        {
            // 村人移動
            Move();
            
            // 目的地に到着したら次の目的地を設定
            if (Vector2.Distance(transform.position, _currentTarget) < 0.1f)
            {
                StartCoroutine(TurnAndWait());
            }
        }
        
    }

    private void Move()
    {
        // 村人を目的地に向かって移動させる
        Vector2 direction = (_currentTarget - (Vector2)transform.position).normalized;
        _rb.velocity = direction * moveSpeed;
        
        // アニメーションの更新
        UpdateAnimation();
    }
    
    private void UpdateAnimation()
    {
        // 移動方向に応じてアニメーションを変更
        Vector2 direction = (_currentTarget - (Vector2)transform.position).normalized;
        _animator.SetFloat(DirectionX, direction.x);
        _animator.SetFloat(DirectionY, direction.y);
    }

    System.Collections.IEnumerator TurnAndWait()
    {
        _isWaiting = true;
        
        // 村人が正面を向いて待機する
        _animator.SetFloat(DirectionX, 0);
        _animator.SetFloat(DirectionY, -1);
        _rb.velocity = Vector2.zero; // 移動を停止
        
        yield return new WaitForSeconds(waitTime);
        
        // 次の目的地を設定
        switch (_currentStep)
        {
            // now Start -> Mid, next Mid -> End
            case MovementStep.StartToMid:
                _currentTarget = endPoint;
                _currentStep = MovementStep.MidToEnd;
                break;
            // now Mid -> End, next End -> Mid
            case MovementStep.MidToEnd:
                _currentTarget = midPoint;
                _currentStep = MovementStep.EndToMid;
                break; 
            // now End -> Mid, next Mid -> Start
            case MovementStep.EndToMid:
                _currentTarget = startPoint;
                _currentStep = MovementStep.MidToStart;
                break;
            // now Mid -> Start, next Start -> Mid
            case MovementStep.MidToStart:
                _currentTarget = midPoint;
                _currentStep = MovementStep.StartToMid;
                break;
        }
        
        _isWaiting = false;
    }
}
