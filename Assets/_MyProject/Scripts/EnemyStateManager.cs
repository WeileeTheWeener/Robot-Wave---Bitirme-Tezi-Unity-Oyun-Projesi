using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    public State currentState;
     
    void Update()
    {
        RunStateMachine(); //STATE MAKINESINI YURUT
    } 
    private void RunStateMachine()
    {
        //EGER CURRENT STATE NULL ISE NEXT STATE NULL OLUR DEGIL ISE RUNCURRENTSTATE CAGIRILIR
        State nextState = currentState?.RunCurrentState();

        if (nextState != null)
        {
            SwitchToTheNextState(nextState); // BIR SONRAKI DURUMA GEC 
            
        }
        currentState._Update(); //CURRENT STATEIN UPDATE FONKSIYONUNU CAGIR
    }
    private void SwitchToTheNextState(State nextState)
    {

        currentState = nextState;
    }
}
