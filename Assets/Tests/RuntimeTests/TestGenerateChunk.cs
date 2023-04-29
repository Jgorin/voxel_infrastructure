using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.IO;
using UnityEditor;
using UnityEngine.Assertions;

[TestFixture]
public class TestGenerateChunk
{
    [Test]
    public void TestGenerateDensityMap()
    {
        // TerrainGeneratorOptions options = TerrainGeneratorOptions.Init();
        // TerrainChunk chunk = TerrainChunk.Instantiate(Vector3Int.zero, options);

        // ComputeBuffer densityMap = new ComputeBuffer(options.numPointsPerAxis * options.numPointsPerAxis * options.numPointsPerAxis, sizeof(float) * 4);
        // Vector4[] densityMapArray = new Vector4[options.numPointsPerAxis * options.numPointsPerAxis * options.numPointsPerAxis];

        // // generate density map
        // chunk.GenerateDensityMap(densityMap);

        // // read density map from GPU
        // densityMap.GetData(densityMapArray);

        // // release buffer
        // densityMap.Release();

        // Debug.Log(chunk.transform.position);
        // Debug.Log((options.numPointsPerAxis - 1) * options.spacing);

        // // print the 8 corners of the density map
        // Debug.Log("Density map corners:");
        // Debug.Log(densityMapArray[0]);
        // Debug.Log(densityMapArray[options.numPointsPerAxis - 1]);
        // Debug.Log(densityMapArray[options.numPointsPerAxis * options.numPointsPerAxis - 1]);
        // Debug.Log(densityMapArray[options.numPointsPerAxis * options.numPointsPerAxis - options.numPointsPerAxis]);
        // Debug.Log(densityMapArray[options.numPointsPerAxis * options.numPointsPerAxis * options.numPointsPerAxis - 1]);
        // Debug.Log(densityMapArray[options.numPointsPerAxis * options.numPointsPerAxis * options.numPointsPerAxis - options.numPointsPerAxis]);
        // Debug.Log(densityMapArray[options.numPointsPerAxis * options.numPointsPerAxis * options.numPointsPerAxis - options.numPointsPerAxis * options.numPointsPerAxis + 1]);
        // Debug.Log(densityMapArray[options.numPointsPerAxis * options.numPointsPerAxis * options.numPointsPerAxis - options.numPointsPerAxis * options.numPointsPerAxis + options.numPointsPerAxis - 1]);
    }
}
