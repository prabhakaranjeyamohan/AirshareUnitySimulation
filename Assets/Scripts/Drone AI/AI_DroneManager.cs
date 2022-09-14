using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Google.Maps.Coord;
using Google.Maps.Scripts;

/// <summary>
/// Central AI drones manager. Responsible for swarm creation and inter-communication
/// </summary>
public class AI_DroneManager : ActorManager
{
    /// <summary>
	/// Drone strategies
	/// </summary>
    public enum Strategy { UAM, Circle, Circuit, Objective }

    [SerializeField]
    private Strategy _strategySelection;

    [Header ("Main parameters")]
    [SerializeField]
    private GameObject _dronePrefab;

    [SerializeField]
    private int _dronesNum;

    [SerializeField]
    private LatLng _startLatLng;

    [SerializeField]
    private float _startElevation;

    [SerializeField]
    private bool _isRelativeElevation;

    [SerializeField]
    private float _spread;

    [Header("Spatial graph parameters")]
    [SerializeField]
    private int _graphNodeSize;

    [SerializeField]
    private int _graphWidth;

    [SerializeField]
    private int _graphHeight;

    [SerializeField]
    private int _graphDepth;

    [SerializeField]
    private int _graphObstacleSize;

    [SerializeField]
    private int _graphObstacleMargin;

    [SerializeField]
    private int _graphObstacleDescentMargin;

    [Header("Uniformly Accelerated Motion strategy parameters")]
    [SerializeField]
    private Vector3 _UAMVelocity;

    [SerializeField]
    private Vector3 _UAMAcceleration;

    [Header("Circle strategy parameters")]
    [SerializeField]
    private float _circleRadius;

    [SerializeField]
    private float _circleVelocity;

    [Header("Circuit strategy parameters")]
    [SerializeField]
    private List<LatLng> _circuitPointsLatLang;

    [SerializeField]
    private float _circuitVelocity;

    [Header("Objective strategy parameters")]

    [SerializeField]
    private float _objectiveTrackVelocity;

    [SerializeField]
    private float _objectiveTrackTheta;

    [SerializeField]
    private float _objectiveTrackThetaShrinkFactor;

    [SerializeField]
    private float _objectiveTrackPhi;

    [SerializeField]
    private float _objectiveTrackPhiOffset;

    [SerializeField]
    private float _objectiveTrackR;

    private DynamicMapsService _dynamicMapsService;

    private ElevationService _elevationService;

    private List<AI_Drone> _activeDrones;
    public List<AI_Drone> ActiveDrones { get { return _activeDrones; } }

    private List<GameObject> _objectives;

    private SpatialGraph _spatialGraph;

    /// <summary>
	/// Performs initial setup
	/// </summary>
    private void Start()
    {
        _activeDrones = new List<AI_Drone>();

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

        _objectives = AADManager.Instance.ProtectedZonesManager.ProtectedZones;

        SetupSpatialGraph();
        InstantiateDrones();
	}

    /// <summary>
	/// Sets up spatial graph according to the objectives' positions
	/// </summary>
	private void SetupSpatialGraph()
	{
        Vector3 graphCenter = _objectives.Aggregate(new Vector3(0.0f, 0.0f, 0.0f), (sum, objective) => sum + objective.transform.position) / _objectives.Count;
        graphCenter.y = _objectives.Min(objective => objective.transform.position.y);

        _spatialGraph = new SpatialGraph(_graphNodeSize, _graphWidth, _graphHeight, _graphDepth, graphCenter);
    }

    /// <summary>
	/// Creates drone swarm
	/// </summary>
    private void InstantiateDrones()
    {
        Vector3 swarmCenter = _dynamicMapsService.MapsService.Projection.FromLatLngToVector3(_startLatLng);
        swarmCenter += new Vector3(0.0f, _isRelativeElevation ? _startElevation + _elevationService.GetTerrainElevation(swarmCenter) : _startElevation, 0.0f);

        for (int i = 0; i < _dronesNum; i++)
		{
            Vector3 offset = Random.insideUnitSphere * _spread;
            Vector3 instPosition = swarmCenter + offset;

            GameObject droneInstance = Instantiate(_dronePrefab, instPosition, Quaternion.identity, transform);

            IAI_DroneStrategy strategy = null;
			switch (_strategySelection)
			{
                case Strategy.UAM:
                    strategy = new AI_DroneStrategyUAM(droneInstance.GetComponent<AI_Drone>(), _UAMVelocity, _UAMAcceleration);
                    break;
                case Strategy.Circle:
                    strategy = new AI_DroneStrategyCircle(droneInstance.GetComponent<AI_Drone>(), _circleRadius, _circleVelocity);
                    break;
                case Strategy.Circuit:
                    List<Vector3> circuitPoints = new List<Vector3>(_circuitPointsLatLang.Select(x => _dynamicMapsService.MapsService.Projection.FromLatLngToVector3(x) + new Vector3(offset.x, offset.y + swarmCenter.y, offset.z)));
                    strategy = new AI_DroneStrategyCircuit(droneInstance.GetComponent<AI_Drone>(), circuitPoints, _circuitVelocity);
                    break;
                case Strategy.Objective:
                    int assignedObjective = Random.Range(0, _objectives.Count);
                    strategy = new AI_DroneStrategyObjective(droneInstance.GetComponent<AI_Drone>(), new AStarPathfinder(_spatialGraph), _spatialGraph, _objectives[assignedObjective].transform.position,
                        _objectiveTrackVelocity, _objectiveTrackTheta, _objectiveTrackThetaShrinkFactor, _objectiveTrackPhi, _objectiveTrackPhiOffset, _objectiveTrackR);
                    break;
            }

            droneInstance.GetComponent<AI_Drone>().Strategy = strategy;
            droneInstance.GetComponent<AI_Drone>().DroneDisabled += OnDroneDisabled;

            ActiveDrones.Add(droneInstance.GetComponent<AI_Drone>());
        }
    }

    /// <summary>
	/// Reacts to one of the drones getting disabled by updating the spatial graph and notifying other drones
	/// </summary>
	/// <param name="disabledDrone">Disabled drone object</param>
    private void OnDroneDisabled(AI_Drone disabledDrone)
    {
        ActiveDrones.Remove(disabledDrone);

        _spatialGraph.AddObstacle(disabledDrone.transform.position, _graphObstacleSize, _graphObstacleMargin, _graphObstacleDescentMargin);

        foreach (var activeDrone in ActiveDrones)
        {
            activeDrone.Strategy.ReactOnDroneDisabled(disabledDrone);
        }
    }
}