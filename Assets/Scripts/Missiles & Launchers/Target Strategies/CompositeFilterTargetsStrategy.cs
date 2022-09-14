using System.Collections.Generic;

/// <summary>
/// Composite strategy containing several other target filtering strategies
/// </summary>
public abstract class CompositeFilterTargetsStrategy : IFilterTargetsStrategy
{
	protected List<IFilterTargetsStrategy> _filterStrategies;

	protected MissileLauncher _missileLauncher;

	public CompositeFilterTargetsStrategy(MissileLauncher missileLauncher, List<IFilterTargetsStrategy> filterStrategies)
	{
		_missileLauncher = missileLauncher;
		_filterStrategies = filterStrategies;
	}

	public CompositeFilterTargetsStrategy(MissileLauncher missileLauncher) : this(missileLauncher, new List<IFilterTargetsStrategy>()) { }

	/// <summary>
	/// Filters targets based on criteria
	/// </summary>
	/// <param name="targets">List of targets</param>
	/// <returns>Filtered list of targets</returns>
	public virtual List<AI_Drone> FilterTargets(List<AI_Drone> targets)
	{
		foreach (var filterStrategy in _filterStrategies)
		{
			targets = filterStrategy.FilterTargets(targets);
		}
		return targets;
	}
}
