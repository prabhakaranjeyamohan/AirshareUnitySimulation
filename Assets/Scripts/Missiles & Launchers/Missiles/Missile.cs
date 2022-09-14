using UnityEngine;
using System.Collections;

/// <summary>
/// Base class for all missiles
/// </summary>
public class Missile : Actor
{
	[SerializeField]
	private float _CMDeployTime;
	public float CMDeployTime { get { return _CMDeployTime; } }

	[SerializeField]
	private GameObject _CMCloudPrefab;

	protected bool _CMDeployStarted;

	public MissileLauncher Launcher { get; set; }

	/// <summary>
	/// Struct containing main target info required for BSS
	/// </summary>
	public struct TargetInfo
	{
		public Vector3 Position { get; set; }

		public Vector3 Velocity { get; set; }

		public Vector3 Acceleration { get; set; }
	}

	public TargetInfo Target { get; set; }

	/// <summary>
	/// Performs initial setup
	/// </summary>
	protected override void Start()
	{
		base.Start();

		_CMDeployStarted = false;

		GetComponent<TrailRenderer>().startColor = AADManager.Instance.IndicatorsCanvas.FriendlyColor;
	}

	/// <summary>
	/// Coroutine for deploying the countermeasures
	/// </summary>
	public IEnumerator DeployCM()
	{
		_CMDeployStarted = true;
		yield return new WaitForSeconds(CMDeployTime);
		Instantiate(_CMCloudPrefab, transform.position, Quaternion.identity, AADManager.Instance.CMContainer.transform);
	}
}
