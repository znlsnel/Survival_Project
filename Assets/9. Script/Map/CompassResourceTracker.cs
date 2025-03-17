using UnityEngine;
using UnityEngine.UI;

public class CompassResourceTracker : MonoBehaviour
{
    public Transform player; 
    public RectTransform compassUI; 
    public GameObject resourceMarkerPrefab; 
    public float detectionRadius = 50f; 
    public LayerMask resourceLayer; 

    private RectTransform rectTransform;
    private GameObject resourceMarker;
    private float compassWidth;

    void Start()
    {
        compassWidth = compassUI.rect.width / 2;
        resourceMarker = Instantiate(resourceMarkerPrefab, compassUI);
        resourceMarker.SetActive(false);
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(110, rectTransform.sizeDelta.y);
    
}

    void Update()
    {
         rectTransform.sizeDelta = new Vector2(110, rectTransform.sizeDelta.y);
    
        UpdateResourceMarker();
    }

    void UpdateResourceMarker()
    {
        Collider[] resources = Physics.OverlapSphere(player.position, detectionRadius, resourceLayer);

        if (resources.Length > 0)
        {
            Transform closestResource = resources[0].transform;
            Vector3 dirToResource = closestResource.position - player.position;
            dirToResource.y = 0; 

            Vector3 forward = player.forward;
            forward.y = 0;
            forward.Normalize();

            float angle = Vector3.SignedAngle(forward, dirToResource, Vector3.up);

            float markerX = (angle / 180f) * compassWidth;
            resourceMarker.GetComponent<RectTransform>().anchoredPosition = new Vector2(markerX, 0);
            resourceMarker.SetActive(true);
        }
        else
        {
            resourceMarker.SetActive(false);
        }
    }
}
