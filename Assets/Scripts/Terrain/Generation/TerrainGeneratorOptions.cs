using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// this class creates a scriptable object that stores the noise options
[CreateAssetMenu(fileName = "TerrainGeneratorOptions", menuName = "Terrain Generator Options")]
public class TerrainGeneratorOptions : ScriptableObject
{
    [Header("MESH OPTIONS")]
    public float isoLevel = 0.1f;

    [Header("DENSITY MAP OPTIONS")]
    public int numPointsPerAxis = 16;
    public int octaves = 8;
    public float spacing = 1;
    public float lacunarity = 2;
    public float persistence = 0.5f;
    public float noiseScale = 1f;
    public float noiseWeight = 1f;

    public static TerrainGeneratorOptions Init(
        float isoLevel = 0.1f,
        int numPointsPerAxis = 16,
        int octaves = 8,
        float spacing = 1.0f,
        float lacunarity = 2.0f,
        float persistence = 0.5f,
        float noiseScale = 1.0f,
        float noiseWeight = 1.0f
    ){
        TerrainGeneratorOptions options = ScriptableObject.CreateInstance<TerrainGeneratorOptions>();
        options.isoLevel = isoLevel;
        options.numPointsPerAxis = numPointsPerAxis;
        options.octaves = octaves;
        options.spacing = spacing;
        options.lacunarity = lacunarity;
        options.persistence = persistence;
        options.noiseScale = noiseScale;
        options.noiseWeight = noiseWeight;
        return options;
    }
}
