using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Зона для размещения перетаскиваемых объектов (2D).
/// Требуется Collider2D (желательно isTrigger). При отпускании объекта внутри зоны
/// объект будет зафиксирован внутри зоны (по центру или с зажатием в границах).
/// </summary>
[AddComponentMenu("MiniGames/CodeMiniGame/DropZone2D")]
[DisallowMultipleComponent]
[RequireComponent(typeof(Collider2D))]
public class DropZone2D : MonoBehaviour
{

    [Tooltip("Какой тип блока принимает зона")]
    [SerializeField] private CodeBlock.BlockType acceptedType;

    public enum SnapMode
    {
        ClampInside,    // Зажать внутри границ зоны с отступами
        SnapToCenter    // Привязать к центру зоны
    }


    [Tooltip("Как позиционировать объект при отпускании в зоне")]
    public SnapMode snapMode = SnapMode.ClampInside;

    [Tooltip("Отступы внутри зоны при режиме ClampInside")]
    public Vector2 padding = Vector2.zero;


    [Tooltip("C# event: вызывается при помещении блока в зону. Передаётся orderIndex блока.")]
    public event Action<int> OnBlockDropped;

    private static readonly HashSet<DropZone2D> registry = new HashSet<DropZone2D>();
    private Collider2D zoneCollider;

    private void Awake()
    {
        zoneCollider = GetComponent<Collider2D>();
        if (zoneCollider != null)
        {
            // Зона — это триггер, симуляцию физики не используем
            zoneCollider.isTrigger = true;
        }
    }

    private void OnEnable()
    {
        registry.Add(this);
    }

    private void OnDisable()
    {
        registry.Remove(this);
    }

    /// <summary>
    /// Возвращает зону, содержащую указанную мировую позицию, либо null.
    /// </summary>
    public static DropZone2D FindContaining(Vector3 worldPosition)
    {
        foreach (var zone in registry)
        {
            if (zone.zoneCollider != null && zone.zoneCollider.bounds.Contains(worldPosition))
            {
                return zone;
            }
        }
        return null;
    }

    /// <summary>
    /// Возвращает позицию объекта в пределах зоны согласно настройкам.
    /// </summary>
    public Vector3 GetPlacementPosition(Vector3 worldPosition)
    {
        if (zoneCollider == null)
        {
            return worldPosition;
        }

        Bounds b = zoneCollider.bounds;

        if (snapMode == SnapMode.SnapToCenter)
        {
            return new Vector3(b.center.x, b.center.y, worldPosition.z);
        }

        float minX = b.min.x + padding.x;
        float maxX = b.max.x - padding.x;
        float minY = b.min.y + padding.y;
        float maxY = b.max.y - padding.y;

        float clampedX = Mathf.Clamp(worldPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(worldPosition.y, minY, maxY);
        return new Vector3(clampedX, clampedY, worldPosition.z);
    }

    /// <summary>
    /// Вызвать событие помещения блока (передаёт orderIndex).
    /// </summary>
    public void RaiseBlockDropped(GameObject blockObject)
    {
        if (blockObject == null)
        {
       
            return;
        }

        CodeBlock codeBlock = blockObject.GetComponent<CodeBlock>();
        if (codeBlock == null)
        {
            
            return;
        }

        if (OnBlockDropped != null)
        {
            OnBlockDropped?.Invoke(codeBlock.OrderIndex);
        }
        else
        {
            Debug.LogWarning("DropZone2D: OnBlockDropped has no subscribers");
        }
    }
}


