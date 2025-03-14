using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class JHN_PlayerManager : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    private JHN_PlayerState currentState;
    private JHN_PlayerState normalState;
    private JHN_PlayerState buildState;


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

    private void OnEnable()
    {
        EventManager.Instance.OnToggleBuildModeRequested += ToggleBuildingMode;
    }

    private void OnDisable()
    {
        EventManager.Instance.OnToggleBuildModeRequested -= ToggleBuildingMode;
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

    private void SetState(JHN_PlayerState newState)
    {
        currentState = newState;
        currentState.EnterState();
    }


}
