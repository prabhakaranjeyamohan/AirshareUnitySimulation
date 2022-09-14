using UnityEngine;

/// <summary>
/// Strategy of firing burst of spread out missiles 
/// </summary>
public class FireMissileStrategyBurst : IFireMissileStrategy
{
	private MissileLauncher _missileLauncher;

    private int _missilesPerBurst;

	private float _burstSpread;

	public FireMissileStrategyBurst(MissileLauncher missileLauncher, int missilesPerBurst, float burstSpread)
	{
		_missileLauncher = missileLauncher;
		_missilesPerBurst = missilesPerBurst;
		_burstSpread = burstSpread;
	}

	/// <summary>
	/// Fires the missile(s)
	/// </summary>
	/// <returns>Number of missiles fired</returns>
	public int Fire()
	{
		int missilesToFire = Mathf.Min(_missilesPerBurst, _missileLauncher.MissilesAvailable);

		for (int i = 0; i < missilesToFire; i++)
		{
			Vector3 targetPosition = _missileLauncher.Target.transform.position + Random.insideUnitSphere * _burstSpread;

			GameObject missileInstance = Object.Instantiate(_missileLauncher.MissilePrefab, _missileLauncher.transform.position, Quaternion.identity, _missileLauncher.transform.parent);
			missileInstance.GetComponent<Missile>().Launcher = _missileLauncher;
			missileInstance.GetComponent<Missile>().Target = new Missile.TargetInfo
			{
				Position = targetPosition,
				Velocity = _missileLauncher.Target.Velocity,
				Acceleration = _missileLauncher.Target.Acceleration
			};
		}

		return missilesToFire;
	}
}
