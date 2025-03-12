using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public abstract class JHN_PlayerState
{
    protected PlayerInput playerInput;

    public JHN_PlayerState(PlayerInput input)
    {
        playerInput = input;
    }
    public abstract void EnterState();
}

public class NormalState : JHN_PlayerState
{
    private JHN_PlayerManager playerManager;

    public NormalState(PlayerInput input, JHN_PlayerManager manager) : base(input)
    {
        playerManager = manager;
    }

    public override void EnterState()
    {
        playerInput.SwitchCurrentActionMap("PlayerInput");
        Debug.Log("�Ϲ� ��� Ȱ��ȭ");
    }
}

public class BuildState : JHN_PlayerState
{
    private JHN_PlayerManager playerManager;

    public BuildState(PlayerInput input, JHN_PlayerManager manager) : base(input)
    {
        playerManager = manager;
    }

    public override void EnterState()
    {
        playerInput.SwitchCurrentActionMap("PlayerBuilding");
        Debug.Log("���� ��� Ȱ��ȭ");
    }
}