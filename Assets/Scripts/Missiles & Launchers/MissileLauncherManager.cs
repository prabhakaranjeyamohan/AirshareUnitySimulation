using System;
using System.Collections.Generic;
using UnityEngine;
using Google.Maps.Coord;
using Google.Maps.Scripts;

/// <summary>
/// Central manager for missile launchers
/// </summary>
public class MissileLauncherManager : ActorManager
{
    /// <summary>
	/// Launcher transform configuration 
	/// </summary>
    [Serializable]
    public struct LauncherConfig
    {
        public LatLng positionLatLng;
        [Range(0, 359)]
        public float azimuthA;   // (deg)
        [Range(0, 90)]
        public float elevationA; // (deg)
    }

    /// <summary>
	/// Missile firing strategies
	/// </summary>
    public enum FireMissileStrategy { Single, Burst };

    /// <summary>
    /// Target selection strategies
    /// </summary>
    public enum SelectTargetStrategy { NearestToSelf, NearestToAnyProtectedZone, NearestToNearestProtectedZone, Fastest };

    /// <summary>
	/// Target filtering strategies
	/// </summary>
    public enum FilterTargetsStrategy { NotTargetedByAny, UnhandledBySelf, UnhandledByAny }

    [SerializeField]
    private string _BSS_IP = "192.168.30.27";
    public string BSS_IP { get { return _BSS_IP; } }

    [SerializeField]
    private int _BSS_PORT = 3290;
    public int BSS_PORT { get { return _BSS_PORT; } }

    [SerializeField]
    private FireMissileStrategy _fireMissileStrategySelection;

    [SerializeField]
    private SelectTargetStrategy _selectTargetStrategySelection;

    [SerializeField]
    private List<FilterTargetsStrategy> _filterTargetsStrategiesSelection;

    [SerializeField]
    private GameObject _launcherPrefab;

    [SerializeField]
    private List<LauncherConfig> _launcherConfigs;

    [SerializeField]
    private float _missileRange;

    [Header("Burst fire strategy parameters")]
    [SerializeField]
    private int _missilesPerBurst;

    [SerializeField]
    private int _burstSpread;

    private List<MissileLauncher> _missileLaunchers;
    public List<MissileLauncher> MissileLaunchers { get { return _missileLaunchers; } }

    private DynamicMapsService _dynamicMapsService;

    private ElevationService _elevationService;

    private List<AI_Drone> _targets;

    /// <summary>
    /// Performs initial setup
    /// </summary>
    private void Start()
	{
        _missileLaunchers = new List<MissileLauncher>();

        _dynamicMapsService = AADManager.Instance.DynamicMapsService;
        _elevationService = AADManager.Instance.ElevationService;
    }

    /// <summary>
    /// Updates object state
    /// </summary>
    private void Update()
	{
        if (!_postMapLoadStarted) return;

        _targets = AADManager.Instance.AI_DroneManager.ActiveDrones;

        AssignTargets();
	}

    /// <summary>
    /// Performs post map load setup
    /// </summary>
    public override void PostMapLoadStart()
    {
        if (!enabled) return;

        base.PostMapLoadStart();

        InstantiateMissileLaunchers();
    }

    /// <summary>
	/// Instantiates missile launchers
	/// </summary>
    private void InstantiateMissileLaunchers()
    {
        foreach (var launcherInfo in _launcherConfigs)
        {
            Vector3 position = _dynamicMapsService.MapsService.Projection.FromLatLngToVector3(launcherInfo.positionLatLng);
            position += new Vector3(0.0f, _elevationService.GetTerrainElevation(position) - _launcherPrefab.GetComponent<MissileLauncher>().GroundPoint.position.y, 0.0f);

            GameObject launcherInstance = Instantiate(_launcherPrefab, position, Quaternion.identity, transform);

            IFireMissileStrategy fireMissileStrategy = null;
            ISelectTargetStrategy selectTargetStrategy = null;
            List<IFilterTargetsStrategy> filterTargetsStrategies = new List<IFilterTargetsStrategy>();

            switch (_fireMissileStrategySelection)
            {
                case FireMissileStrategy.Single:
                    fireMissileStrategy = new FireMissileStrategySingle(launcherInstance.GetComponent<MissileLauncher>());
                    break;
                case FireMissileStrategy.Burst:
                    fireMissileStrategy = new FireMissileStrategyBurst(launcherInstance.GetComponent<MissileLauncher>(), _missilesPerBurst, _burstSpread);
                    break;
            }

            foreach (var filterTargetStrategySelection in _filterTargetsStrategiesSelection)
            {
                switch (filterTargetStrategySelection)
                {
                    case FilterTargetsStrategy.NotTargetedByAny:
                        filterTargetsStrategies.Add(new FilterTargetsStrategyNotTargetedByAny(launcherInstance.GetComponent<MissileLauncher>()));
                        break;
                    case FilterTargetsStrategy.UnhandledBySelf:
                        filterTargetsStrategies.Add(new FilterTargetsStrategyUnhandledBySelf(launcherInstance.GetComponent<MissileLauncher>()));
                        break;
                    case FilterTargetsStrategy.UnhandledByAny:
                        filterTargetsStrategies.Add(new FilterTargetStrategyUnhandledByAny(launcherInstance.GetComponent<MissileLauncher>()));
                        break;
                }
            }

            switch (_selectTargetStrategySelection)
            {
                case SelectTargetStrategy.NearestToSelf:
                    selectTargetStrategy = new SelectTargetStrategyNearestToSelf(launcherInstance.GetComponent<MissileLauncher>(), filterTargetsStrategies);
                    break;
                case SelectTargetStrategy.NearestToAnyProtectedZone:
                    selectTargetStrategy = new SelectTargetStrategyNearestToAnyProtectedZone(launcherInstance.GetComponent<MissileLauncher>(), filterTargetsStrategies);
                    break;
                case SelectTargetStrategy.NearestToNearestProtectedZone:
                    selectTargetStrategy = new SelectTargetStrategyNearestToNearestProtectedZone(launcherInstance.GetComponent<MissileLauncher>(), filterTargetsStrategies);
                    break;
                case SelectTargetStrategy.Fastest:
                    selectTargetStrategy = new SelectTargetStrategyFastest(launcherInstance.GetComponent<MissileLauncher>(), filterTargetsStrategies);
                    break;

            }

            launcherInstance.GetComponent<MissileLauncher>().AzimuthA = launcherInfo.azimuthA;
            launcherInstance.GetComponent<MissileLauncher>().ElevationA = launcherInfo.elevationA;
            launcherInstance.GetComponent<MissileLauncher>().FireMissileStrategy = fireMissileStrategy;
            launcherInstance.GetComponent<MissileLauncher>().SelectTargetStrategy = selectTargetStrategy;

            MissileLaunchers.Add(launcherInstance.GetComponent<MissileLauncher>());
        }
    }

    /// <summary>
	/// Assigns targets to missile launchers
	/// </summary>
    private void AssignTargets()
    {
        foreach (var launcher in MissileLaunchers)
        {
            List<AI_Drone> assignedTargets = _targets.FindAll(target => Vector3.Distance(target.transform.position, launcher.transform.position) <= _missileRange);

            launcher.AssignedTargets = assignedTargets;
        }
    }

}
