using UnityEngine;

namespace Objects.Interactables
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class RouteStone : Interactable
    {
        [SerializeField] private GameObject[] waypoints;
        private int currentWaypointIndex = 0;
        [SerializeField] private float waypointSpeed = 2f;
        
        private bool isMovingToWaypoint = false;
        
        protected override void Start()
        {
            base.Start();
            
            if (waypoints.Length == 0)
            {
                Debug.LogError("RouteStone: No waypoints assigned.");
            }
        }
        
        protected override void Update()
        {
            // Check if space is pressed to trigger waypoint movement
            if (Input.GetKeyDown(KeyCode.Space) && !isMovingToWaypoint && waypoints.Length > 0)
            {
                isMovingToWaypoint = true;
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    currentWaypointIndex = 0;
                }
            }
            
            // If moving to waypoint, handle that movement
            if (isMovingToWaypoint && waypoints.Length > 0)
            {
                MoveToWaypoint();
            }
        }
        
        private void MoveToWaypoint()
        {
            if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < 0.1f)
            {
                // We've reached the waypoint
                isMovingToWaypoint = false;
                rb.linearVelocity = Vector2.zero;
                return;
            }
            
            // Move toward the current waypoint
            Vector2 direction = (waypoints[currentWaypointIndex].transform.position - transform.position).normalized;
            rb.linearVelocity = direction * waypointSpeed;
        }
    }
}