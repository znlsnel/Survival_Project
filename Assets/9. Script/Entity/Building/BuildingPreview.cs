using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    [SerializeField] private LayerMask placementLayer;

    private MeshRenderer meshRenderer;
    private Collider previewCollider;
    private bool canPlace = false;

    
    private MeshRenderer previewRenderer;

    private Vector3 targetPosition;
    private Material[] originalMaterials;   // 원래 가지고 있던 메테리얼
    private Material[] previewMaterials;

    private static Material previewMaterial;    // 한번만 가져오기
    private bool lastCanPlace = false;

    private void OnValidate()
    {
        placementLayer = LayerMask.GetMask("Ground", "Construction");
        Debug.Log($"Placement Layer 설정됨: {placementLayer.value}");

        if (previewRenderer == null)
        {
            previewRenderer = gameObject.AddComponent<MeshRenderer>(); // 프리뷰 전용 메쉬 렌더러 추가
        }

    }

    private void Awake()
    {
        if (previewMaterial == null)
        {
            previewMaterial = Resources.Load<Material>("Materials/Preview_Mat");
        }

        meshRenderer = GetComponentInChildren<MeshRenderer>();
        previewCollider = GetComponent<Collider>();

        if (meshRenderer == null)
        {
            return;
        }

        if (previewCollider == null)
        {
            return;
        }

        originalMaterials = meshRenderer.sharedMaterials;
        if (originalMaterials == null || originalMaterials.Length == 0)
        {
            originalMaterials = new Material[] { meshRenderer.sharedMaterial };
        }
        previewCollider.enabled = false;     
    }


    private void Update()
    {
        FollowMouse();

    }
    private void FollowMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, placementLayer))
        {
            targetPosition = new Vector3(hit.point.x, hit.point.y, hit.point.z);
            targetPosition.y = targetPosition.y + previewCollider.bounds.size.y / 2;
            transform.position = targetPosition;
            // AdjustToGround();
            canPlace = true;
        }
        else
        {
            canPlace = false;
        }

        if (lastCanPlace != canPlace)
        {
            lastCanPlace = canPlace;
            UpdatePreviewColor();
        }
    }


    public void UpdatePreviewColor()
    {
        if (previewMaterials == null || previewMaterials.Length == 0)
            return;

        Color targetColor = canPlace ? Color.green : Color.red;

        // 모든 Material 색상 변경
        foreach (Material mat in previewMaterials)
        {
            mat.color = new Color(targetColor.r, targetColor.g, targetColor.b, 0.5f); // 반투명
        }
    }

    public void SetPreviewColor(bool hasResources)
    {
        if (previewMaterials == null || previewMaterials.Length == 0) return;

        Color targetColor = hasResources ? Color.green : Color.yellow;

        foreach (Material mat in previewMaterials)
        {
            mat.color = new Color(targetColor.r, targetColor.g, targetColor.b, 0.5f); // 반투명
        }
    }

    public bool CanPlace()
    {
        return canPlace;
    }


    // mat 바꿈
    public void SetPreviewMode()
    {
        if (meshRenderer == null)
        {
            Debug.LogError("meshRenderer가 null");
            return;
        }

        if (previewMaterial == null)
        {
            Debug.LogError("previewMaterial가 null");
            return;
        }

        // 기존 Material 개수만큼 새로운 프리뷰 Material
        previewMaterials = new Material[originalMaterials.Length];

        for (int i = 0; i < originalMaterials.Length; i++)
        {
            previewMaterials[i] = new Material(previewMaterial);
            previewMaterials[i].color = new Color(1, 1, 1, 0.5f); // 기본적으로 반투명 흰색
        }

        // 새 Material 적용
        meshRenderer.materials = previewMaterials;
    }




}