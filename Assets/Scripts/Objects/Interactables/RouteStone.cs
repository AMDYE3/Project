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
        // Store waypoint positions instead of references to waypoint objects
        [SerializeField] private Vector3[] waypointPositions;
        [SerializeField] private Sprite hasSoulSprite;
        [SerializeField] private Sprite noSoulSprite;
        
        private SpriteRenderer spriteRenderer;
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
            
            // Get the sprite renderer component
            spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null)
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
                
            if (spriteRenderer == null)
                Debug.LogError("RouteStone: No SpriteRenderer found on this object or its children");
            
            EventCenter.AddListener<bool>(EventType.PossessRouteStone, SetSoul);
            
            // Delay waypoint finding slightly to ensure all waypoints have initialized
            StartCoroutine(InitializeWaypoints());
        }
        
        private IEnumerator InitializeWaypoints()
        {
            // Wait for the end of the frame to ensure all Start methods have been called
            yield return new WaitForEndOfFrame();
            
            // Find all waypoints in the scene
            Waypoint[] waypointComponents = FindObjectsByType<Waypoint>(FindObjectsSortMode.None);
            
            if (waypointComponents.Length == 0)
            {
                Debug.LogError("RouteStone: No waypoints assigned.");
                yield break;
            }
            
            // Create a deep copy by storing their positions
            waypointPositions = new Vector3[waypointComponents.Length];
            for (int i = 0; i < waypointComponents.Length; i++)
            {
                waypointPositions[i] = waypointComponents[i].transform.position;
            }
            
            Debug.Log($"RouteStone: Initialized with {waypointPositions.Length} waypoints");
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
            if (!soul)
            {
                return;
            }
            
            if (waypointPositions == null || waypointPositions.Length == 0)
            {
                return;
            }
            
            // Update sprite based on soul possession
            UpdateSprite();
            
            // Check if space is pressed to trigger waypoint movement
            if (Input.GetKeyDown(KeyCode.Space) && !isMovingToWaypoint && waypointPositions.Length > 0)
            {
                isMovingToWaypoint = true;
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypointPositions.Length)
                {
                    currentWaypointIndex = 0;
                }
            }
            
            // If moving to waypoint, handle that movement
            if (isMovingToWaypoint && waypointPositions.Length > 0)
            {
                MoveToWaypoint();
            }
            
            if(rb.linearVelocity.magnitude > 0)
                onMoved?.Invoke(lastPosition, transform.position);
        }
        
        private void UpdateSprite()
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.sprite = soul ? hasSoulSprite : noSoulSprite;
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
            
            if (Vector2.Distance(waypointPositions[currentWaypointIndex], transform.position) < 5f)
            {
                // We've reached the waypoint
                isMovingToWaypoint = false;
                rb.linearVelocity = Vector2.zero;
                return;
            }
            
            // Move toward the current waypoint
            Vector2 direction = ((Vector2)waypointPositions[currentWaypointIndex] - (Vector2)transform.position).normalized;
            rb.linearVelocity = direction * moveSpeed;
        }

        private void OnDisable()
        {
            EventCenter.RemoveListener<bool>(EventType.PossessRouteStone, SetSoul);
        }
    }
}