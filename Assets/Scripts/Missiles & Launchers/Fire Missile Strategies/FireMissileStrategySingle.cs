using UnityEngine;

/// <summary>
/// Strategy of firing a single missile 
/// </summary>
public class FireMissileStrategySingle : IFireMissileStrategy
{
	private MissileLauncher _missileLauncher;

	public FireMissileStrategySingle(MissileLauncher missileLauncher)
	{
		_missileLauncher = missileLauncher;
	}

	/// <summary>
	/// Fires the missile(s)
	/// </summary>
	/// <returns>Number of missiles fired</returns>
	public int Fire()
	{
		GameObject missileInstance = Object.Instantiate(_missileLauncher.MissilePrefab, _missileLauncher.transform.position, Quaternion.identity, _missileLauncher.transform.parent);
		missileInstance.GetComponent<Missile>().Launcher = _missileLauncher;
		missileInstance.GetComponent<Missile>().Target = new Missile.TargetInfo
		{
			Position = _missileLauncher.Target.transform.position,
			Velocity = _missileLauncher.Target.Velocity,
			Acceleration = _missileLauncher.Target.Acceleration
		};

		return 1;
	}
}
