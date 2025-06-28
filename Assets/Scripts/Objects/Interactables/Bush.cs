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
        bool canSpread(GameObject obj)
        {
            if (obj != null)
            {
                if (obj.CompareTag("Soil") || obj.CompareTag("Grass"))
                {
                    return true;
                }
            }
            else
            {
                return true;
            }

            return true;
        }
        void Spread()
        {
            Vector2Int dim = WorldManager.Instance.WorldSize;
            int cols = dim.x;
            int rows = dim.y;

            Vector2Int idx = WorldManager.Instance.GetIndex(this.gameObject);
            int x = idx.x; // 列
            int y = idx.y; // 行

            int[,] directions = new int[,] {
                {-1, 0}, // 上（row-1）
                {1, 0},  // 下（row+1）
                {0, -1}, // 左（col-1）
                {0, 1}   // 右（col+1）
            };

            for (int i = 0; i < 4; i++) {
                int newX = x + directions[i, 1]; // 注意 col 是横坐标
                int newY = y + directions[i, 0]; // 注意 row 是纵坐标

                if (newX >= 0 && newX < cols && newY >= 0 && newY < rows) {
                    Vector2Int neighborIdx = new Vector2Int(newX, newY);
                    GameObject obj = WorldManager.Instance.GetObject(neighborIdx);
                    if(canSpread(obj))
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
            EventCenter.AddListener<bool>(EventType.PossessBush, SetSoul);
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            if (Input.GetKeyDown(KeyCode.Space))
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

