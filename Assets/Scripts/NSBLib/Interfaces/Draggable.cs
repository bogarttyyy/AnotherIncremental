using NSBLib.Helpers;
using UnityEngine;

namespace Interfaces
{
    public abstract class Draggable : MonoBehaviour, IDraggable
    {
        public bool isDraggable = true;
        
        public virtual void OnInitialClick(Vector3 position)
        {
            
        }

        public virtual void OnDragged(Vector3 position)
        {
            if (!isDraggable) return;
        }

        public virtual void OnDragReleased()
        {
            
        }

        protected void SetIsDraggable(bool value)
        {
            isDraggable = value;
        }
    }
}