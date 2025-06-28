using UnityEngine;

namespace Objects
{
    public class EndPoint : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                EventSystem.EventCenter.Broadcast(EventSystem.EventType.ReachEnd);
            }
        }
    }
}
