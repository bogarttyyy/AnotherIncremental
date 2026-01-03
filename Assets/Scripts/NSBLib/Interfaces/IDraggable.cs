using UnityEngine;

namespace Interfaces
{
    public interface IDraggable
    {
        void OnInitialClick(Vector3 position);
        void OnDragged(Vector3 position);
        void OnDragReleased();
    }
}