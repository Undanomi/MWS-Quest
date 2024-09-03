using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("プレイヤーの移動速度移動速度")] public float moveSpeed;
    
    private Rigidbody2D _rb;
    
    private float _horizontal;
    private float _vertical;
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxis("Horizontal");
        _vertical = Input.GetAxis("Vertical");
    }
    
    void FixedUpdate()
    {
        Move();
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
}
