using UnityEngine;
using EventSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private bool hasSoul = true;

    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    private Direction currentDir; // 当前附身方向

    public bool HasSoul
    {
        get => hasSoul;
        set => hasSoul = value;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Move()
    {
        Vector2 movement = Vector2.zero;

        if (Input.GetKey(KeyCode.D))
        {
            movement += Vector2.right;
            currentDir = Direction.Right; // 更新当前附身方向
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            movement += Vector2.left;
            currentDir = Direction.Left; // 更新当前附身方向
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            movement += Vector2.up;
            currentDir = Direction.Up; // 更新当前附身方向
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            movement += Vector2.down;
            currentDir = Direction.Down; // 更新当前附身方向
        }
        
        rb.linearVelocity = movement * moveSpeed;
    }

    private void Possess()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (hasSoul)
            {
                hasSoul = false; // 玩家失去灵魂
                // TODO: 根据所选方向探测最近物体，并附身对应物体种类
            }
        }
    }
    
    private void Update()
    {
        Move();
        Possess();
    }
}
