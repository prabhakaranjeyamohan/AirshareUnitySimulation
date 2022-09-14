using Priority_Queue;
using UnityEngine;

/// <summary>
/// Wrapper class for path node, used in priority queue
/// </summary>
public class PriorityPathNode : FastPriorityQueueNode
{
	public Vector3Int PathNodePos { get; set; }

	public PriorityPathNode(Vector3Int pathNodePos)
	{
		PathNodePos = pathNodePos;
	}
}
