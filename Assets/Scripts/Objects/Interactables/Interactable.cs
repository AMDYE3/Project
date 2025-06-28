using System;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected bool soul;
    protected bool penetrable;
    protected int height;
    protected Rigidbody2D rb;

    private Vector3 lastPosition;
    private Action<Vector2, Vector2> onMoved;
    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastPosition = transform.position;
        soul = false;
        height = 0;
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
       if(rb.linearVelocity.magnitude > 0)
        {
            onMoved?.Invoke(lastPosition, transform.position);
            lastPosition = transform.position;
        }
    }
}
