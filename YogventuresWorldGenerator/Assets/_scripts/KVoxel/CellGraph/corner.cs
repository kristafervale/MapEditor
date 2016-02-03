//-----------------------------------------------------------------------
// <copyright file="corner.cs" company="Winterkewl Games LLC">
//     Copyright (c) Winterkewl Games LLC.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace KVoxel
{
	public class Corner
	{
		public int index;
		
		public Vector3 point;  // location
		public bool ocean;  // ocean
		public bool water;  // lake or ocean
		public bool coast;  // touches ocean and land polygons
		public bool border;  // at the edge of the map
		public double elevation;  // 0.0-1.0
		public double moisture;  // 0.0-1.0
		
		public List<Center> touches;
		public List<Edge> protrudes;
		public List<Corner> adjacent;
		
		public int river;  // 0 if no river, or volume of water in river
		public Corner downslope;  // pointer to adjacent corner most downhill
		public Corner watershed;  // pointer to coastal corner, or null
		public int watershed_size;
	}
}