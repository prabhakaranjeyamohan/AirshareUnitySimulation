using UnityEngine;
using Google.Maps.Coord;
using IUX;

/// <summary>
/// Missile constructed through continious polling of the Ballistic Solutions Server
/// </summary>
public class BSSMissile : Missile
{
	private BSSHandler _BSSHandler;

	private Vector3 _startPosition;

	private Vector3 _lastPosition;

	private Vector3 _missilePositionENU;

	private Vector3 _interceptPositionENU;

	private float _timeToIntercept;

	private float _launchTime;

	private bool _interceptReached;

	private bool _replyAvailable;

	private bool _firstReply;

	/// <summary>
    /// Performs initial setup
    /// </summary
	protected override void Start()
	{
		base.Start();

		_timeToIntercept = 0.0f;
		_missilePositionENU = Vector3.zero;
		_interceptPositionENU = Vector3.zero;
		_launchTime = Time.time;
		_startPosition = _lastPosition = transform.position;
		_firstReply = true;

		LatLng reference = AADManager.Instance.DynamicMapsService.LatLng;

		Point3 basePositionECEF = CoordinateConversion.ENU_TO_ECEF
		(
			new Point3 { X = transform.position.x, Y = transform.position.z, Z = transform.position.y },
			reference.Lat,
			reference.Lng,
			CoordinateConversion.GEO_to_ECEF(new Point3 { X = reference.Lat, Y = reference.Lng, Z = 0 })
		);

		Point3 targetPositionECEF = CoordinateConversion.ENU_TO_ECEF
		(
			new Point3 { X = Target.Position.x, Y = Target.Position.z, Z = Target.Position.y },
			reference.Lat,
			reference.Lng,
			CoordinateConversion.GEO_to_ECEF(new Point3 { X = reference.Lat, Y = reference.Lng, Z = 0 })
		);
		Point3 targetVelocityECEF = CoordinateConversion.ENU_TO_ECEF(new Point3 { X = Target.Velocity.x, Y = Target.Velocity.z, Z = Target.Velocity.y }, reference.Lat, reference.Lng);
		Point3 targetAccelerationECEF = CoordinateConversion.ENU_TO_ECEF(new Point3 { X = Target.Acceleration.x, Y = Target.Acceleration.z, Z = Target.Acceleration.y }, reference.Lat, reference.Lng);

		_BSSHandler = new BSSHandler(new BSSRequest
		{
			BasePosition = basePositionECEF,
			TargetPosition = targetPositionECEF,
			TargetVelocity = targetVelocityECEF,
			TargetAcceleration = targetAccelerationECEF,
			GetInterceptENU = true,
			GetMissilePositionENU = true,
			MissileBlockConfig = Launcher.BlockConfig
		});

		_BSSHandler.SendRequest();
	}

	/// <summary>
	/// Updates object state
	/// </summary>
	private void Update()
	{
		if (_CMDeployStarted) return;

		lock (_BSSHandler.replyLock)
		{
			BSSReply reply = _BSSHandler.BSSReply;

			_replyAvailable = reply != null;

			if (_replyAvailable)
			{
				if (_firstReply)
				{
					_timeToIntercept = reply.TimeToIntercept;
					_interceptPositionENU = new Vector3((float)reply.InterceptENU.X, (float)reply.InterceptENU.Z, (float)reply.InterceptENU.Y);
					_firstReply = false;
				}
				_missilePositionENU = new Vector3((float)reply.MissilePositionENU.X, (float)reply.MissilePositionENU.Z, (float)reply.MissilePositionENU.Y);
			}
		}

		if (_replyAvailable)
		{

			Vector3 interceptPosition = _startPosition + _interceptPositionENU;
			DebugUtils.DrawCube(interceptPosition, 10.0f, Color.red, Time.deltaTime);

			transform.position = _startPosition + _missilePositionENU;

			if (!Mathf.Approximately(Vector3.Distance(transform.position, _lastPosition), 0.0f))
			{
				transform.rotation = Quaternion.LookRotation(transform.position - _lastPosition);
			}

			_lastPosition = transform.position;

			if (_interceptReached)
			{
				StartCoroutine(DeployCM());
				transform.GetComponent<MeshRenderer>().enabled = false;
				_BSSHandler.Close();
			}
		}
	}

	/// <summary>
	/// Updates object state at the end of the frame
	/// </summary>
	private void LateUpdate()
	{
		if (_CMDeployStarted || _firstReply) return;

		_BSSHandler.BSSRequest.FlightTime = Time.time - _launchTime;

		_BSSHandler.SendRequest();

		if (_replyAvailable && _timeToIntercept <= _BSSHandler.BSSRequest.FlightTime) _interceptReached = true;
	}

	/// <summary>
	/// Closes BSS handler on application quit
	/// </summary>
	private void OnApplicationQuit()
	{
		_BSSHandler.Close();
	}
}


