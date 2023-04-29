using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class creates a scriptable object that stores the noise options
[CreateAssetMenu(fileName = "TerrainRendererOptions", menuName = "Terrain Renderer Options")]
public class TerrainRendererOptions : ScriptableObject
{
    public int renderDistance = 16;
}
