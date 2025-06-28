using Objects.Interactables;
using UnityEngine;
using EventSystem;
using EventType = EventSystem.EventType;
namespace Objects.Interactables
{
    public class BushReplica : Interactable
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();
            SetSoul(true);
            EventCenter.AddListener<bool>(EventType.PossessBush, SetSoul);
        }

        // Update is called once per frame
        void Update()
        {
            base.Update();
            EventCenter.AddListener<bool>(EventType.PossessBush, SetSoul);
        }
    }
}
