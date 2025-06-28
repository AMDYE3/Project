using UnityEngine;

namespace Objects
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class EndPoint : MonoBehaviour
    {
        private void Start()
        {
            // Ensure the collider is set as a trigger
            var collider = GetComponent<BoxCollider2D>();
            collider.isTrigger = true;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                EventSystem.EventCenter.Broadcast(EventSystem.EventType.ReachEnd);
            }
        }
    }
}
