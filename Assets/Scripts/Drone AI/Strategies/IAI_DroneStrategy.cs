/// <summary>
/// Abstraction for all drone strategies
/// </summary>
public interface IAI_DroneStrategy
{
	/// <summary>
	/// Updates object state
	/// </summary>
	void Update();

	/// <summary>
	/// Updates object physics
	/// </summary>
	void FixedUpdate();

	/// <summary>
	/// Reacts to one of the drones getting disabled
	/// </summary>
	/// <param name="disabledDrone">Disabled drone object</param>
	void ReactOnDroneDisabled(AI_Drone disabledDrone);
}
