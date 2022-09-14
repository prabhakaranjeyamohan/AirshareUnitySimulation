using UnityEngine;

/// <summary>
/// Simple drone strategy of moving in parametric circular trajectory
/// </summary>
public class AI_DroneStrategyCircle : IAI_DroneStrategy
{
    private float _radius;

    private float _velocity;

	private AI_Drone _drone;

    private Vector3 _origin;

    private float _tau;

	public AI_DroneStrategyCircle(AI_Drone drone, float radius, float velocity)
    {
        _drone = drone;
        _radius = radius;
        _velocity = velocity;

        _origin = drone.transform.position;
        _tau = 0.0f;
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
        _drone.transform.position = _origin + new Vector3(_radius * Mathf.Sin(_tau), 0.0f, _radius * Mathf.Cos(_tau) - _radius);

        _tau += _velocity * Time.deltaTime;
    }

    /// <summary>
    /// Reacts to one of the drones getting disabled
    /// </summary>
    /// <param name="disabledDrone">Disabled drone object</param>
    public void ReactOnDroneDisabled(AI_Drone disabledDrone) { }
}
