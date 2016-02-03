//-----------------------------------------------------------------------
// <copyright file="center.cs" company="Winterkewl Games LLC">
//     Copyright (c) Winterkewl Games LLC.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;
using BenTools.Mathematics;
using BenTools.Data;

using ConvexHullSpiral;

namespace KVoxel
{
		public class Cell
		{
				public Vector site;
				public bool water;  // lake or ocean
				public bool ocean;  // ocean
				public bool coast;  // land polygon touching an ocean
				public bool border;  // at the edge of the map
				public bool BiomeOverride = false;
				public eBiome biome;  // biome type
				public double elevation;  // 0.0-1.0
				public double moisture;  // 0.0-1.0
				public double heat;
				public double max;
				// Probably be better to make these arrays and size them on creation.
				public List<Vector> neighborCells = new List<Vector> ();
				public List<VoronoiEdge> voronoiEdges = new List<VoronoiEdge> ();
				
				public Cell ()
				{
						Reset ();
				}

				public Cell (Vector Site)
				{
						site = Site;
						Reset ();
				}

				public void Reset ()
				{
						biome = eBiome.water;
						water = true;
						ocean = true;
						coast = false;
						border = false;
						elevation = 0;
						moisture = 0;
						heat = 0;
						neighborCells.Clear ();
						voronoiEdges.Clear ();
				}

				public void IsBorder ()
				{
						water = true;
						border = true;
						ocean = true;
						biome = eBiome.water;
				}

		}
}
