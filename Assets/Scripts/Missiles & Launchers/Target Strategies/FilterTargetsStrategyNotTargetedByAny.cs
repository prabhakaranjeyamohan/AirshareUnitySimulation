using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Strategy of filtering out targets that are currently targeted by at least one of the launchers
/// </summary>
public class FilterTargetsStrategyNotTargetedByAny : CompositeFilterTargetsStrategy
{
	public FilterTargetsStrategyNotTargetedByAny(MissileLauncher missileLauncher, List<IFilterTargetsStrategy> filterStrategies) : base(missileLauncher, filterStrategies) { }

	public FilterTargetsStrategyNotTargetedByAny(MissileLauncher missileLauncher) : base(missileLauncher, new List<IFilterTargetsStrategy>()) { }

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
				if(launcher.Target == target) return false;
			}
			return true;
		}));
	}
}
