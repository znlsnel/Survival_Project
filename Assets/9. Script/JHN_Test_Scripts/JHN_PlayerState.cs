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
    private PlayerModeHandler playerManager;

    public NormalState(PlayerInput input, PlayerModeHandler manager) : base(input)
    {
        playerManager = manager;
    }

    public override void EnterState()
    { 
        InputManager.ModeChange(true);
        Debug.Log("일반 모드 활성화");
    }
}

public class BuildState : JHN_PlayerState
{
    private PlayerModeHandler playerManager;

    public BuildState(PlayerInput input, PlayerModeHandler manager) : base(input)
    {
        playerManager = manager;
    }

    public override void EnterState()
    { 
		InputManager.ModeChange(false);
        Debug.Log("건축 모드 활성화");
    }
}