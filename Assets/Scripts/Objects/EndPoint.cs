using System;
using UnityEngine;

namespace Objects
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class EndPoint : MonoBehaviour
    {
        private SpriteRenderer spriteRenderer;
        private bool isFinished = false;
        
        private void Start()
        {
            // Ensure the collider is set as a trigger
            var collider = GetComponent<BoxCollider2D>();
            collider.isTrigger = true;

            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            
            // Set all children's scale to 0.35
            foreach (Transform child in transform)
            {
                child.localScale = new Vector3(0.3f, 0.3f, 1f);
            }
        }

        private void Update()
        {
            // Check if the player has the soul
            bool hasSoul = GameObject.FindWithTag("Player")?.GetComponent<PlayerController>().HasSoul ?? false;

            // Update the sprite based on whether the player has the soul, if not, set sprite color darker
            spriteRenderer.color = hasSoul ? Color.white : new Color(0.5f, 0.5f, 0.5f, 1f);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && other.GetComponent<PlayerController>().HasSoul && !isFinished)
            {
                isFinished = true;
                EventSystem.EventCenter.Broadcast(EventSystem.EventType.ReachEnd);
            }
        }
    }
}
