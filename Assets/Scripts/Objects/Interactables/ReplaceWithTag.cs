using UnityEngine;

namespace Objects.Interactables
{
    public class ReplaceWithTag : MonoBehaviour
    {
        [SerializeField]
        private string hitWith; // 要检测的 tag

        [SerializeField]
        private int ReplaceTo; // 替换成的 prefab index

        void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(hitWith))
            {
                Vector2Int idx = WorldManager.Instance.GetIndex(this.gameObject);
                WorldManager.Instance.CreateObject(ReplaceTo, idx);
                // 不再需要 Destroy(gameObject)，CreateObject 会处理
            }
        }
    }
}