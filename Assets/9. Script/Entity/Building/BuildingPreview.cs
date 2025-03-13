using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    [SerializeField] private Material validMaterial; // 배치 가능 시 (초록색)
    [SerializeField] private Material invalidMaterial;

    private MeshRenderer meshRenderer;
    private Collider previewCollider;
    private bool canPlace = false;


    private void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        previewCollider = GetComponent<Collider>();

        if (meshRenderer == null)
            Debug.LogError("BuildingPreview: MeshRenderer가 없습니다.");

        if (previewCollider == null)
            Debug.LogError("BuildingPreview: Collider가 없습니다.");
    }


    private void Update()
    {
        FollowMouse();
        UpdatePreviewColor();
    }
    private void FollowMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            transform.position = hit.point;
        }
    }


    private void UpdatePreviewColor()
    {
        if (meshRenderer != null)
        {
            meshRenderer.material = canPlace ? validMaterial : invalidMaterial;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        canPlace = false;
        UpdatePreviewColor();
    }

    private void OnTriggerExit(Collider other)
    {
        canPlace = true;
        UpdatePreviewColor();
    }

    public bool CanPlace()
    {
        return canPlace;
    }


}
