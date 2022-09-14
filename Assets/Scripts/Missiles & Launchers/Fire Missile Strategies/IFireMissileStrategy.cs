/// <summary>
/// Abstraction for all missile launcher missile firing strategies
/// </summary>
public interface IFireMissileStrategy
{
	/// <summary>
	/// Fires the missile(s)
	/// </summary>
	/// <returns>Number of missiles fired</returns>
	int Fire();
}
