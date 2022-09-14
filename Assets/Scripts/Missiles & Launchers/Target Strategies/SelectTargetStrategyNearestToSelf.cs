using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Strategy of selecting the nearest target to this missile launcher
/// </summary>
public class SelectTargetStrategyNearestToSelf : CompositeFilterTargetsStrategy, ISelectTargetStrategy
{
	public SelectTargetStrategyNearestToSelf(MissileLauncher missileLauncher, List<IFilterTargetsStrategy> filterStrategies) : base(missileLauncher, filterStrategies) { }

	public SelectTargetStrategyNearestToSelf(MissileLauncher missileLauncher) : base(missileLauncher, new List<IFilterTargetsStrategy>()) { }

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
				return Vector3.Distance(minDistTarget.transform.position, launcherPos) < Vector3.Distance(nextTarget.transform.position, launcherPos) ? minDistTarget : nextTarget;
			});
		}
		catch (Exception)
		{
			return null;
		}
	}
}
