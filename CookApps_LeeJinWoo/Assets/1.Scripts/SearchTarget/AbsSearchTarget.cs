using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsSearchTarget : MonoBehaviour
{
    [SerializeField]
    protected BaseUnitData unitDataCs;

    [SerializeField]
    protected LayerMask layerMask = 0;   // 오버랩스피어 레이어 마스크 변수

}
