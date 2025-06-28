using System;
using EventSystem;
using Objects.Interactables;
using UnityEngine;
using EventType = EventSystem.EventType;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float soulDetectionRadius = 0.5f;
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
        rb.gravityScale = 0f;
    }

    private void Move()
    {
        Vector2 movement = Vector2.zero;

        if (Input.GetKey(KeyCode.D))
        {
            movement += Vector2.right;
            currentDir = Direction.Right; // 更新当前附身方向
        }
        if (Input.GetKey(KeyCode.A))
        {
            movement += Vector2.left;
            currentDir = Direction.Left; // 更新当前附身方向
        }
        if (Input.GetKey(KeyCode.W))
        {
            movement += Vector2.up;
            currentDir = Direction.Up; // 更新当前附身方向
        }
        if (Input.GetKey(KeyCode.S))
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
                Vector2Int currIdx = WorldManager.Instance.GetIndex(gameObject);

                GameObject neighbor = null;
                switch (currentDir)
                {
                    case Direction.Up:
                        neighbor = WorldManager.Instance.GetObject(currIdx + new Vector2Int(0, 1));
                        break;
                    
                    case Direction.Down:
                        neighbor = WorldManager.Instance.GetObject(currIdx + new Vector2Int(0, -1));
                        break;
                    
                    case Direction.Left:
                        neighbor = WorldManager.Instance.GetObject(currIdx + new Vector2Int(-1, 0));
                        break;
                    
                    case Direction.Right:
                        neighbor = WorldManager.Instance.GetObject(currIdx + new Vector2Int(1, 0));
                        break;
                }

                if (neighbor)
                {
                    var interactable = neighbor.GetComponent<Interactable>();
                    switch (interactable)
                    {
                        case Stone:
                            EventCenter.Broadcast(EventType.PossessStone, true);
                            break;
                        
                        case Wall:
                            EventCenter.Broadcast(EventType.PossessWall, true);
                            break;
                        
                        case RouteStone:
                            EventCenter.Broadcast(EventType.PossessRouteStone, true);
                            break;
                    }
                }
            }
            else
            {
                TryReclaimSoul();
            }
        }
    }

    private void TryReclaimSoul()
    {
        Collider2D[] nearbyObjects = Physics2D.OverlapCircleAll(transform.position, soulDetectionRadius);

        foreach (Collider2D nearbyObject in nearbyObjects)
        {
            if (nearbyObject.CompareTag("soul"))
            {
                hasSoul = true;
                Destroy(nearbyObject);
                break;
            }
        }
    }
    
    private void Update()
    {
        Move();
        Possess();
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, soulDetectionRadius);
    }
}
