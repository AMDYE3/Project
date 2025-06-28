using UnityEngine;
using EventSystem;
using EventType = EventSystem.EventType;

namespace Objects.Interactables
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Wall : Interactable
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();
            rb.gravityScale = 0f;
            EventCenter.AddListener<bool>(EventType.PossessWall, SetSoul);
        }
        
        private void OnDisable()
        {
            EventCenter.RemoveListener<bool>(EventType.PossessWall, SetSoul);
        }
    }
}