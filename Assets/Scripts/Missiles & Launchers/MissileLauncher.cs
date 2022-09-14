using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Maps.Coord;
using IUX;

/// <summary>
/// Missile launcher script
/// </summary>
public class MissileLauncher : Actor
{
    [SerializeField]
    private int _missilesNum;

    [SerializeField]
    private float _turnRate;

    [SerializeField]
    private float _reloadTime;

    [SerializeField]
    private float _fireDelay;

    [SerializeField]
    private bool _resetHandledTargets;

    [SerializeField]
    private float _handledTargetsResetTime;

    [SerializeField]
    private GameObject _missilePrefab;
    public GameObject MissilePrefab { get { return _missilePrefab; } }

    [SerializeField]
    private BlockConfig _blockConfig;
    public BlockConfig BlockConfig { get { return _blockConfig; } }

    [SerializeField]
    private Transform _turret;

    [SerializeField]
    private Transform _groundPoint; 
    public Transform GroundPoint { get { return _groundPoint; } }

    public IFireMissileStrategy FireMissileStrategy { get; set; }

    public ISelectTargetStrategy SelectTargetStrategy { get; set; }

    public List<AI_Drone> AssignedTargets { get; set; }

    private List<AI_Drone> _handledTargets;
    public List<AI_Drone> HandledTargets { get { return _handledTargets; } }

    private AI_Drone _target; 
    public AI_Drone Target { get { return _target; } }

    public float AzimuthA { get; set; }

    public float ElevationA { get; set; }

    private BSSHandler _BSSHandler;

    private int _missilesAvailable;
    public int MissilesAvailable { get { return _missilesAvailable; } }

    private LatLng _reference;

    private bool _canFire;

    /// <summary>
    /// Performs initial setup
    /// </summary>
    protected override void Start()
    {
        base.Start();

        _handledTargets = new List<AI_Drone>();
        _missilesAvailable = _missilesNum;
        _canFire = true;
        _reference = AADManager.Instance.DynamicMapsService.LatLng;

        Point3 basePositionECEF = CoordinateConversion.ENU_TO_ECEF
        (
            new Point3 { X = transform.position.x, Y = transform.position.z, Z = transform.position.y },
            _reference.Lat,
            _reference.Lng,
            CoordinateConversion.GEO_to_ECEF(new Point3 { X = _reference.Lat, Y = _reference.Lng, Z = 0 })
        );

        _BSSHandler = new BSSHandler(new BSSRequest
        {
            BasePosition = basePositionECEF,
            GetLaunchAngles = true,
            MissileBlockConfig = BlockConfig
        });

        if (_resetHandledTargets) StartCoroutine(ResetHandledTargets());
    }

    /// <summary>
    /// Updates object state
    /// </summary>
    private void Update()
	{
        if (MissilesAvailable == 0) return;

        if (Target == null || Target.ActorState == ActorState.Disabled) {
            _target = SelectTargetStrategy.SelectTarget();
            return;
        }

        float launchAzimuthA;
        float launchElevationA;

        lock (_BSSHandler.replyLock)
        {
            BSSReply reply = _BSSHandler.BSSReply;

            if (reply == null) return;

            launchAzimuthA = reply.LaunchAzimuth;
            launchElevationA = reply.LaunchElevation;
        }

        Quaternion launchRotation = Quaternion.Euler(-launchElevationA, launchAzimuthA, 0.0f);

        _turret.transform.rotation = Quaternion.RotateTowards(_turret.transform.rotation, launchRotation, _turnRate * Time.deltaTime);

        if (_turret.transform.rotation == launchRotation && _canFire)
        {
            int missilesFired = FireMissileStrategy.Fire();
            HandledTargets.Add(Target);
            _target = null;
            _missilesAvailable -= missilesFired;
            if (MissilesAvailable == 0) StartCoroutine(Reload());
            if (_fireDelay > 0.0f) StartCoroutine(FireDelay());
        }
	}

    /// <summary>
    /// Updates object state at the end of the frame
    /// </summary>
    private void LateUpdate()
	{
        if (Target == null || MissilesAvailable == 0) return;

        Point3 targetPositionECEF = CoordinateConversion.ENU_TO_ECEF
        (
            new Point3 { X = Target.transform.position.x, Y = Target.transform.position.z, Z = Target.transform.position.y },
            _reference.Lat,
            _reference.Lng,
            CoordinateConversion.GEO_to_ECEF(new Point3 { X = _reference.Lat, Y = _reference.Lng, Z = 0 })
        );
        Point3 targetVelocityECEF = CoordinateConversion.ENU_TO_ECEF(new Point3 { X = Target.Velocity.x, Y = Target.Velocity.z, Z = Target.Velocity.y }, _reference.Lat, _reference.Lng);
        Point3 targetAccelerationECEF = CoordinateConversion.ENU_TO_ECEF(new Point3 { X = Target.Acceleration.x, Y = Target.Acceleration.z, Z = Target.Acceleration.y }, _reference.Lat, _reference.Lng);

        _BSSHandler.BSSRequest.TargetPosition = targetPositionECEF;
        _BSSHandler.BSSRequest.TargetVelocity = targetVelocityECEF;
        _BSSHandler.BSSRequest.TargetAcceleration = targetAccelerationECEF;

        _BSSHandler.SendRequest();
    }

    /// <summary>
	/// Coroutine for reloading
	/// </summary>
    private IEnumerator Reload()
    {
        yield return new WaitForSeconds(_reloadTime);
        _missilesAvailable = _missilesNum;
    }
    
    /// <summary>
	/// Coroutine for fire delay
	/// </summary>
    private IEnumerator FireDelay()
    {
        _canFire = false;
        yield return new WaitForSeconds(_fireDelay);
        _canFire = true;
    }

    /// <summary>
    /// Coroutine for reseting handled targets
    /// </summary>
    private IEnumerator ResetHandledTargets()
    {
        yield return new WaitForSeconds(_handledTargetsResetTime);
        HandledTargets.Clear();
        StartCoroutine(ResetHandledTargets());
    }

    /// <summary>
	/// Closes BSS handler on application quit
	/// </summary>
	private void OnApplicationQuit()
	{
        _BSSHandler.Close();
	}
}
