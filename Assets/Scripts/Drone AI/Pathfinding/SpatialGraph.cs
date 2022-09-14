using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Representation of world space segment in form of 3D grid of fixed size cells
/// </summary>
public class SpatialGraph
{
	private int _nodeSize;

	private int _width;  // X - W

	private int _height; // Y - H

	private int _depth;  // Z - D

	private Vector3 _center; // ( W / 2, 0, D / 2 )

	private bool[,,] _nodes; // false = free, true = blocked 
 
	public SpatialGraph(int nodeSize, int width, int height, int depth, Vector3 center)
	{
		_nodeSize = nodeSize;
		_width = width;
		_height = height;
		_depth = depth;
		_center = center;

		_nodes = new bool[_width, _height, _depth];
	}

	/// <summary>
	/// Converts world space coordinates to graph space coordinates
	/// </summary>
	/// <param name="worldPos">Point in world space</param>
	/// <returnsPoint in graph space</returns>
	public Vector3Int WorldToGraphPoint(Vector3 worldPos)
	{
		return new Vector3Int
		(
			Mathf.FloorToInt((worldPos.x - _center.x + _width * _nodeSize / 2) / _nodeSize),
			Mathf.FloorToInt((worldPos.y - _center.y) / _nodeSize),
			Mathf.FloorToInt((worldPos.z - _center.z + _depth * _nodeSize / 2) / _nodeSize)
		);
	}

	/// <summary>
	/// Converts graph space coordinates to world space coordinates
	/// </summary>
	/// <param name="nodePos">Point in graph space</param>
	/// <returns>Point in world space</returns>
	public Vector3 GraphToWorldPoint(Vector3Int nodePos)
	{
		return new Vector3
		(
			(nodePos.x + 0.5f) * _nodeSize + _center.x - _width * _nodeSize / 2,
			(nodePos.y + 0.5f) * _nodeSize + _center.y,
			(nodePos.z + 0.5f) * _nodeSize + _center.z - _depth * _nodeSize / 2
		);
	}

	/// <summary>
	/// Gets the list of node neighbors positions. Nodes one unit apart vertically, horizontally or diagonally are considered neighbors
	/// </summary>
	/// <param name="nodePos">Node position, graph space</param>
	/// <returns>List of node neighbors positions</returns>
	public List<Vector3Int> GetNeighbors(Vector3Int nodePos)
	{
		List<Vector3Int> neighbors = new List<Vector3Int>();

		for (int i = -1; i <= 1; i++)
		{
			for (int j = -1; j <= 1; j++)
			{
				for (int k = -1; k <= 1; k++)
				{
					if (i == 0 && j == 0 && k == 0) continue;

					Vector3Int nNodePos = new Vector3Int(nodePos.x + i, nodePos.y + j, nodePos.z + k);

					if (!InsideGraph(nNodePos)) continue;

					neighbors.Add(nNodePos);
				}
			}
		}
		
		return neighbors;
	}

	/// <summary>
	/// Places an obstacle in the graph by setting the occupied nodes' states to 'blocked'
	/// </summary>
	/// <param name="obstacleWorldPos">Obstacle position, world space</param>
	/// <param name="obstacleSize">Obstacle size, all axes</param>
	/// <param name="margin">Margin, all axes</param>
	/// <param name="descentMargin">Y axis downward margin</param>
	public void AddObstacle(Vector3 obstacleWorldPos, float obstacleSize, int margin, int descentMargin)
	{
		Vector3Int obstacleGraphPos = WorldToGraphPoint(obstacleWorldPos);

		int bound = Mathf.CeilToInt(obstacleSize / _nodeSize) + margin;

		for (int i = -bound; i <= bound; i++)
		{
			for (int j = -descentMargin - bound; j <= bound; j++)
			{
				for (int k = -bound; k <= bound; k++)
				{
					if (!InsideGraph(obstacleGraphPos + new Vector3Int(i, j, k))) continue;

					_nodes[obstacleGraphPos.x + i, obstacleGraphPos.y + j, obstacleGraphPos.z + k] = true;
				}
			}
		}

		DebugUtils.DrawCuboid
		(
			GraphToWorldPoint(obstacleGraphPos) + new Vector3(0.0f, -descentMargin * _nodeSize / 2.0f, 0.0f),
			(bound + 0.5f) * _nodeSize * 2,
			(bound + 0.5f + descentMargin / 2.0f) * _nodeSize * 2,
			(bound + 0.5f) * _nodeSize * 2,
			Color.yellow
		);
	}

	/// <summary>
	/// Determines whether the node located at specified position is free or not
	/// </summary>
	/// <param name="nodePos">Node position, graph space</param>
	/// <returns>True if node is free, false otherwise</returns>
	public bool IsNodeFree(Vector3Int nodePos) => !_nodes[nodePos.x, nodePos.y, nodePos.z];

	/// <summary>
	/// Determines whether all nodes located at specified positions are free or not
	/// </summary>
	/// <param name="nodesPos">Node positions, graph space</param>
	/// <returns>True if all nodes are free, false otherwise</returns>
	public bool AreNodesFree(IEnumerable<Vector3Int> nodesPos)
	{
		foreach (var nodePos in nodesPos)
		{
			if (!IsNodeFree(nodePos)) return false;
		}

		return true;
	}

	/// <summary>
	/// Determines whether the node located at specified position is within the bounds of the graph or not
	/// </summary>
	/// <param name="nodePos">Node position, world space</param>
	/// <returns>True if node is inside the graph, false otherwise</returns>
	private bool InsideGraph(Vector3Int nodePos)
	{
		return nodePos.x >= 0 && nodePos.x < _nodes.GetLength(0) &&
			   nodePos.y >= 0 && nodePos.y < _nodes.GetLength(1) &&
			   nodePos.z >= 0 && nodePos.z < _nodes.GetLength(2);
	}
}
