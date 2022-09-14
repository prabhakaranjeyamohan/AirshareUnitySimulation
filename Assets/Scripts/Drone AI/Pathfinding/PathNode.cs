using UnityEngine;

/// <summary>
/// Class representing a path node, used by pathfinding algorithms
/// </summary>
public class PathNode
{
	public float Cost { get; set; }

	public bool Visited { get; set; }

	public bool HasPrev { get; set; }

	public Vector3Int Prev { get; set; }

	public PathNode()
	{
		Cost = float.MaxValue;
	}

	public PathNode(float cost)
	{
		Cost = cost;
	}
}

