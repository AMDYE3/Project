using System;
using UnityEngine;

namespace Objects
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class EndPoint : MonoBehaviour
    {
        [SerializeField] private Sprite noSoulSprite;
        [SerializeField] private Sprite hasSoulSprite;
        private SpriteRenderer spriteRenderer;
        
        private void Start()
        {
            // Ensure the collider is set as a trigger
            var collider = GetComponent<BoxCollider2D>();
            collider.isTrigger = true;

            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.sprite = hasSoulSprite;
            
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

            // Update the sprite based on whether the player has the soul
            spriteRenderer.sprite = hasSoul ? hasSoulSprite : noSoulSprite;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && other.GetComponent<PlayerController>().HasSoul)
            {
                EventSystem.EventCenter.Broadcast(EventSystem.EventType.ReachEnd);
            }
        }
    }
}
