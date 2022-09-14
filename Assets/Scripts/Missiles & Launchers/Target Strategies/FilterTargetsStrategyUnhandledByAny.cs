using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Strategy of filtering out targets that have been handled by at least one of the launchers
/// </summary>
public class FilterTargetStrategyUnhandledByAny: CompositeFilterTargetsStrategy
{
	public FilterTargetStrategyUnhandledByAny(MissileLauncher missileLauncher, List<IFilterTargetsStrategy> filterStrategies) : base(missileLauncher, filterStrategies) { }

	public FilterTargetStrategyUnhandledByAny(MissileLauncher missileLauncher) : base(missileLauncher, new List<IFilterTargetsStrategy>()) { }

	/// <summary>
	/// Filters targets based on criteria
	/// </summary>
	/// <param name="targets">List of targets</param>
	/// <returns>Filtered list of targets</returns>
	public override List<AI_Drone> FilterTargets(List<AI_Drone> targets)
	{
		targets = base.FilterTargets(targets);

		return new List<AI_Drone>(targets.Where(target =>
		{
			foreach (var launcher in AADManager.Instance.MissileLauncherManager.MissileLaunchers)
			{
				if (launcher.HandledTargets.Contains(target)) return false;
			}
			return true;
		}));
	}
}
