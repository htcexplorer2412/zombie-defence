using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
     
    public EnemyState currentState;
    public FieldOfView fov;
    void Update()
    {
        RunCurrentState();
    }

    private void RunCurrentState()
    {
        EnemyState nextState = currentState?.RunCurrentState();

        if(nextState != null)
        {
            SwitchToNextState(nextState);
        }

    }

    private void SwitchToNextState(EnemyState nextState)
    {
        currentState = nextState;
    }
}
