using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class representing a countermeasure
/// </summary>
public class CounterMeasure : MonoBehaviour
{
    /// <summary>
	/// Struct containing distance to collider and corresponding hit probability
	/// </summary>
    [Serializable]
    public struct DistanceHitProb
    {
        public float distance;
        [Range(0, 1)]
        public float hitProb;
    }

    [SerializeField]
    private List<DistanceHitProb> _distanceHitProbs;
    public List<DistanceHitProb> DistanceHitProbs { get { return _distanceHitProbs; } }

    private Dictionary<BoxCollider, float> _colliderHitProbs;
    public Dictionary<BoxCollider, float> ColliderHitProbs { get { return _colliderHitProbs; } }

    /// <summary>
    /// Performs initial setup
    /// </summary>
    private void Start()
    {
        _colliderHitProbs = new Dictionary<BoxCollider, float>();
        AddDistanceTriggers();
    }

    /// <summary>
    /// Constructs triggers based on distance & hit probability info
    /// </summary>
    private void AddDistanceTriggers()
    {
        foreach (var distanceHitProb in DistanceHitProbs)
        {
            BoxCollider collider = gameObject.AddComponent<BoxCollider>();

            collider.isTrigger = true;
            collider.center = new Vector3(0.0f, 0.0f, 0.0f);

            collider.size = new Vector3(
                (distanceHitProb.distance + transform.lossyScale.x / 2) / (transform.lossyScale.x / 2),
                (distanceHitProb.distance + transform.lossyScale.y / 2) / (transform.lossyScale.y / 2),
                (distanceHitProb.distance + transform.lossyScale.z / 2) / (transform.lossyScale.z / 2)
            );

            ColliderHitProbs.Add(collider, distanceHitProb.hitProb);
        }
    }
}
