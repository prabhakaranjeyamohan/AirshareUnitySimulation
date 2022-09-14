using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Helper class for drawing debug shapes
/// </summary>
public static class DebugUtils
{
	/// <summary>
	/// Draws a cuboid of specified dimensions by connecting every pair of verices with lines
	/// </summary>
	/// <param name="center">Cuboid center, world space</param>
	/// <param name="sizeX">Cuboid width, X axis</param>
	/// <param name="sizeY">Cuboid height, Y axis</param>
	/// <param name="sizeZ">cuboid length, Z axis</param>
	/// <param name="color">Cuboid color</param>
	/// <param name="time">Cuboid lifespan (seconds displayed on the screen)</param>
	public static void DrawCuboid(Vector3 center, float sizeX, float sizeY, float sizeZ, Color color, float time = 1000.0f)
	{
		for (int i1 = -1; i1 <= 1; i1 += 2) for (int i2 = -1; i2 <= 1; i2 += 2) for (int i3 = -1; i3 <= 1; i3 += 2)
		for (int i4 = -1; i4 <= 1; i4 += 2) for (int i5 = -1; i5 <= 1; i5 += 2) for (int i6 = -1; i6 <= 1; i6 += 2)
		{
			Debug.DrawLine(center + new Vector3(sizeX * i1 / 2, sizeY * i2 / 2, sizeZ * i3 / 2), center + new Vector3(sizeX * i4 / 2, sizeY * i5 / 2, sizeZ * i6 / 2), color, time);
		}
	}

	/// <summary>
	/// Draws a cube of specified size by connecting every pair of verices with lines
	/// </summary>
	/// <param name="center">Cube center, world space</param>
	/// <param name="size">Cube size (all axes)</param>
	/// <param name="color">Cube color</param>
	/// <param name="time">Cuboid lifespan (seconds displayed on the screen</param>
	public static void DrawCube(Vector3 center, float size, Color color, float time = 1000.0f)
	{
		DrawCuboid(center, size, size, size, color, time);
	}

	/// <summary>
	/// Draws a path connecting every sequential pair of points with lines
	/// </summary>
	/// <param name="points">List of points positions, world space</param>
	/// <param name="color">Path color</param>
	/// <param name="time">Path lifespan (seconds displayed on the screen</param>
	public static void DrawPath(List<Vector3> points, Color color, float time = 1000.0f)
	{
		for (int i = 0; i < points.Count - 1; i++)
		{
			Debug.DrawLine(points[i], points[i + 1], color, time);
		}
	}

}
