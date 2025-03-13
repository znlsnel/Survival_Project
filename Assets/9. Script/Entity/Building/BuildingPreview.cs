using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    [SerializeField] private Material validMaterial; // ��ġ ���� �� (�ʷϻ�)
    [SerializeField] private Material invalidMaterial;
    [SerializeField] private LayerMask placementLayer;

    private MeshRenderer meshRenderer;
    private Collider previewCollider;
    private bool canPlace = false;

    private Vector3 targetPosition;


    private void OnValidate()
    {
        placementLayer = LayerMask.GetMask("Ground", "Construction");
        Debug.Log($"Placement Layer ������: {placementLayer.value}");
    }

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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, placementLayer))
        {
            targetPosition = new Vector3(hit.point.x, transform.position.y, hit.point.z);
            AdjustToGround();
        }
    }

    private void AdjustToGround()
    {
        Vector3 centerPosition = previewCollider.bounds.center; // �Ǻ��� �߾ӿ� �����ʴ� ��찡 �̝���
        Ray groundRay = new Ray(centerPosition, Vector3.down);
        if (Physics.Raycast(groundRay, out RaycastHit groundHit, 5f, placementLayer))   // ���� ���̰� �ʹ� ������ �ٴ��� ��ã��..
        {
            targetPosition.y = groundHit.point.y;
            canPlace = true;
            Debug.Log("���� �ٴ��� ��������");

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
            meshRenderer.material = invalidMaterial; // �⺻������ ��ġ �Ұ� ����
        }
    }
}