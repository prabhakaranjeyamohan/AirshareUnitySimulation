using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Strategy of filtering out targets that have been handled by this launcher
/// </summary>
public class FilterTargetsStrategyUnhandledBySelf: CompositeFilterTargetsStrategy
{
	public FilterTargetsStrategyUnhandledBySelf(MissileLauncher missileLauncher, List<IFilterTargetsStrategy> filterStrategies) : base(missileLauncher, filterStrategies) { }

	public FilterTargetsStrategyUnhandledBySelf(MissileLauncher missileLauncher) : base(missileLauncher, new List<IFilterTargetsStrategy>()) { }

	/// <summary>
	/// Filters targets based on criteria
	/// </summary>
	/// <param name="targets">List of targets</param>
	/// <returns>Filtered list of targets</returns>
	public override List<AI_Drone> FilterTargets(List<AI_Drone> targets)
	{
		targets = base.FilterTargets(targets);

		return new List<AI_Drone>(targets.Where(target => !_missileLauncher.HandledTargets.Contains(target)));
	}
}
