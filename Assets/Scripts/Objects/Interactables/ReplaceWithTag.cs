using UnityEngine;

namespace Objects.Interactables
{
    public class ReplaceWithTag : MonoBehaviour
    {
        [SerializeField]
        private string hitWith; // 要检测的 tag

        [SerializeField]
        private int ReplaceTo; // 替换成的 prefab index
        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(hitWith))
            {
                Vector2Int idx = WorldManager.Instance.GetIndex(this.gameObject);
                WorldManager.Instance.CreateObject(ReplaceTo, idx);
                if (this.gameObject != null)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}