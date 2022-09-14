using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Strategy of selecting the nearest target to the nearest protected zone to this missile launcher
/// </summary>
public class SelectTargetStrategyNearestToNearestProtectedZone : CompositeFilterTargetsStrategy, ISelectTargetStrategy
{
	public SelectTargetStrategyNearestToNearestProtectedZone(MissileLauncher missileLauncher, List<IFilterTargetsStrategy> filterStrategies) : base(missileLauncher, filterStrategies) { }

	public SelectTargetStrategyNearestToNearestProtectedZone(MissileLauncher missileLauncher) : base(missileLauncher, new List<IFilterTargetsStrategy>()) { }

	/// <summary>
	/// Selects single target based on criteria
	/// </summary>
	/// <returns>Selected target</returns>
	public AI_Drone SelectTarget()
	{
		List<AI_Drone> targets = FilterTargets(_missileLauncher.AssignedTargets);

		Vector3 launcherPos = _missileLauncher.transform.position;

		Vector3 nearestProtectedZonePos = AADManager.Instance.ProtectedZonesManager.ProtectedZones.Aggregate((minDistZone, nextZone) =>
		{
			return Vector3.Distance(minDistZone.transform.position, launcherPos) < Vector3.Distance(nextZone.transform.position, launcherPos) ? minDistZone : nextZone;
		}).transform.position;

		try
		{
			return targets.Aggregate((minDistTarget, nextTarget) =>
			{
				return Vector3.Distance(minDistTarget.transform.position, nearestProtectedZonePos) < Vector3.Distance(nextTarget.transform.position, nearestProtectedZonePos) ? minDistTarget : nextTarget;
			});
		}
		catch (Exception)
		{
			return null;
		}
	}
}
