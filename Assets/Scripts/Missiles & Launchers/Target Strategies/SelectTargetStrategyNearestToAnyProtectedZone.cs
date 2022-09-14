using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Strategy of selecting the nearest target to any of the protected zones
/// </summary>
public class SelectTargetStrategyNearestToAnyProtectedZone : CompositeFilterTargetsStrategy, ISelectTargetStrategy
{
	public SelectTargetStrategyNearestToAnyProtectedZone(MissileLauncher missileLauncher, List<IFilterTargetsStrategy> filterStrategies) : base(missileLauncher, filterStrategies) { }

	public SelectTargetStrategyNearestToAnyProtectedZone(MissileLauncher missileLauncher) : base(missileLauncher, new List<IFilterTargetsStrategy>()) { }

	/// <summary>
	/// Selects single target based on criteria
	/// </summary>
	/// <returns>Selected target</returns>
	public AI_Drone SelectTarget()
	{
		List<AI_Drone> targets = FilterTargets(_missileLauncher.AssignedTargets);

		Vector3 launcherPos = _missileLauncher.transform.position;
		try
		{
			return targets.Aggregate((minDistTarget, nextTarget) =>
			{
				float minDistTargetDistance = AADManager.Instance.ProtectedZonesManager.ProtectedZones.Min(zone => Vector3.Distance(zone.transform.position, minDistTarget.transform.position));
				float nextTargetDistance = AADManager.Instance.ProtectedZonesManager.ProtectedZones.Min(zone => Vector3.Distance(zone.transform.position, nextTarget.transform.position));

				return minDistTargetDistance < nextTargetDistance ? minDistTarget : nextTarget;
			});
		}
		catch (Exception)
		{
			return null;
		}
	}
}
