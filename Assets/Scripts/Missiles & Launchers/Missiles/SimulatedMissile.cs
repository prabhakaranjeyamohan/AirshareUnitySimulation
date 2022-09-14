using System.Collections;
using UnityEngine;

/// <summary>
/// Simple Unity simulated missile
/// </summary>
public class SimulatedMissile : Missile
{
    [field: SerializeField]
    public float Thrust { get; set; }     // (N)

    [field: SerializeField]
    public float BurnTime { get; set; }

    private bool _burnFinished;

    /// <summary>
    /// Performs initial setup
    /// </summary>
    protected override void Start()
    {
        base.Start();

        _burnFinished = false;

        StartCoroutine(EngineBurn());

        transform.rotation = Quaternion.LookRotation(Target.Position - transform.position);
    }

    /// <summary>
    /// Updates object physics
    /// </summary>
    private void FixedUpdate()
	{
        if (_CMDeployStarted) return;

        GetComponent<Rigidbody>().AddForce(transform.rotation * Vector3.forward * Thrust);
    }

    /// <summary>
    /// Updates object state
    /// </summary>
    private void Update()
    {
        if (_CMDeployStarted) return;

        if (_burnFinished)
        {
            ActorState = ActorState.Disabled;
            StartCoroutine(DeployCM());
            return;
        }
    }

    /// <summary>
	/// Coroutine for engine burn
	/// </summary>
    private IEnumerator EngineBurn()
    {
        yield return new WaitForSeconds(BurnTime);
        _burnFinished = true;
    }

}
