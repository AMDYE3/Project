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

        private Vector3 lastPosition;
        private Action<Vector2, Vector2> onMoved;
        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            lastPosition = transform.position;
            soul = false;
            height = 0;
        }

        protected enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }
        protected Direction currentDir;

        protected virtual void Move()
        {
            Vector2 movement = Vector2.zero;

            if (Input.GetKey(KeyCode.RightArrow))
            {
                movement += Vector2.right;
                currentDir = Direction.Right;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                movement += Vector2.left;
                currentDir = Direction.Left;
            }
            if (Input.GetKey(KeyCode.UpArrow))
            {
                movement += Vector2.up;
                currentDir = Direction.Up;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                movement += Vector2.down;
                currentDir = Direction.Down;
            }
            rb.linearVelocity = movement * moveSpeed;
        }
        public void SetSoul(bool soul)
        {
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
