using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPreview : MonoBehaviour
{
    [SerializeField] private LayerMask placementLayer;

    private MeshRenderer meshRenderer;
    private Collider previewCollider;
    private bool canPlace = false;

    private MeshRenderer previewRenderer;

    private Vector3 targetPosition;
    private Material[] originalMaterials;   // ���� ������ �ִ� ���׸���
    private Material[] previewMaterials;

    private static Material previewMaterial;    // �ѹ��� ��������

    private void OnValidate()
    {
        placementLayer = LayerMask.GetMask("Ground", "Construction");
        Debug.Log($"Placement Layer ������: {placementLayer.value}");

        if (previewRenderer == null)
        {
            previewRenderer = gameObject.AddComponent<MeshRenderer>(); // ������ ���� �޽� ������ �߰�
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
        if (previewMaterials == null || previewMaterials.Length == 0)
            return;

        Color targetColor = canPlace ? Color.green : Color.red;

        // ��� Material ���� ����
        foreach (Material mat in previewMaterials)
        {
            mat.color = new Color(targetColor.r, targetColor.g, targetColor.b, 0.5f); // ������
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


    // mat �ٲ�
    public void SetPreviewMode()
    {
        if (meshRenderer == null)
        {
            Debug.LogError("meshRenderer�� null");
            return;
        }

        if (previewMaterial == null)
        {
            Debug.LogError("previewMaterial�� null");
            return;
        }

        // ���� Material ������ŭ ���ο� ������ Material
        previewMaterials = new Material[originalMaterials.Length];

        for (int i = 0; i < originalMaterials.Length; i++)
        {
            previewMaterials[i] = new Material(previewMaterial);
            previewMaterials[i].color = new Color(1, 1, 1, 0.5f); // �⺻������ ������ ���
        }

        // �� Material ����
        meshRenderer.materials = previewMaterials;
    }



}