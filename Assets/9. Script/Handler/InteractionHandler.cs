using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;

public interface IInteractableObject
{
	public void Interaction();
}


public class InteractionHandler : MonoBehaviour
{
	[Header("Core")]
	[SerializeField] private float interactionDistance;
	[SerializeField] private LayerMask interactionLayer;
	[SerializeField, Range(-1, 1)] private float xOffset = 0f;
	[SerializeField, Range(-1, 1)]private float yOffset = 0f;


	private IInteractableObject interactableObject;
	private InfoDisplayHandler displayObject;

    void Start()
    {
		InputManager.Instance.Interaction.action.started += InputInteraction;
		InvokeRepeating(nameof(FindObject), 0, 0.1f);
    }


    void FindObject()
    {
		Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width * (xOffset + 1f) / 2f, Screen.height * (yOffset + 1f) / 2f, 0));
		RaycastHit hit; 

		float dist = interactionDistance + (Camera.main.transform.position - transform.position).magnitude;

		if (Physics.Raycast(ray, out hit, dist))
		{
			var target = hit.collider.gameObject;
			if (target.TryGetComponent(out InfoDisplayHandler ido))
			{
				if (ido != displayObject)
				{
					ido.ShowInfo();
					displayObject = ido;
				} 
			}
			else 
			{ 
				displayObject = null;
			}

			if (target.TryGetComponent(out IInteractableObject io))
			{
				if (io != interactableObject)
					interactableObject = io; 
			}
			else
				interactableObject = null;
		}
		else
		{
			displayObject = null;
			interactableObject = null;
		}

		if (displayObject == null)
			UIManager.Instance.ObjectInfoUI.CloseUI();
	}
	


	void InputInteraction(InputAction.CallbackContext context)
	{
		if (interactableObject == null)
			return;

		interactableObject.Interaction();
	}

}
