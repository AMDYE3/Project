using UnityEngine;

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
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
        }
    }
}