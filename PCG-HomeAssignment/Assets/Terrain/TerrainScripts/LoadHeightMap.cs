using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class LoadHeightMap : MonoBehaviour
{
    private Terrain terrain;

    private TerrainData terrainData;
    
    [SerializeField]
    private Texture2D heightMapImage;
    
    [SerializeField]
    private Vector3 heightMapScale = new Vector3(1, 1, 1);

    [Header("Play Mode Options")]
    [SerializeField]
    private bool loadHeightMap = true;
    
    [SerializeField]
    private bool flattenTerrainOnExit = true;

    [Header("Edit Mode Options")]
    [SerializeField] 
    private bool flattenTerrainInEditMode = false;
    
    [SerializeField]
    private bool loadHeightMapInEditMode = false;
    
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
        
        if (loadHeightMap)
        {
            LoadHeightMapImage();
        }
    }
    
    void LoadHeightMapImage()
    {
        float[,] heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution];

        for (int width = 0; width < terrainData.heightmapResolution; width++)
        {
            for (int height = 0; height < terrainData.heightmapResolution; height++)
            {
                heightMap[width, height] =
                    heightMapImage.GetPixel((int)(width * heightMapScale.x), (int)(height * heightMapScale.x))
                        .grayscale * heightMapScale.y;
            }
        }
        
        terrainData.SetHeights(0,0, heightMap);
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
        
        terrainData.SetHeights(0,0, heightMap);
    }

    //Called in edit mode only
    void OnValidate()
    {
        if (terrain == null)
        {
            terrain = this.GetComponent<Terrain>();
        }

        if (terrainData == null)
        {
            terrainData = Terrain.activeTerrain.terrainData;
        }
        
        if (flattenTerrainInEditMode)
        {
            FlattenTerrain();
        } 
        else if (loadHeightMapInEditMode)
        {
            LoadHeightMapImage();
        }
    }

    void OnDestroy()
    {
        if (flattenTerrainOnExit)
        {
            FlattenTerrain();
        }
    }
}
