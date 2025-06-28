using UnityEngine;
using System.Collections;

namespace Objects.Interactables
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class RouteStone : Interactable
    {
        [SerializeField] private Waypoint[] waypoints;
        private int currentWaypointIndex = 0;
        private bool isMovingToWaypoint = false;
        
        protected override void Start()
        {
            base.Start();
            
            // Delay waypoint finding slightly to ensure all waypoints have initialized
            StartCoroutine(InitializeWaypoints());
        }
        
        private IEnumerator InitializeWaypoints()
        {
            // Wait for the end of the frame to ensure all Start methods have been called
            yield return new WaitForEndOfFrame();
            
            // Find all waypoints in the scene
            waypoints = FindObjectsByType<Waypoint>(FindObjectsSortMode.None);
            
            if (waypoints.Length == 0)
            {
                Debug.LogError("RouteStone: No waypoints assigned.");
            }
        }
        
        protected override void Update()
        {
            if (waypoints.Length == 0)
            {
                return;
            }
            
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
            if (Vector2.Distance(waypoints[currentWaypointIndex].transform.position, transform.position) < 5f)
            {
                // We've reached the waypoint
                isMovingToWaypoint = false;
                rb.linearVelocity = Vector2.zero;
                return;
            }
            
            // Move toward the current waypoint
            Vector2 direction = (waypoints[currentWaypointIndex].transform.position - transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
        }
    }
}