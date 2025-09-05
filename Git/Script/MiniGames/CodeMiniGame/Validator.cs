using System.Collections.Generic;
using UnityEngine;

public class Validator : MonoBehaviour
{
    [Header("Зоны для отслеживания")]
    [SerializeField] private List<DropZone2D> trackedZones = new List<DropZone2D>();

    [Header("Собранный порядок блоков")]
    [SerializeField] private List<int> placedOrder = new List<int>();
    [SerializeField] private CodeValidatorSO codeValidator;

    private void OnEnable()
    {
        // Если список пуст — попробуем найти зоны в сцене автоматически
        if (trackedZones == null || trackedZones.Count == 0)
        {
            var found = FindObjectsOfType<DropZone2D>();
            if (found != null && found.Length > 0)
            {
                trackedZones = new List<DropZone2D>(found);
            }
            else
            {
                Debug.LogWarning("Validator: no DropZone2D found to subscribe");
            }
        }

        foreach (var zone in trackedZones)
        {
            if (zone != null)
            {
                zone.OnBlockDropped += OnZoneBlockDropped;
            }
        }
    }

    private void OnDisable()
    {
        foreach (var zone in trackedZones)
        {
            if (zone != null)
            {
                zone.OnBlockDropped -= OnZoneBlockDropped;
            }
        }
    }

    // Добавляет orderIndex в список при срабатывании события зоны
    private void OnZoneBlockDropped(int orderIndex)
    {
        placedOrder.Add(orderIndex);
    }

    public void ValidateCode()
    {
        if (Validate()) Debug.Log("GOOD");
        else Debug.Log("BAD");
    }

    private bool Validate()
    {
        bool isCorrect = false;

        for(int i = 0; i<placedOrder.Count; i++)
        {
            if (placedOrder[i] == codeValidator.CorrectOrder[i]) { isCorrect = true; }
            else { isCorrect = false; }
         }

        return isCorrect;
    }
}
