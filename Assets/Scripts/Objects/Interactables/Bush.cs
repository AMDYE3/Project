using Objects.Interactables;
using Unity.VisualScripting;
using UnityEngine;
using EventSystem;
using EventType = EventSystem.EventType;

namespace Objects.Interactables
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    
    public class Bush : Interactable
    {
        private bool hasSpread;
        bool canSpread(GameObject obj)
        {
            if (obj != null && obj.CompareTag("Soil"))
            {
                return true;
            }

            return false;
        }
        void Spread()
        {
            Vector2Int dim = WorldManager.Instance.WorldSize;
            int cols = dim.x;
            int rows = dim.y;

            Vector2Int idx = WorldManager.Instance.GetIndex(this.gameObject);
            int x = idx.x; // 列
            int y = idx.y; // 行

            // 8个方向：上，下，左，右，左上，右上，左下，右下
            int[,] directions = new int[,] {
                {-1, 0},  // 上
                {1, 0},   // 下
                {0, -1},  // 左
                {0, 1},   // 右
                {-1, -1}, // 左上
                {-1, 1},  // 右上
                {1, -1},  // 左下
                {1, 1}    // 右下
            };

            for (int i = 0; i < 8; i++) {
                int newX = x + directions[i, 1]; // col
                int newY = y + directions[i, 0]; // row

                if (newX >= 0 && newX < cols && newY >= 0 && newY < rows) {
                    Vector2Int neighborIdx = new Vector2Int(newX, newY);
                    GameObject obj = WorldManager.Instance.GetObject(neighborIdx);
                    if (canSpread(obj))
                    {
                        WorldManager.Instance.CreateObject(7, neighborIdx); // bush index is 7
                    }
                }
            }
        }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected  override void Start()
        {
            base.Start();
            hasSpread = false;
            EventCenter.AddListener<bool>(EventType.PossessBush, SetSoul);
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            if (hasSpread&&Input.GetKeyDown(KeyCode.Space))
            {
                Spread();
                
            }
        }

        private void OnDisable()
        {
            EventCenter.RemoveListener<bool>(EventType.PossessBush, SetSoul);
        }
    }    
}

