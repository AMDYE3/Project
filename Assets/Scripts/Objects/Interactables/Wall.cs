using UnityEngine;
using EventSystem;
using EventType = EventSystem.EventType;

namespace Objects.Interactables
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Wall : Interactable
    {
        protected override void Start()
        {
            base.Start();
            EventCenter.AddListener<bool>(EventType.PossessWall, SetSoul);
        }
        
        private void OnDisable()
        {
            EventCenter.RemoveListener<bool>(EventType.PossessWall, SetSoul);
        }
    }
}