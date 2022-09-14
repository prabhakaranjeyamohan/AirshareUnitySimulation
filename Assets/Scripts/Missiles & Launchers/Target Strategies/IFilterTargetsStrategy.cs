using System.Collections.Generic;

/// <summary>
/// Abstraction for all target filtering strategies
/// </summary>
public interface IFilterTargetsStrategy
{
	/// <summary>
	/// Filters targets based on criteria
	/// </summary>
	/// <param name="targets">List of targets</param>
	/// <returns>Filtered list of targets</returns>
	List<AI_Drone> FilterTargets(List<AI_Drone> targets);
}
