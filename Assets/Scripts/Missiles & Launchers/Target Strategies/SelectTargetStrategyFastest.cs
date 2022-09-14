using System;
using System.Linq;
using System.Collections.Generic;

/// <summary>
/// Strategy of selecting the fastest target
/// </summary>
public class SelectTargetStrategyFastest : CompositeFilterTargetsStrategy, ISelectTargetStrategy
{
	public SelectTargetStrategyFastest(MissileLauncher missileLauncher, List<IFilterTargetsStrategy> filterStrategies) : base(missileLauncher, filterStrategies) { }

	public SelectTargetStrategyFastest(MissileLauncher missileLauncher) : base(missileLauncher, new List<IFilterTargetsStrategy>()) { }

	/// <summary>
	/// Selects single target based on criteria
	/// </summary>
	/// <returns>Selected target</returns>
	public AI_Drone SelectTarget()
	{
		List<AI_Drone> targets = FilterTargets(_missileLauncher.AssignedTargets);

		try
		{
			return targets.Aggregate((fastestTarget, nextTarget) => fastestTarget.Velocity.magnitude > nextTarget.Velocity.magnitude ? fastestTarget : nextTarget );
		}
		catch (Exception)
		{
			return null;
		}
	}
}
