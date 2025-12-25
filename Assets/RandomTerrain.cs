using Unity.Mathematics;
using UnityEngine;

public class RandomTerrain : MonoBehaviour
{
    public TerrainData terrainData;
    public float scale = 100;
    public float height = 0.1f;

    public void Start()
    {
        int resolution = terrainData.heightmapResolution;
        float [,] heights = new float[resolution, resolution];
        for (int x = 0; x < resolution; x++)
        {
            for (int y = 0; y < resolution; y++)
            {
                heights[y, x] = noise.pnoise(new float2(x, y) / scale, new float2(1, 1) * 1000f) * height;
            }
        }
        terrainData.SetHeights(0, 0, heights);
    }
}
