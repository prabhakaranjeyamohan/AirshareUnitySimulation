using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Objective-oriented drone strategy involving approach tactics and counter measure evasion
/// </summary>
public class AI_DroneStrategyObjective : IAI_DroneStrategy
{
	private AI_Drone _drone;

	private Pathfinder _pathfinder;

	private SpatialGraph _graph;

	private Vector3 _objective;

	private List<Vector3> _track;

	private float _trackR;

	private int _nextPoint;

	private float _velocity;

	private float _tau;

	public AI_DroneStrategyObjective(AI_Drone drone, Pathfinder pathfinder, SpatialGraph graph, Vector3 objective, float velocity, float theta, float thetaShrinkFactor, float phi, float phiOffset, float r)
	{
		_track = new List<Vector3>();

		_drone = drone;
		_pathfinder = pathfinder;
		_graph = graph;
		_objective = objective;
		_velocity = velocity;
		_trackR = r;

		_nextPoint = 1;
		_tau = 0.0f;

		CreateTrack(theta, thetaShrinkFactor, phi, phiOffset, r);
	}

	/// <summary>
	/// Creates the objective-oriented drone track
	/// </summary>
	/// <param name="theta">XZ plane dispersion angle</param>
	/// <param name="thetaShrinkFactor">Shrink factor of theta</param>
	/// <param name="phi">Y axis dispersion angle upper bound</param>
	/// <param name="phiOffset">Y axis dispersion angle lower bound</param>
	/// <param name="r">Length of the advancement towards the objective with each track point</param>
	private void CreateTrack(float theta, float thetaShrinkFactor, float phi, float phiOffset, float r)
	{
		int trackLength = (int)((Vector3.Distance(_objective, _drone.transform.position) - r) / r);

		Vector3 lastPoint = _drone.transform.position;
		_track.Add(lastPoint);

		for (int i = 0; i < trackLength; i++)
		{
			Vector3 lastPointDir = lastPoint - _objective;

			Vector3 trackPoint = Quaternion.Euler(0.0f, Random.Range(-theta, theta), 0.0f) * Vector3.Normalize(new Vector3(lastPointDir.x, 0.0f, lastPointDir.z));
			float randomPhi = Random.Range(phiOffset, phi);
			trackPoint = _objective + new Vector3(trackPoint.x * Mathf.Cos(Mathf.Deg2Rad * randomPhi), Mathf.Sin(Mathf.Deg2Rad * randomPhi), trackPoint.z * Mathf.Cos(Mathf.Deg2Rad * randomPhi)) * (trackLength - i) * r;

			_track.Add(trackPoint);

			lastPoint = trackPoint;
			theta *= thetaShrinkFactor;
		}

		_track.Add(_objective);

		DebugUtils.DrawPath(_track, Color.red);
	}

	/// <summary>
	/// Updates object state
	/// </summary>
	public void Update()
	{
		if (_nextPoint == _track.Count) return;

		if (_tau == 1.0f)
		{
			_nextPoint++;
			_tau = 0.0f;
		}
	}

	/// <summary>
	/// Updates object physics
	/// </summary>
	public void FixedUpdate()
	{
		if (_nextPoint == _track.Count) return;

		_drone.transform.position = Vector3.Lerp(_track[_nextPoint - 1], _track[_nextPoint], _tau);

		_tau = Mathf.Min(_tau + _velocity / Vector3.Distance(_track[_nextPoint - 1], _track[_nextPoint]) * Time.deltaTime, 1.0f);
	}

	/// <summary>
	/// Reacts to one of the drones getting disabled by modifying the track to avoid countermeasures
	/// </summary>
	/// <param name="disabledDrone">Disabled drone object</param>
	public void ReactOnDroneDisabled(AI_Drone disabledDrone)
	{
		ModifyTrackAvoidCM();
	}

	/// <summary>
	/// Modifies the drone track by replacing the path segments potentially blocked by countermeasures with bypasses computed by pathfinder
	/// </summary>
	private void ModifyTrackAvoidCM()
	{

		bool[] pointsStatus = new bool[_track.Count - _nextPoint + 1];
		bool[] edgesStatus = new bool[pointsStatus.Length - 1];

		Vector3 lastPoint = _drone.transform.position;

		_track[_nextPoint - 1] = lastPoint;
		_tau = 0.0f;

		pointsStatus[0] = _graph.IsNodeFree(_graph.WorldToGraphPoint(lastPoint));

		for (int i = _nextPoint; i < _track.Count; i++)
		{
			pointsStatus[i - _nextPoint + 1] = _graph.IsNodeFree(_graph.WorldToGraphPoint(_track[i]));
			edgesStatus[i - _nextPoint] = pointsStatus[i - _nextPoint + 1] ?
				_graph.AreNodesFree(PartitionTrackEdge(lastPoint, _track[i]).Select(point => _graph.WorldToGraphPoint(point))) : false;

			lastPoint = _track[i];
		}

		lastPoint = _drone.transform.position;

		int shift = 0;

		for (int i = 0; i < pointsStatus.Length - 1; i++)
		{
			if (pointsStatus[i])
			{
				lastPoint = _track[_nextPoint + shift + i - (int)Mathf.Sign(Mathf.Abs(shift))];

				for (int j = i + 1; j < pointsStatus.Length; j++)
				{
					if (pointsStatus[j])
					{
						if ((j - i > 1 || !edgesStatus[i]))
						{
							List<Vector3> bypass = _pathfinder.FindPath(lastPoint, _track[_nextPoint + shift + j - 1]);

							bypass.RemoveAt(0);
							bypass.RemoveAt(bypass.Count - 1);

							DebugUtils.DrawPath(bypass, Color.blue);

							if (j - i > 1) _track.RemoveRange(_nextPoint + shift + i, j - 1 - i);
							shift -= j - 1 - i;

							_track.InsertRange(_nextPoint + shift + j - 1, bypass);
							shift += bypass.Count;

						}

						i = j - 1;
						break;
					}
				}
			}
		}
	}

	/// <summary>
	/// Partitions track edge into segments of length _trackR
	/// </summary>
	/// <param name="start">Edge start position, world space</param>
	/// <param name="end">Edge end position, world space</param>
	/// <returns>List of intermediary points</returns>
	private List<Vector3> PartitionTrackEdge(Vector3 start, Vector3 end)
	{
		List<Vector3> partition = new List<Vector3>();
		partition.Add(start);

		float segmentLen = Vector3.Distance(start, end);
		for (int i = 0; i < Mathf.FloorToInt(segmentLen / _trackR); i++)
		{
			float t = (i + 1) * _trackR / segmentLen;
			partition.Add(Vector3.Lerp(start, end, t));
		}
		partition.Add(end);

		return partition;
	}

}
