//-----------------------------------------------------------------------
// <copyright file="edge.cs" company="Winterkewl Games LLC">
//     Copyright (c) Winterkewl Games LLC.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace KVoxel
{
	public class Edge
	{
		public int index;
		public Center d0, d1;  // Delaunay edge
		public Corner v0, v1;  // Voronoi edge
		public Vector3 midpoint;  // halfway between v0,v1
		public int river;  // volume of water, or 0
	}
}