using UnityEngine;
using UnityEngine.InputSystem;

public class JHN_PlayerManager : MonoBehaviour
{
    public static JHN_PlayerManager Instance { get; private set; }

    [SerializeField] private PlayerInput playerInput; // 에디터에서 자동 할당

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
            normalState = new NormalState(playerInput);
            buildState = new BuildState(playerInput);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        currentState = buildState ?? new NormalState(playerInput);
    }

    public void ToggleBuildingMode()
    {
        if (currentState is NormalState)
        {
            SetState(buildState);
        }
        else
        {
            SetState(normalState);
        }
    }

    private void SetState(JHN_PlayerState newState)
    {
        currentState = newState;
        currentState.EnterState();
    }
}
