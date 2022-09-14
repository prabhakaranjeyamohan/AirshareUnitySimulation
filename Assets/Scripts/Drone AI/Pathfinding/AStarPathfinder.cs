using System.Collections.Generic;
using UnityEngine;
using Priority_Queue;

/// <summary>
/// Zombie-key based implementation of A* pathfinding algorithm
/// </summary>
public class AStarPathfinder : Pathfinder
{
	private static int _pqMaxNodes = 100000;

	enum HeuristicType { Chebyshev, Euclidean };

	public AStarPathfinder(SpatialGraph graph) : base(graph) { }

	/// <summary>
	/// Finds path between start and end positions consisting of world space positions of spatial graph nodes
	/// </summary>
	/// <param name="startWorldPos">Start position, world space</param>
	/// <param name="endWorldPos">End position, world space</param>
	/// <returns>List of path points positions, world space</returns>
	public override List<Vector3> FindPath(Vector3 startWorldPos, Vector3 endWorldPos)
	{
		_seenNodes.Clear();
		_pathNodes.Clear();

		Vector3Int startNodePos = _graph.WorldToGraphPoint(startWorldPos);
		Vector3Int endNodePos = _graph.WorldToGraphPoint(endWorldPos);

		_seenNodes.Add(startNodePos, new PathNode(0.0f));
		_seenNodes.Add(endNodePos, new PathNode());

		FastPriorityQueue<PriorityPathNode> priorityQueue = new FastPriorityQueue<PriorityPathNode>(_pqMaxNodes);

		priorityQueue.Enqueue(new PriorityPathNode(startNodePos), 0);

		while (priorityQueue.Count > 0)
		{
			PriorityPathNode lowest = priorityQueue.Dequeue();

			_seenNodes[lowest.PathNodePos].Visited = true;

			if (lowest.PathNodePos == endNodePos) break;

			List<Vector3Int> neighbors = _graph.GetNeighbors(lowest.PathNodePos);

			foreach (var neighborPos in neighbors)
			{
				if (!_seenNodes.ContainsKey(neighborPos)) _seenNodes.Add(neighborPos, new PathNode());

				if (!_graph.IsNodeFree(neighborPos)) continue;

				float nodeCost = _seenNodes[lowest.PathNodePos].Cost + 1;

				if (_seenNodes[neighborPos].Cost > nodeCost)
				{
					_seenNodes[neighborPos].Cost = nodeCost;
					_seenNodes[neighborPos].HasPrev = true;
					_seenNodes[neighborPos].Prev = lowest.PathNodePos;

					priorityQueue.Enqueue(new PriorityPathNode(neighborPos), nodeCost + heuristic(neighborPos, endNodePos));
				}
			}
		}

		Vector3Int currentNodePos = endNodePos;
		_pathNodes.Add(_graph.GraphToWorldPoint(currentNodePos));

		while (_seenNodes[currentNodePos].HasPrev)
		{
			_pathNodes.Add(_graph.GraphToWorldPoint(_seenNodes[currentNodePos].Prev));
			currentNodePos = _seenNodes[currentNodePos].Prev;
		}

		_pathNodes.Reverse();

		return _pathNodes;
	}

	/// <summary>
	/// Heuristic for approximating distance between nodes
	/// </summary>
	/// <param name="node1Pos">First node position, spatial graph coordinates</param>
	/// <param name="node2Pos">Second node position, spatial graph coordinates</param>
	/// <param name="method">Heuristic method</param>
	/// <returns>distance between nodes</returns>
	private float heuristic(Vector3Int node1Pos, Vector3Int node2Pos, HeuristicType method = HeuristicType.Euclidean)
	{
		switch (method)
		{
			case HeuristicType.Chebyshev:
				return Mathf.Max(
					Mathf.Abs(node1Pos.x - node2Pos.x),
					Mathf.Abs(node1Pos.y - node2Pos.y),
					Mathf.Abs(node1Pos.z - node2Pos.z)
				);
			case HeuristicType.Euclidean:
			default:
				return Mathf.Sqrt(
					Mathf.Pow(node1Pos.x - node2Pos.x, 2) +
					Mathf.Pow(node1Pos.y - node2Pos.y, 2) +
					Mathf.Pow(node1Pos.z - node2Pos.z, 2)
				);
		}
	}
}
