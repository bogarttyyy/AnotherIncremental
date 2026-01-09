using NSBLib.Interfaces;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace NSBLib.Managers
{
    
    public class ClickInputManager : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private float maxDistance = 1000f;
        [SerializeField] private LayerMask clickableLayers = ~0;
        [SerializeField] private bool ignoreUI = true;
        [SerializeField] private float dragThreshold = 10f; // Pixels allowed to move before it becomes a drag

        private Vector2 startMousePosition;
        private bool isPotentialClick;
        private bool isFirstClick;
        private bool isClickHeld;
        
        // Dragging state
        private IDraggable currentDraggable;
        private bool isDragging;
        private Plane dragPlane;
        private bool is3DDrag;

        private void Awake()
        {
            cam ??= Camera.main;
        }

        public void Update()
        {
            // New Input System mouse
            var mouse = Mouse.current;
            if (mouse == null) 
                return; // e.g. on touch-only device
            
            Vector2 screenPos = mouse.position.ReadValue();
            
            // 1. On Mouse Down: Record start position and check UI
            if (mouse.leftButton.wasPressedThisFrame || mouse.rightButton.wasPressedThisFrame)
            {
                if (!isClickHeld)
                {
                    isFirstClick = true;
                }
                
                if (ignoreUI && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                {
                    isPotentialClick = false;
                    return;
                }
                
                startMousePosition = mouse.position.ReadValue();
                isPotentialClick = true;
                isDragging = false;
                currentDraggable = null;
                
                // Check 3D Hit for Dragging
                Ray ray = cam.ScreenPointToRay(screenPos);
                if (Physics.Raycast(ray, out var hit3D, maxDistance, clickableLayers))
                {
                    var draggable = hit3D.collider.GetComponentInParent<IDraggable>();
                    if (draggable != null)
                    {
                        currentDraggable = draggable;
                        is3DDrag = true;
                        // Create a plane at the object's position facing the camera for smooth dragging
                        dragPlane = new Plane(-cam.transform.forward, hit3D.transform.position);
                    }
                }
                // Check 2D Hit for Dragging (if no 3D hit found or prefer 2D)
                else
                {
                    RaycastHit2D hit2D = Physics2D.Raycast(cam.ScreenToWorldPoint(screenPos), Vector2.zero, maxDistance, clickableLayers);
                    if (hit2D.collider != null)
                    {
                        var draggable = hit2D.collider.GetComponentInParent<IDraggable>();
                        if (draggable != null)
                        {
                            currentDraggable = draggable;
                            is3DDrag = false;
                        }
                    }
                }
                
                if (isFirstClick)
                    currentDraggable?.OnInitialClick(screenPos);
            }
            
            // 2. While Holding: Process Drag
            if (mouse.leftButton.isPressed ||  mouse.rightButton.isPressed)
            {
                if (isFirstClick)
                {
                    isFirstClick = false;
                    isClickHeld = true;
                }
                
                if (currentDraggable != null)
                {
                    // Check if we crossed the threshold to start dragging
                    if (!isDragging)
                    {
                        if (Vector2.Distance(startMousePosition, screenPos) > dragThreshold)
                        {
                            isDragging = true;
                            isPotentialClick = false; // Cancel the click since we are dragging
                        }
                    }

                    // Perform the drag
                    if (isDragging)
                    {
                        Vector3 targetPosition = Vector3.zero;

                        if (is3DDrag)
                        {
                            Ray ray = cam.ScreenPointToRay(screenPos);
                            if (dragPlane.Raycast(ray, out float enter))
                            {
                                targetPosition = ray.GetPoint(enter);
                            }
                        }
                        else // 2D
                        {
                            targetPosition = cam.ScreenToWorldPoint(screenPos);
                            targetPosition.z = -1; // Ensure Z is 0 for 2D
                        }

                        currentDraggable.OnDragged(targetPosition);
                    }
                }
            }

            // 3. On Mouse Up: Check distance to determine if it was a Click or Drag
            if (mouse.leftButton.wasReleasedThisFrame || mouse.rightButton.wasReleasedThisFrame)
            {
                // Release draggable
                currentDraggable?.OnDragReleased();
                
                // Reset drag state
                currentDraggable = null;
                isDragging = false;
                isClickHeld = false;
                
                if (!isPotentialClick) 
                    return;

                isPotentialClick = false;

                // Vector2 screenPos = mouse.position.ReadValue();
                if (Vector2.Distance(startMousePosition, screenPos) > dragThreshold)
                    return; // Moved too much, treated as a drag

                // NSBLogger.Log($"Click detected!");
            
                Ray ray = cam.ScreenPointToRay(screenPos);

                if (Physics.Raycast(ray, out var hit3D, maxDistance, clickableLayers))
                {
                    //NSBLogger.Log($"Hit3D: {hit3D.collider.gameObject.name}");

                    if (mouse.rightButton.wasReleasedThisFrame)
                    {
                        var rightClickable = hit3D.collider.GetComponentInParent<IRightClickable>();
                        rightClickable?.OnRightClicked();
                    }
                    else
                    {
                        var clickable = hit3D.collider.GetComponentInParent<IClickable>();
                        clickable?.OnClicked();
                    }
                    return;
                }
            
                RaycastHit2D hit2D = Physics2D.Raycast(cam.ScreenToWorldPoint(screenPos), Vector2.zero, maxDistance, clickableLayers);
                if (hit2D.collider != null)
                {
                    // NSBLogger.Log($"Hit2D: {hit2D.collider.gameObject.name}");
                    if (mouse.rightButton.wasReleasedThisFrame)
                    {
                        var rightClickable = hit2D.collider.GetComponentInParent<IRightClickable>();
                        rightClickable?.OnRightClicked();
                    }
                    else
                    {
                        var clickable = hit2D.collider.GetComponentInParent<IClickable>();
                        clickable?.OnClicked();
                    }
                }
                else
                {
                    // NSBLogger.Log($"Null Hit: {hit2D.collider}");
                }
            }
        }
    }
}