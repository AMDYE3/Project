using System;
using UnityEngine;
using System.Collections;
using EventSystem;
using EventType = EventSystem.EventType;

namespace Objects.Interactables
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class RouteStone : Interactable
    {
        [SerializeField] private Waypoint[] waypoints;
        private int currentWaypointIndex = 0;
        private bool isMovingToWaypoint = false;
        
        // Reference to the WorldManager
        private WorldManager worldManager;
        // Current grid position
        private Vector2Int currIdx;
        
        protected override void Start()
        {
            base.Start();
            
            // Get the WorldManager instance
            worldManager = WorldManager.Instance;
            
            EventCenter.AddListener<bool>(EventType.PossessRouteStone, SetSoul);
            
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
        
        // Check if there's a neighbor in a specific direction
        private GameObject CheckNeighbor(Vector2Int direction)
        {
            return worldManager.GetObject(currIdx + direction);
        }
        
        // Helper method to check neighbor above
        private GameObject GetNeighborAbove()
        {
            return CheckNeighbor(new Vector2Int(0, 1));
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
        
        // Update the grid position before moving
        private void MoveToWaypoint()
        {
            // Example of checking a neighbor before moving
            GameObject neighbor = CheckNeighbor(new Vector2Int(0, 1));
            if (neighbor != null)
            {
                // Do something with the neighbor if needed
                Debug.Log("Found neighbor above: " + neighbor.name);
            }
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

        private void OnDisable()
        {
            EventCenter.RemoveListener<bool>(EventType.PossessRouteStone, SetSoul);
        }
    }
}