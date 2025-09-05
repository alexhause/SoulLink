using UnityEngine;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject tooltipPanel;


    public void OnPointerEnter(PointerEventData eventData)
    {
        ShowTooltip("");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }

    public void ShowTooltip(string text)
    {
        tooltipPanel.SetActive(true); 
    }

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}
