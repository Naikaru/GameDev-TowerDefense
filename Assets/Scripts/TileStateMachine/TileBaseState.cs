using UnityEngine;

public class TileBaseState : ITileState
{
    private TileStateHandler m_StateHandler;

    public TileBaseState(TileStateHandler stateHandler)
    {
        m_StateHandler = stateHandler;
    }

    public void EnterState()
    {

    }

    public void ExitState()
    {

    }
}
