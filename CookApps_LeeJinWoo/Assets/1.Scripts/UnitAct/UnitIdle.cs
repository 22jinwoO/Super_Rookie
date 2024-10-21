using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitIdle : MonoBehaviour, IUnitActState
{

    [SerializeField]
    private IUnitController contorllerCs;

    [SerializeField]
    private ISearchTarget searchTargetCs;


    public void Enter()
    {
        contorllerCs = GetComponent<IUnitController>();
        searchTargetCs = GetComponent<ISearchTarget>();
        searchTargetCs.ActState = this;
    }

    public void DoAction()
    {
        searchTargetCs.SearchTarget();
    }

    public void Exit()
    {
        contorllerCs.UnitAct = null;
        contorllerCs.Action = UnitState.Tracking;
    }

}
