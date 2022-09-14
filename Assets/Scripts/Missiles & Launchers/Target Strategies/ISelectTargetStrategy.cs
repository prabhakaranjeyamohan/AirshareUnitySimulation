/// <summary>
/// Abstraction for all missile launcher target selection strategies
/// </summary>
public interface ISelectTargetStrategy
{
	/// <summary>
	/// Selects single target based on criteria
	/// </summary>
	/// <returns>Selected target</returns>
	AI_Drone SelectTarget();
}
