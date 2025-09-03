using UnityEngine;


[AddComponentMenu("MiniGames/CodeMiniGame/Draggable")]
[DisallowMultipleComponent]
[
    RequireComponent(typeof(Collider2D))
]
public class Draggable : MonoBehaviour
{
    private Camera mainCamera;
    private Plane dragSurface;
    private Vector3 grabOffsetWorld;
    private bool isDragging;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void OnEnable()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void OnMouseDown()
    {
        if (mainCamera == null)
        {
            return;
        }

        // Always drag on XY plane in 2D
        Vector3 normal = Vector3.forward;
        dragSurface = new Plane(normal, transform.position);

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (dragSurface.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            grabOffsetWorld = transform.position - hitPoint;
            isDragging = true;
        }
    }

    private void OnMouseDrag()
    {
        if (!isDragging || mainCamera == null)
        {
            return;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (dragSurface.Raycast(ray, out float enter))
        {
            Vector3 target = ray.GetPoint(enter) + grabOffsetWorld;
            MoveTo(target);
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
        grabOffsetWorld = Vector3.zero;

        // Если отпустили внутри зоны — разместить по правилам зоны (тип не проверяем здесь)
        DropZone2D zone = DropZone2D.FindContaining(transform.position);
        if (zone != null)
        {
            Vector3 snapped = zone.GetPlacementPosition(transform.position);
            transform.position = snapped;

            // Сообщить зоне, что блок помещён
            zone.RaiseBlockDropped(gameObject);
        }
    }

    private void MoveTo(Vector3 worldPosition)
    {
        transform.position = worldPosition;
    }
}


