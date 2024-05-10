using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[System.Serializable]
public class TerrainTextureData
{
    public Texture2D terrainTexture;
    public Vector2 tileSize;
}

public class GenerateRandomHeights : MonoBehaviour
{
    
    private Terrain terrain;
    private TerrainData terrainData;
    
    [SerializeField]
    [Range(0f, 1f)]
    private float minRandomHeightRange = 0.0f; //min is 0
    
    [SerializeField]
    [Range(0f, 1f)]
    private float maxRandomHeightRange = 1.0f; //max is 1
    
    [SerializeField]
    private bool flattenTerrain = true;
    
    
    [Header("Perlin Noise Options")]
    [SerializeField]
    private bool perlinNoise = false;
    
    [SerializeField]
    private float perlinNoiseWidthScale = 0.01f;
    
    [SerializeField]
    private float perlinNoiseHeightScale = 0.01f;

    
    [Header("Texture Data")]
    [SerializeField]
    private List<TerrainTextureData> terrainTextureData;
    
    [SerializeField]
    private bool addTerrainTexture = false;
    
    // Start is called before the first frame update
    void Start()
    {
        if (terrain == null)
        {
            terrain = this.GetComponent<Terrain>();
        }
        if (terrainData == null)
        {
            terrainData = Terrain.activeTerrain.terrainData;
        }
        
        GenerateHeights();
        AddTerrainTextures();
    }

    void GenerateHeights()
    {
        float[,] heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];
        
        for (int width = 0; width < terrainData.heightmapResolution; width++)
        {
            for (int height = 0; height < terrainData.heightmapResolution; height++)
            {
                /*if (perlinNoise)
                {
                    heightMap[width, height] = Mathf.PerlinNoise(width * perlinNoiseWidthScale, height * perlinNoiseHeightScale);
                }
                else
                {
                    heightMap[width, height] = Random.Range(minRandomHeightRange, maxRandomHeightRange);
                }*/
                
                heightMap[width, height] = Random.Range(minRandomHeightRange, maxRandomHeightRange);
                heightMap[width, height] += Mathf.PerlinNoise(width * perlinNoiseWidthScale, height * perlinNoiseHeightScale);
            }
        }
        
        terrainData.SetHeights(0, 0, heightMap);
    }
    
    void FlattenTerrain()
    {
        float[,] heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];
        
        for (int width = 0; width < terrainData.heightmapResolution; width++)
        {
            for (int height = 0; height < terrainData.heightmapResolution; height++)
            {
                heightMap[width, height] = 0;
            }
        }
        
        terrainData.SetHeights(0, 0, heightMap);
    }

    private void AddTerrainTextures()
    {
        TerrainLayer[] terrainLayers = new TerrainLayer[terrainTextureData.Count];
        
        for (int i = 0; i < terrainTextureData.Count; i++)
        {
            if (addTerrainTexture)
            {
                terrainLayers[i] = new TerrainLayer();
                terrainLayers[i].diffuseTexture = terrainTextureData[i].terrainTexture;
                terrainLayers[i].tileSize = terrainTextureData[i].tileSize;
            }
            else
            {
                terrainLayers[i] = new TerrainLayer();
                terrainLayers[i].diffuseTexture = null;
            }
        }

        terrainData.terrainLayers = terrainLayers;
    }

    private void OnDestroy()
    {
        if (flattenTerrain)
        {
            FlattenTerrain();
        }
    }
}
