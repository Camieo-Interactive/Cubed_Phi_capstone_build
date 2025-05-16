// Class Filled with utils functions and constants..


using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;


public static class CubedPhiUtils {
    public const int ENEMY_LAYER = 6;
    public const int TOWER_LAYER = 7;
    public const int ENEMY_BULLET_LAYER = 8;
    public const int TOWER_BULLET_LAYER = 9;

    public static bool IsPointerOverUI(Vector2 screenPos, GraphicRaycaster raycaster, EventSystem eventSystem) {
        PointerEventData eventData = new PointerEventData(eventSystem)
        {
            position = screenPos
        };

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);

        return results.Count > 0;
    }
}