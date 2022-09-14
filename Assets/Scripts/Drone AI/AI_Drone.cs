using System;
using UnityEngine;

/// <summary>
/// AI drone agent script
/// </summary>
public class AI_Drone : Actor
{
    public IAI_DroneStrategy Strategy { get; set; }

    public event Action<AI_Drone> DroneDisabled;

    private Vector3 _velocity;
    public Vector3 Velocity { get { return _velocity; } }

    private Vector3 _acceleration;
    public Vector3 Acceleration { get { return _acceleration; } }

    private Vector3 _lastPos;

    private Vector3 _lastVelocity;

    /// <summary>
	/// Performs initial setup
	/// </summary>
    protected override void Start()
    {
        base.Start();

        GetComponent<TrailRenderer>().startColor = AADManager.Instance.IndicatorsCanvas.HostileColor;

        _lastPos = transform.position;
        _lastVelocity = new Vector3(0.0f, 0.0f, 0.0f);
    }

    /// <summary>
	/// Updates object state
	/// </summary>
    private void Update()
    {
        if(ActorState == ActorState.Active && Strategy != null) Strategy.Update();
    }

    /// <summary>
	/// Updates object physics
	/// </summary>
	private void FixedUpdate()
	{
        if (ActorState == ActorState.Active && Strategy != null) Strategy.FixedUpdate();

        _velocity = (transform.position - _lastPos) / Time.deltaTime;
        _acceleration = (Velocity - _lastVelocity) / Time.deltaTime;

        _lastVelocity = Velocity;
        _lastPos = transform.position;
    }

    /// <summary>
	/// Responds to entering the trigger
	/// </summary>
	/// <param name="collider">Entered trigger collider</param>
    public void OnTriggerEnter(Collider collider)
    {
        if (ActorState != ActorState.Active) return;

        if (collider.tag == "CMCloud")
        {
            if (UnityEngine.Random.Range(0.0f, 1.0f) < collider.GetComponent<CMCloud>().ColliderHitProb)
            {
                DisableDrone();
            }
        }
        else if(collider.tag == "CM" )
        {
            if (UnityEngine.Random.Range(0.0f, 1.0f) < collider.GetComponent<CounterMeasure>().ColliderHitProbs[(BoxCollider)collider])
            {
                DisableDrone();
            }
        }
    }

    /// <summary>
	/// Disables the drone
	/// </summary>
	private void DisableDrone()
	{
        ActorState = ActorState.Disabled;
        GetComponent<Rigidbody>().isKinematic = false;
        DroneDisabled.Invoke(this);
    }
}
