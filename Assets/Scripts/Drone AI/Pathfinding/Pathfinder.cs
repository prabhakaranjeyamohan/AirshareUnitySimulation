using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract base class for pathfinding algorithms
/// </summary>
public abstract class Pathfinder
{
	protected SpatialGraph _graph;

	protected Dictionary<Vector3Int, PathNode> _seenNodes;

	protected List<Vector3> _pathNodes;

	public Pathfinder(SpatialGraph graph)
	{
		_graph = graph;

		_seenNodes = new Dictionary<Vector3Int, PathNode>();
		_pathNodes = new List<Vector3>();
	}

	/// <summary>
	/// Finds path between start and end positions consisting of world space positions of spatial graph nodes
	/// </summary>
	/// <param name="startWorldPos">Start position, world space</param>
	/// <param name="endWorldPos">End position, world space</param>
	/// <returns>List of path points positions, world space</returns>
	public abstract List<Vector3> FindPath(Vector3 startWorldPos, Vector3 endWorldPos);
}
