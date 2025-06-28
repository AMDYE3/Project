using UnityEngine;
using System.Collections.Generic;
namespace Objects.Interactables
{
public class StopCollidingWith : MonoBehaviour
{
    [Tooltip("Tags of objects to ignore collision with")]
    public string[] tagNames;

    private Collider2D myCollider;
    private HashSet<Collider2D> ignored = new HashSet<Collider2D>();
    private float checkInterval = .2f;
    private float timer = 0f;

    void Start()
    {
        myCollider = GetComponent<Collider2D>();
        if (myCollider == null)
        {
            Debug.LogError($"{name}: No Collider2D found.");
        }

        CheckAndIgnore(); // Initial check
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= checkInterval)
        {
            timer = 0f;
            CheckAndIgnore();
        }
    }

    void CheckAndIgnore()
    {
        if (myCollider == null) return;

        foreach (string tag in tagNames)
        {
            if (string.IsNullOrEmpty(tag)) continue;

            GameObject[] objs = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject obj in objs)
            {
                Collider2D otherCol = obj.GetComponent<Collider2D>();
                if (otherCol != null && !ignored.Contains(otherCol))
                {
                    Physics2D.IgnoreCollision(myCollider, otherCol);
                    ignored.Add(otherCol);
                }
            }
        }
    }
} 
}