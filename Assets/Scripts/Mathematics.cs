using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Mathematics
{
    public static float SquareDistance(Vector3 a, Vector3 b){
        return (a.x - b.x) * (a.x - b.x) + (a.y - b.y) * (a.y - b.y) + (a.z - b.z) * (a.z - b.z);
    }
}
