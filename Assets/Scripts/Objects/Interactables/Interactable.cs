using System;
using UnityEngine;

namespace Objects.Interactables
{
    public abstract class Interactable : MonoBehaviour
    {
        protected bool soul;
        protected bool penetrable;
        protected int height;
        protected Rigidbody2D rb;
        [SerializeField] protected float moveSpeed = 5f;

        protected Vector3 lastPosition;
        protected Action<Vector2, Vector2> onMoved;
        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.gravityScale = 0f;
            lastPosition = transform.position;
            soul = false;
            rb.bodyType = RigidbodyType2D.Static; // Default to static
            height = 0;
        }

        protected enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        protected virtual void Move()
        {
            if (!soul)
            {
                return;
            }
            
            Vector2 movement = Vector2.zero;

            if (Input.GetKey(KeyCode.RightArrow))
            {
                movement += Vector2.right;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                movement += Vector2.left;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                movement += Vector2.up;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                movement += Vector2.down;
            }
            rb.linearVelocity = movement * moveSpeed;
        }
        
        public void SetSoul(bool soul)
        {
            if (soul)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
            else
            {
                // Make it be static
                rb.bodyType = RigidbodyType2D.Static;
            }
            this.soul = soul;
        }
        
        public bool GetSoul()
        {
            return soul;
        }
    
        public void SetPenetrable(bool penetrable)
        {
            this.penetrable = penetrable;
        }
        public bool GetPenetrable()
        {
            return penetrable;
        }
    
        public void SetHeight(int height)
        {
            this.height = height;
        }
    
        public int GetHeight()
        {
            return height;
        }
    
        public Vector3 GetLocation()
        {
            return transform.position;
        }
    
    
        public void SetMoveCallback(Action<Vector2, Vector2> updateMap)
        {
            onMoved = updateMap;
        }
    
        protected virtual void Update()
        {
            Move(); 
            if(rb.linearVelocity.magnitude > 0)
            {
                onMoved?.Invoke(lastPosition, transform.position);
                lastPosition = transform.position;
            }
        }
    }
}
