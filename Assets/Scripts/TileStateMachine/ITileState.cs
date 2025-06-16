using UnityEngine;

public interface ITileState
{
    // Called only once when the object enters the state
    void EnterState();

    // Called only once when the object exists the state
    void ExitState();
}
