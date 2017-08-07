using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstType
{
    ObsType_NONE = 0,
    ObsType_TA = 1,
    ObsType_TB = 2,
    ObsType_TC = 3
}


public class ObstacleType : MonoBehaviour
{
    public ObstType obstacle_type = ObstType.ObsType_TA;

}
