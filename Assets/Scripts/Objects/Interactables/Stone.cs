using EventSystem;
using UnityEngine;
using EventType = EventSystem.EventType;

namespace Objects.Interactables
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class Stone : Interactable
    {
        private Animator animator;
        
        protected override void Start()
        {
            base.Start();
            EventCenter.AddListener<bool>(EventType.PossessStone, SetSoul);
            animator = GetComponent<Animator>();
        }

        private void OnDisable()
        {
            EventCenter.RemoveListener<bool>(EventType.PossessStone, SetSoul);
        }
    }

}