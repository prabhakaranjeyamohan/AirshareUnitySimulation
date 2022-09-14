using System;
using System.Collections.Generic;
using UnityEngine;
using Google.Maps.Scripts;
using Google.Maps.Coord;

/// <summary>
/// Central manger class for protected zones
/// </summary>
public class ProtectedZonesManager : ActorManager
{
    /// <summary>
    /// Launcher transform configuration 
    /// </summary>
    [Serializable]
    public struct ProtectedZoneInfo
    {
        public LatLng positionLatLng;

        public float radius; 
    }

    [SerializeField]
    private GameObject _protectedZonePrefab;

    [SerializeField]
    private List<ProtectedZoneInfo> _protectedZonesInfo;

    private List<GameObject> _protectedZones;
    public List<GameObject> ProtectedZones { get { return _protectedZones; } }

    private DynamicMapsService _dynamicMapsService;

    private ElevationService _elevationService;

    /// <summary>
    /// Performs initial setup
    /// </summary>
    private void Start()
    {
        _protectedZones = new List<GameObject>();

        _dynamicMapsService = AADManager.Instance.DynamicMapsService;
        _elevationService = AADManager.Instance.ElevationService;
    }

    /// <summary>
	/// Performs post map load setup
	/// </summary>
    public override void PostMapLoadStart()
    {
        if (!enabled) return;

        base.PostMapLoadStart();

        InstantiateProtectedZones();
    }

    /// <summary>
	/// Instantiates protected zones 
	/// </summary>
    private void InstantiateProtectedZones()
    {
        foreach (var protectedZoneInfo in _protectedZonesInfo)
        {
            Vector3 position = _dynamicMapsService.MapsService.Projection.FromLatLngToVector3(protectedZoneInfo.positionLatLng);
            position += new Vector3(0.0f, _elevationService.GetTerrainElevation(position), 0.0f);

            GameObject protectedZoneInstance = Instantiate(_protectedZonePrefab, position, Quaternion.identity);
            protectedZoneInstance.transform.parent = transform;

            protectedZoneInstance.GetComponent<ProtectedZone>().Radius = protectedZoneInfo.radius;

            ProtectedZones.Add(protectedZoneInstance);
        }
    }
}
