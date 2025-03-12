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
    public NormalState(PlayerInput input) : base(input) { }
    public override void EnterState()
    {
        Debug.Log("�Ϲ� ��� Ȱ��ȭ");
    }
}


public class BuildState : JHN_PlayerState
{
    public BuildState(PlayerInput input) : base(input) { }
    public override void EnterState()
    {
        Debug.Log("���� ��� Ȱ��ȭ");
    }
}
