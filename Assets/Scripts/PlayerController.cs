using System;
using EventSystem;
using Objects.Interactables;
using Unity.VisualScripting;
using UnityEngine;
using EventType = EventSystem.EventType;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float soulDetectionRadius = 100f;
    private Rigidbody2D rb;
    private bool hasSoul = true;
    
    // sub object's animator
    private Animator animator;

    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    private Direction currentDir; // 当前附身方向
    
    enum AttachType
    {
        Stone,
        Wall,
        RouteStone,
        Bush,
        None
    }
    private AttachType attachType = AttachType.None;

    public bool HasSoul
    {
        get => hasSoul;
        set => hasSoul = value;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        animator = GetComponentInChildren<Animator>();
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
        
        if (movement != Vector2.zero)
        {
            // 设置动画状态
            animator.SetBool("isWalking", true);
        }
        else
        {
            // 停止动画
            animator.SetBool("isWalking", false);
        }

        rb.linearVelocity = movement * moveSpeed;
        
        WorldManager.Instance.UpdateDepth(gameObject);
        var pos = transform.localPosition;
        pos.z -= 0.5f;
        transform.localPosition = pos;
    }

    private void Possess()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter) || Input.GetKeyDown(KeyCode.Return))
        {
            if (hasSoul)
            {
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

                if (neighbor && neighbor.GetComponent<Interactable>())
                {
                    hasSoul = false; // 玩家失去灵魂
                    
                    animator.SetTrigger("spell");
                    
                    var interactable = neighbor.GetComponent<Interactable>();
                    switch (interactable)
                    {
                        case Stone:
                            EventCenter.Broadcast(EventType.PossessStone, true);
                            attachType = AttachType.Stone;
                            break;
                        
                        case Wall:
                            EventCenter.Broadcast(EventType.PossessWall, true);
                            attachType = AttachType.Wall;
                            break;
                        
                        case RouteStone:
                            EventCenter.Broadcast(EventType.PossessRouteStone, true);
                            attachType = AttachType.RouteStone;
                            break;
                        case BushOriginal:
                            EventCenter.Broadcast(EventType.PossessBush, true);
                            attachType = AttachType.Bush;
                            break;
                        case BushReplica:
                            EventCenter.Broadcast(EventType.PossessBush, true);
                            attachType = AttachType.Bush;
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
            var interactable = nearbyObject.GetComponent<Interactable>();
            if (interactable != null && interactable.GetSoul())
            {
                // Broadcast event to stop possession
                switch (attachType)
                {
                    case AttachType.Stone:
                        EventCenter.Broadcast(EventType.PossessStone, false);
                        break;
                    case AttachType.Wall:
                        EventCenter.Broadcast(EventType.PossessWall, false);
                        break;
                    case AttachType.RouteStone:
                        EventCenter.Broadcast(EventType.PossessRouteStone, false);
                        break;
                    case AttachType.Bush:
                        EventCenter.Broadcast(EventType.PossessBush, false);
                        break;                    
                    
                }
            
                // Reclaim the soul
                hasSoul = true;
                attachType = AttachType.None;
                break; // Exit after reclaiming from the first valid object
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
