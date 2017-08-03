using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MapArea
{
    NONE = 0,
    AREA_A = 1,
    AREA_B = 2,
    AREA_C = 3,
    AREA_D = 4,
    AREA_E = 5,
    AREA_F = 6,
    AREA_G = 7,
    AREA_H = 8,
    AREA_I = 9,
    AREA_J = 10,
    AREA_K = 11,
    AREA_L = 12,
    AREA_M = 13,
    AREA_N = 14,
    AREA_MAX = 15
}


public class DropMapArea : MonoBehaviour
{
    public MapArea mapArea = MapArea.NONE;
}
