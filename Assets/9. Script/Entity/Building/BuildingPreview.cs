using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    [SerializeField] private Material validMaterial; // ��ġ ���� �� (�ʷϻ�)
    [SerializeField] private Material invalidMaterial;

    private MeshRenderer meshRenderer;
    private Collider previewCollider;
    private bool canPlace = false;


    private void Start()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        previewCollider = GetComponent<Collider>();

        if (meshRenderer == null)
            Debug.LogError("BuildingPreview: MeshRenderer�� �����ϴ�.");

        if (previewCollider == null)
            Debug.LogError("BuildingPreview: Collider�� �����ϴ�.");
    }


    private void Update()
    {
        FollowMouse();
        UpdatePreviewColor();
    }
    private void FollowMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
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
