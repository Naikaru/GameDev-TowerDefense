using UnityEngine;

public class TileHoverState : ITileState
{
    private TileStateHandler m_StateHandler;

    public TileHoverState(TileStateHandler stateHandler)
    {
        m_StateHandler = stateHandler;
    }
    
    public void EnterState()
    {
        m_StateHandler.EnableOutline();
    }

    public void ExitState()
    {
        m_StateHandler.DisableOutline();
    }
}
