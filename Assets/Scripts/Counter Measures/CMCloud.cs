using UnityEngine;

/// <summary>
/// Class representing a cloud of countermeasures
/// </summary>
public class CMCloud : Actor
{
    /// <summary>
	/// CM cloud simulation strategies
	/// </summary>
    public enum SimulationStrategy { Approximation, Precise }

    [SerializeField]
    private SimulationStrategy _simulationStrategy;

    [SerializeField]
    private float _cmNumber;
    public float CMNumber { get { return _cmNumber; } }

    [SerializeField]
    private float _radius;
    public float Radius { get { return _radius; } }

    [SerializeField]
    private float _descendVelocity;
    public float DescendVelocity { get { return _descendVelocity; } }

    [Range(0, 1)]
    [SerializeField]
    private float _colliderHitProb;
    public float ColliderHitProb { get { return _colliderHitProb; } }

    [SerializeField]
    private GameObject _CMPrefab;

    /// <summary>
	/// Performs initial setup
	/// </summary>
    protected override void Start()
    {
        base.Start();

        transform.localScale = new Vector3(Radius * 2, Radius * 2, Radius * 2);

        switch (_simulationStrategy)
        {
            case SimulationStrategy.Precise:
                InstantiateCMs();
                break;
            case SimulationStrategy.Approximation:
                GetComponent<MeshCollider>().enabled = true;
                break;
        }
    }

    /// <summary>
	/// Updates object state
	/// </summary>
    private void Update()
    {
        transform.position -= new Vector3(0.0f, DescendVelocity, 0.0f) * Time.deltaTime;
    }

    /// <summary>
	/// Instantiates countermeasures
	/// </summary>
    private void InstantiateCMs()
    {
        for (int i = 0; i < CMNumber; i++)
        {
            Vector3 offset = Random.insideUnitSphere * Radius;
            GameObject CMInstance = Instantiate(_CMPrefab, transform.position + offset, Quaternion.identity);
            CMInstance.transform.parent = transform;
        }
    }
}
