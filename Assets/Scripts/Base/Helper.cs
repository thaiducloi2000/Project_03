using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Helper
{
    // Preference Camera
    private static Camera camera;
    public static Camera MainCamera
    {
        get
        {
            if (camera == null) camera = Camera.main;
            return camera;
        }
    }

    // Avoid Garbage Collector
    private static readonly Dictionary<float, WaitForSeconds> waitDictionary = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds GetWait(float time)
    {
        if (waitDictionary.TryGetValue(time, out var wait)) return wait;

        waitDictionary[time] = new WaitForSeconds(time);
        return waitDictionary[time];
    }

    // Is Pointer OverUI
    private static PointerEventData eventDataCurrentPosition;
    private static List<RaycastResult> result;

    public static bool isOverUI()
    {
        eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        result = new List<RaycastResult>();

        EventSystem.current.RaycastAll(eventDataCurrentPosition, result);
        return result.Count > 0;
    }

    // World Point of Canvas 
    public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element)
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, MainCamera, out var result);
        return result;
    }

    // Quick Destroy Childrent Object
    public static void DeleteChildrents(Transform t)
    {
        foreach (Transform child in t) Object.Destroy(child.gameObject);
    }
}
