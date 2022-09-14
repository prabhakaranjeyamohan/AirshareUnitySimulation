using UnityEngine;

/// <summary>
/// Simple drone strategy of moving in uniformly accelerated motion
/// </summary>
public class AI_DroneStrategyUAM : IAI_DroneStrategy
{
	private Vector3 _velocity;

	private Vector3 _acceleration;

	private AI_Drone _drone;

	public AI_DroneStrategyUAM(AI_Drone drone, Vector3 initVelocity, Vector3 acceleration)
	{
		_drone = drone;
		_velocity = initVelocity;
		_acceleration = acceleration;
	}

	/// <summary>
	/// Updates object state
	/// </summary>
	public void Update() { }

	/// <summary>
	/// Updates object physics
	/// </summary>
	public void FixedUpdate()
	{
		_drone.transform.position += _velocity * Time.deltaTime;
		_velocity += _acceleration * Time.deltaTime;
	}

	/// <summary>
	/// Reacts to one of the drones getting disabled
	/// </summary>
	/// <param name="disabledDrone">Disabled drone object</param>
	public void ReactOnDroneDisabled(AI_Drone disabledDrone) { }
}
