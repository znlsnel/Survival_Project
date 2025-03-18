using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;

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
	[SerializeField] private Image aimUI;


	private IInteractableObject interactableObject;
	private InfoDisplayHandler displayObject;

    void Start()
    {
		
		aimUI.transform.localPosition = aimUI.transform.parent.InverseTransformPoint(new Vector3(Screen.width * (xOffset + 1f) / 2f, Screen.height * (yOffset + 1f) / 2f, 0));
		 
		InputManager.Interaction.started += InputInteraction;
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
		{
			GetComponent<PlayerUIHandler>().ObjectInfoUI.CloseUI();
		}


		if (displayObject != null || interactableObject != null)
			aimUI.color = new Color(0.3f, 1f, 0.3f, 1f);
		else
			aimUI.color = new Color(1f, 1f, 1f, 1f); 

	}



	void InputInteraction(InputAction.CallbackContext context)
	{
		if (interactableObject == null)
			return;

		interactableObject.Interaction();
	}

}
