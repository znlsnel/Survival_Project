using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerModeHandler : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    private PlayerState currentState;
    private PlayerState normalState;
    private PlayerState buildState;

    private void OnValidate()
    {
        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }

        if (playerInput != null)
        {
            normalState = new NormalState(playerInput, this);
            buildState = new BuildState(playerInput, this);
        }
    }

    private void Start()
    {
        EventManager.Instance.OnToggleBuildModeRequested += ToggleBuildingMode;
    }

    public void ToggleBuildingMode()
    {

        bool isBuildingMode = currentState is NormalState;

        if (isBuildingMode)
        {
            SetState(buildState);
        }
        else
        {
            SetState(normalState);
        }
        EventManager.Instance.BuildingModeChanged(isBuildingMode);
    }

    private void SetState(PlayerState newState)
    {
        currentState = newState;
        currentState.EnterState();
    }


}
