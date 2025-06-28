using EventSystem;
using UnityEngine;
using EventType = EventSystem.EventType;

namespace Objects.Interactables
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Stone : Interactable
    {
        protected override void Start()
        {
            base.Start();
            rb.gravityScale = 0f;
            EventCenter.AddListener<bool>(EventType.PossessStone, SetSoul);
        }

        private void OnDisable()
        {
            EventCenter.RemoveListener<bool>(EventType.PossessStone, SetSoul);
        }
    }

}