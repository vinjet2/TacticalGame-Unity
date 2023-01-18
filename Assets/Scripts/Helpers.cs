using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class Helpers {
    
    // Référenace à la Caméra
    private static Camera _camera;

    public static Camera Camera {
        get {
            if (_camera == null) _camera = Camera.main;
            return _camera;
        }
    }

    // Cursor Over UI
    private static PointerEventData _eventDataCurrentPosition;
    //private static List<RayCastResult> _results;

    /*public static bool IsOverUI() {
        _eventDataCurrentPosition = new PointerEventData(EventSystem.current) { position = Input.mousePosition};
        _result = new List<RayCastResult>();
        EventSystem.current.RaycastAll(_eventDataCurrentPosition, _results);
        return _results.Count > 0;
    }*/

    // Find world point of canvas element
    public static Vector2 GetWorldPositionOfCanvasElement(RectTransform element){
        RectTransformUtility.ScreenPointToWorldPointInRectangle(element, element.position, Camera, out var result);
        return result;
    }

    // Quickly destroy all child objects
    public static void DeleteChildren(this Transform t) {
        foreach (Transform child in t) Object.Destroy(child.gameObject);
    }

}
