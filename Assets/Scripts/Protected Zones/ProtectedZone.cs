using UnityEngine;

/// <summary>
/// Class representing a protected zone
/// </summary>
public class ProtectedZone : Actor
{
    public float Radius { get; set; }

    /// <summary>
    /// Performs initial setup
    /// </summary>
    protected override void Start()
    {
        base.Start();

        transform.localScale = new Vector3(Radius * 2, Radius * 2, Radius * 2);
        GetComponent<SphereCollider>().radius = Radius;
    }
}
