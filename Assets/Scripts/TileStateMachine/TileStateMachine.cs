using System;
using UnityEngine;

[Serializable]
public class TileStateMachine
{
    public ITileState CurrentState {  get; private set; }

    public TileBaseState baseState;
    public TileHoverState hoverState;

    public event Action<ITileState> stateChanged;

    public TileStateMachine(TileStateHandler stateHandler)
    {
        this.baseState = new TileBaseState(stateHandler);
        this.hoverState = new TileHoverState(stateHandler);
    }

    public void Init(ITileState state)
    {
        // Enter new state
        state.EnterState();
        // Update Current State
        CurrentState = state;

        // Call state changed event
        stateChanged?.Invoke(state);
    }

    public void TransitionTo(ITileState nextState)
    {
        // Exit current state
        CurrentState.ExitState();
        // Enter new state
        nextState.EnterState();
        // Update Current State
        CurrentState = nextState;

        // Call state changed event
        stateChanged?.Invoke(nextState);
    }
}
