using System;
using UnityEngine;

/// <summary>
/// Script for computing elevation at specified 2D world space coordinates based on terrain
/// </summary>
public class ElevationService : MonoBehaviour
{
    public float GetTerrainElevation(Vector3 point)
    {
        Terrain[] terrainTiles = GetComponentsInChildren<Terrain>();

        Terrain closestTile = null;
        float minDistance = float.MaxValue;

        foreach (var tile in terrainTiles)
        {
            Vector2 tileCenter = new Vector2(tile.transform.position.x + tile.terrainData.size.x / 2, tile.transform.position.z + tile.terrainData.size.z / 2);

            float distance = Vector2.Distance(tileCenter, new Vector2(point.x, point.z));

            if (distance < minDistance)
            {
                closestTile = tile;
                minDistance = distance;
            }
        }

        if (closestTile == null) throw new Exception("Absent terrain. Cannot get elevation.");

        return closestTile.SampleHeight(point) + closestTile.GetPosition().y;
    }
}
