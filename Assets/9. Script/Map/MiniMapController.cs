using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapController: MonoBehaviour
{ public Transform playerTransform;  
    public RectTransform miniMapTransform;  
    public RectTransform miniMapPlayerIcon; 
    public float scaleFactor = 1.0f; 

    void Update()
    {
        if (playerTransform == null) return;

        miniMapTransform.rotation = Quaternion.Euler(0, 0, -playerTransform.eulerAngles.y);

        Vector3 worldPos = playerTransform.position;
        miniMapPlayerIcon.anchoredPosition = new Vector2(worldPos.x * scaleFactor, worldPos.z * scaleFactor);
    }
}
