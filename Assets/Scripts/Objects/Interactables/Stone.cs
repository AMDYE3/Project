using EventSystem;
using UnityEngine;
using EventType = EventSystem.EventType;

namespace Objects.Interactables
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Animator))]
    public class Stone : Interactable
    {
        private Animator animator;
        
        protected override void Start()
        {
            base.Start();
            EventCenter.AddListener<bool>(EventType.PossessStone, SetSoul);
            animator = GetComponent<Animator>();
        }

        protected override void Update()
        {
            base.Update();
            if (rb.linearVelocity.magnitude < 0.1f)
                animator.SetBool("isRolling", false);
            else
                animator.SetBool("isRolling", true);
        }

        private void OnDisable()
        {
            EventCenter.RemoveListener<bool>(EventType.PossessStone, SetSoul);
        }
    }

}