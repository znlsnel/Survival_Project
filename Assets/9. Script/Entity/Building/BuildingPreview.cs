using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    [SerializeField] private Material validMaterial; // 배치 가능 시 (초록색)
    [SerializeField] private Material invalidMaterial;
    [SerializeField] private LayerMask placementLayer;

    private MeshRenderer meshRenderer;
    private Collider previewCollider;
    private bool canPlace = false;

    private Vector3 targetPosition;

    private void OnValidate()
    {
        placementLayer = LayerMask.GetMask("Ground", "Construction");
        Debug.Log($"Placement Layer 설정됨: {placementLayer.value}");
    }

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
        AdjustToGround();
        UpdatePreviewColor();
    }
    private void FollowMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, placementLayer))
        {
            targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
        }
    }

    private void AdjustToGround()
    {
        Ray groundRay = new Ray(targetPosition + Vector3.up, Vector3.down);
        if (Physics.Raycast(groundRay, out RaycastHit groundHit, 2f, placementLayer))
        {
            targetPosition.y = groundHit.point.y;
            canPlace = true;
            Debug.Log("제발 바닥을 감지해줘");

        }
        else canPlace = false;

        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);
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

    public void SetPreviewMode()
    {

        if (meshRenderer != null)
        {
            meshRenderer.material = invalidMaterial; // 기본적으로 배치 불가 색상
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return; // 실행 중일 때만 표시

        // 1. 카메라에서 마우스 위치로 쏘는 레이 (파란색)
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(mouseRay.origin, mouseRay.origin + mouseRay.direction * 10f);

        // 2. 프리뷰 오브젝트에서 바닥으로 쏘는 레이 (빨간색)
        Vector3 groundRayStart = targetPosition + Vector3.up * 1f;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundRayStart, groundRayStart + Vector3.down * 2f);
    }
}