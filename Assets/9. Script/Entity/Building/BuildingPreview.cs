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

        UpdatePreviewColor();
    }
    private void FollowMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, placementLayer))
        {
            targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            AdjustToGround();
        }
    }

    private void AdjustToGround()
    {
        Vector3 centerPosition = previewCollider.bounds.center; // 피봇이 중앙에 있지않는 경우가 이씅ㅁ
        Ray groundRay = new Ray(centerPosition, Vector3.down);
        if (Physics.Raycast(groundRay, out RaycastHit groundHit, 5f, placementLayer))   // 레이 길이가 너무 작으면 바닥을 못찾음..
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
}