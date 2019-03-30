using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TerrainGenerator : MonoBehaviour
{
    // Public Variables
    public int depth = 20;
    public int width = 256;
    public int lenght = 256;
    public float scale = 20f;
    public float offsetX = 100f;
    public float offsetY = 100f;

    public NavMeshSurface navMesh;

    //Private Variables

    void Start()
    {
        offsetX = Random.Range(0f, 500f);
        offsetY = Random.Range(0f, 500f);

        Terrain terrain = GetComponent<Terrain>();
        terrain.name = "Random Terain";
        terrain.terrainData = GenerateTerrain(terrain.terrainData);

        navMesh.BuildNavMesh();
    }

    TerrainData GenerateTerrain(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;

        terrainData.size = new Vector3(width, depth, lenght);

        terrainData.SetHeights(0, 0, GenerateHeights());
        return terrainData;
    }

    float[,] GenerateHeights()
    {
        float[,] heights = new float[width, lenght];
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < lenght; y++)
            {
                heights[x, y] = CalculateHeight(x, y);
            }
        }

        return heights;
    }

    float CalculateHeight(int x, int y)
    {
        float xCoord = (float)x / width * (scale / 1.3f) + offsetX;
        float yCoord = (float)y / lenght * (scale / 1.3f) + offsetY;
        float result = 0f;
        
        for (int i = 0; i < 5; i++)
        {
            if (result == 0f)
            {
                result = Mathf.PerlinNoise(Mathf.PerlinNoise(xCoord, yCoord), Mathf.PerlinNoise(xCoord, yCoord));
                continue;
            }

            result = Mathf.PerlinNoise(result + xCoord / 1.3f, yCoord) + Mathf.PerlinNoise(xCoord, result + yCoord / 1.3f) * 0.2f;
        }

        return result;
    }
}
