using UnityEngine;
using UnityEngine.Events;
using Google.Maps.Event;
using Google.Maps.Scripts;

/// <summary>
/// Singleton central manager class, responsible for orchestrating main AAD components and scripts
/// </summary>
public class AADManager : MonoBehaviour
{
    private static AADManager _instance; // AADManager singleton instance
    public static AADManager Instance { get { return _instance; } }

    private AI_DroneManager _AI_DroneManager;
    public AI_DroneManager AI_DroneManager { get { return _AI_DroneManager; } }

    private MissileLauncherManager _MissileLauncherManager;
    public MissileLauncherManager MissileLauncherManager { get { return _MissileLauncherManager; } }

    private ProtectedZonesManager _ProtectedZonesManager;
    public ProtectedZonesManager ProtectedZonesManager { get { return _ProtectedZonesManager; } }

    private DynamicMapsService _DynamicMapsService;
    public DynamicMapsService DynamicMapsService { get { return _DynamicMapsService; } }

    private ElevationService _ElevationService;
    public ElevationService ElevationService { get { return _ElevationService; } }

    private IndicatorsCanvas _IndicatorsCanvas;
    public IndicatorsCanvas IndicatorsCanvas { get { return _IndicatorsCanvas; } }

    [field: SerializeField]
    public GameObject CMContainer { get; set; }


    /// <summary>
	/// Performs initial setup
	/// </summary>
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        _AI_DroneManager = FindObjectOfType<AI_DroneManager>();
        _MissileLauncherManager = FindObjectOfType<MissileLauncherManager>();
        _ProtectedZonesManager = FindObjectOfType<ProtectedZonesManager>();
        _DynamicMapsService = FindObjectOfType<DynamicMapsService>();
        _ElevationService = FindObjectOfType<ElevationService>();
        _IndicatorsCanvas = FindObjectOfType<IndicatorsCanvas>();

        UnityAction<MapLoadedArgs> mapLoadedAction = null;
        mapLoadedAction = new UnityAction<MapLoadedArgs>(delegate (MapLoadedArgs args)
        {
            StartActorManagers();
            DynamicMapsService.MapsService.Events.MapEvents.Loaded.RemoveListener(mapLoadedAction);
        });

        DynamicMapsService.MapsService.Events.MapEvents.Loaded.AddListener(mapLoadedAction);
    }

    /// <summary>
    /// Starts all actor managers scripts
    /// </summary>
    private void StartActorManagers()
    {
        ProtectedZonesManager.PostMapLoadStart();
        AI_DroneManager.PostMapLoadStart();
        MissileLauncherManager.PostMapLoadStart();
    }

}
