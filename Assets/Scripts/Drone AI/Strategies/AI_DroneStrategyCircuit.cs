using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Drone strategy of moving through a circuit of points
/// </summary>
public class AI_DroneStrategyCircuit : IAI_DroneStrategy
{
    private List<Vector3> _circuit;

    private int _lastPoint;

    private int _nextPoint;

    private float _velocity;

    private float _tau;

    private AI_Drone _drone;

    public AI_DroneStrategyCircuit(AI_Drone drone, List<Vector3> points, float velocity)
    {
        _drone = drone;
        _circuit = points;
        _velocity = velocity;

        _circuit.Add(drone.transform.position);
        _lastPoint = _circuit.Count - 1;
        _nextPoint = 0;

        _tau = 0.0f;
    }

    /// <summary>
    /// Updates object state
    /// </summary>
    public void Update()
    {
        if (_tau == 1.0f)
        {
            _lastPoint = _nextPoint;
            _nextPoint = _nextPoint == _circuit.Count - 1 ? 0 : _nextPoint + 1;
            _tau = 0.0f;
        }
    }

    /// <summary>
    /// Updates object physics
    /// </summary>
    public void FixedUpdate()
    {
        _drone.transform.position = Vector3.Lerp(_circuit[_lastPoint], _circuit[_nextPoint], _tau);

        _tau = Mathf.Min(_tau + _velocity / Vector3.Distance(_circuit[_lastPoint], _circuit[_nextPoint]) * Time.deltaTime, 1.0f);
    }

    /// <summary>
    /// Reacts to one of the drones getting disabled
    /// </summary>
    /// <param name="disabledDrone">Disabled drone object</param>
    public void ReactOnDroneDisabled(AI_Drone disabledDrone) { }
}
