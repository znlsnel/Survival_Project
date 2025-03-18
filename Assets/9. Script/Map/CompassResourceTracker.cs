using UnityEngine;
using System.Collections.Generic;

public class CompassResourceTracker : MonoBehaviour
{
    public Transform player;
    public RectTransform compassUI;
    public GameObject resourceMarkerPrefab;
    public float detectionRadius = 50f;
    public LayerMask resourceLayer;
    private RectTransform rectTransform;
    private float compassWidth;
    private Dictionary<Transform, GameObject> resourceMarkers = new Dictionary<Transform, GameObject>();

    void Start()
    {
        compassWidth = compassUI.rect.width / 2;
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(110, rectTransform.sizeDelta.y);


    }

    void Update()
    {
        UpdateResourceMarkers();
        rectTransform.sizeDelta = new Vector2(110, rectTransform.sizeDelta.y);
    }

    void UpdateResourceMarkers()
    {
        Collider[] resources = Physics.OverlapSphere(player.position, detectionRadius, resourceLayer);
        HashSet<Transform> detectedResources = new HashSet<Transform>();

        foreach (Collider resource in resources)
        {
            Transform resourceTransform = resource.transform;
            detectedResources.Add(resourceTransform);

            if (!resourceMarkers.ContainsKey(resourceTransform))
            {
                GameObject marker = Instantiate(resourceMarkerPrefab, compassUI);
                resourceMarkers[resourceTransform] = marker;
            }

            Vector3 dirToResource = resourceTransform.position - player.position;
            dirToResource.y = 0;

            Vector3 forward = player.forward;
            forward.y = 0;
            forward.Normalize();

            float angle = Vector3.SignedAngle(forward, dirToResource, Vector3.up);
            float markerX = (angle / 180f) * compassWidth;

            // 마커 UI 업데이트
            GameObject markerObject = resourceMarkers[resourceTransform];
            markerObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(markerX, 0);
            markerObject.SetActive(true);
        }

        // 사라진 자원 마커 제거
        foreach (var resource in new List<Transform>(resourceMarkers.Keys))
        {
            if (!detectedResources.Contains(resource))
            {
                Destroy(resourceMarkers[resource]);
                resourceMarkers.Remove(resource);
            }
        }
    }
}
