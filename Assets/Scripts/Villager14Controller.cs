using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class Villager14Controller : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 2.0f;

    [Header("移動座標設定")]
    public Vector2 topLeftPoint;
    public Vector2 topRightPoint;
    public Vector2 bottomRightPoint;
    public Vector2 bottomLeftPoint;

    private Animator _animator;
    private Rigidbody2D _rb;

    private Vector2 _currentTarget;

    private static readonly int DirectionX = Animator.StringToHash("DirectionX");
    private static readonly int DirectionY = Animator.StringToHash("DirectionY");

    private enum MovementStep
    {
        TopLeftToTopRight,
        TopRightToBottomRight,
        BottomRightToBottomLeft,
        BottomLeftToTopLeft,
    }

    private MovementStep _currentStep;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();

        _currentTarget = topRightPoint;
        _currentStep = MovementStep.TopLeftToTopRight;
        transform.position = topLeftPoint;
    }

    void Update()
    {
        // 村人移動
        Move();
        // 目的地に到着したら次の目的地を設定
        if (Vector2.Distance(transform.position, _currentTarget) < 0.1f)
        {
            ChangeTargetPoint();
        }
    }

    private void Move()
    {
        // 村人の移動
        var direction = (_currentTarget - (Vector2)transform.position).normalized;
        _rb.velocity = direction * moveSpeed;

        // アニメーションパラメータの設定
        _animator.SetFloat(DirectionX, direction.x);
        _animator.SetFloat(DirectionY, direction.y);
    }

    private void ChangeTargetPoint()
    {
        // 次の目的地を設定
        switch (_currentStep)
        {
            // now topLeft -> topRight, next topRight -> bottomRight
            case MovementStep.TopLeftToTopRight:
                _currentTarget = bottomRightPoint;
                _currentStep = MovementStep.TopRightToBottomRight;
                break;
            // now topRight -> bottomRight, next bottomRight -> bottomLeft
            case MovementStep.TopRightToBottomRight:
                _currentTarget = bottomLeftPoint;
                _currentStep = MovementStep.BottomRightToBottomLeft;
                break;
            // now bottomRight -> bottomLeft, next bottomLeft -> topLeft
            case MovementStep.BottomRightToBottomLeft:
                _currentTarget = topLeftPoint;
                _currentStep = MovementStep.BottomLeftToTopLeft;
                break;
            // now bottomLeft -> topLeft, next topLeft -> topRight
            case MovementStep.BottomLeftToTopLeft:
                _currentTarget = topRightPoint;
                _currentStep = MovementStep.TopLeftToTopRight;
                break;
        }
    }
}
