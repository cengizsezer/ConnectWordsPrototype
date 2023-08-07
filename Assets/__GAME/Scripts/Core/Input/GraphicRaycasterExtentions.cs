using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public static class GraphicRaycasterExtensions
{
    public static T GetUIElement<T>(this GraphicRaycaster graphicRaycaster)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;

        var results = new List<RaycastResult>();
        graphicRaycaster.Raycast(eventData, results);

        foreach (var result in results)
        {
            T uiElement = result.gameObject.GetComponent<T>();
            if (uiElement != null)
            {
                return uiElement;
            }
        }

        return default; 
    }
}
