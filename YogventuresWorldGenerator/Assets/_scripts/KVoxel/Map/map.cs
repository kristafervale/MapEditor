//-----------------------------------------------------------------------
// <copyright file="map.cs" company="Winterkewl Games LLC">
//     Copyright (c) Winterkewl Games LLC.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

using BenTools.Mathematics;
using LibNoise;
using LibNoise.Unity.Generator;
using LibNoise.Unity.Operator;

namespace KVoxel
{
	#region constructors
		public class Map
		{
				public Map ()
				{
						r = new System.Random ();
//						InfinityLength = Max / 2;
//						VOrigin = Vector3.zero;
//						MaxVect = new Vector3 (Max, 0, Max);
//						MaxXVect = new Vector3 (Max, 0, 0);
//						MaxYVect = new Vector3 (0, 0, Max);
				}
	#endregion

	#region Measurments
				public double Max = 10.0; // Maximum world size. Default map size is 2000 game units positive units in each direction.
				private double Min = 0.0; // Minimum MUST always stay positive normally will be zero
				private double BorderCellMin = 50.0;
				private double BorderCellMax = 50.0;
				private double Border = 50.0;  // Offset inside of Size that keeps sites from getting too close to Max
				private Vector3 VOrigin = Vector3.zero; // Could be used to offset in world space the entire world.
				private float InfinityLength = 1.0f; // Voronoi diagrams produce edge of the map vectors pointing to infinity, we use a vector of |M| magnitude to represent how far we need to extend it till we get usable data from it. this is M.
	#endregion
	
	#region Noise and Random
				public System.Random r;
				private int Seed;
				public Perlin baseContinentNoise;
				public Scale baseContinentSubtractNoise;
				public Turbulence perturbedContinentNoise;
				public RiggedMultifractal mountainNoise;
				public Billow baseFlatTerrainNoise;
				public ScaleBias flatTerrainNoise;
				public Perlin terrainTypeNoise;
				public Select finalTerrainNoise;
				public Subtract finalTerrainNoiseMinusContinentNoise;
				public Perlin heatNoise;
				public Perlin moistureNoise;
				public Perlin nutrientNoise;
				public Perlin mineralNoise;
				private double[,] heatValues;
				private double[,] moistureValues;
				private double[,] nutrientValues;

	#endregion

	#region Voronoi Graph
				private Vector[] Sites_World;
				private VoronoiGraph Graph_World;
				private bool GenerateCells = true;
				private bool Relax = false;
				private int RelaxSteps = 3;
				private bool ClampValues = true;
				private int NumberOfSites = 512; // Number of seed points to generate for the voronoi diagram, the higher the number the more tesselated the map will be.		

				private bool BuildGraph ()
				{
						
						Sites_World = GenerateRandom (Seed, NumberOfSites, 0, Max);
						Profiler.BeginSample ("Fortune Compute VoronoiGraph");
						Graph_World = Fortune.ComputeVoronoiGraph (Sites_World);
						Profiler.EndSample ();
						if (Relax) {
								// Lloyd Relax the points here to get more even distribution.
								// First we need to know the N number of Voronoi Verts that make up each cell.
								// Then we can find the centroid of each of those Cells, and move the Site that created this cell to that centroid.
								// Finally we replace our existing Voronoi graph with a new one made from these centered sites.
								// This will be computationally expensive so we don't want to do it too many times, 2 max??
								for (int i = 0; i < RelaxSteps; i++) {
										Sites_World = LloydRelax (Sites_World, Graph_World);
										Graph_World = Fortune.ComputeVoronoiGraph (Sites_World);
								}
						}
						// now you have all the voronoi vertices in graph.Vertices and all the voronoi edges in Relsult.Edges
						//Debug.Log ("Number of Sites: " + sites.Length); 
						//Debug.Log ("graph.Verts Length: " + graph.Vertizes.Count);
						
						if (GenerateCells) {
								Debug.Log ("Starting Generating Cells in the BuildGraph, Max is: " + Max);
								//Convert Voronoi to Cells
								for (int j =0; j < Sites_World.Length; j++) {					
										Vector v = Sites_World [j];
										Cell c = new Cell (v);
										
										c.max = Max;
										c.border = false;
										List<VoronoiEdge> currentBorders = new List<VoronoiEdge> ();
										// Find Cell VoronoiEdges
										foreach (VoronoiEdge edge in Graph_World.Edges) {
												if (edge.LeftData == v || edge.RightData == v) {
														if (edge.IsInfinite) {
																c.IsBorder ();
																Vector AdjustedVec = edge.FixedPoint;
																edge.VVertexA = AdjustedVec;
																edge.VVertexB = AdjustedVec + (edge.DirectionVector * InfinityLength);
														} else if (edge.IsPartlyInfinite) {
																c.IsBorder ();
													
																Vector AdjustedVec = edge.FixedPoint;
																
																if (edge.VVertexA == Fortune.VVInfinite) {
																		edge.VVertexA = AdjustedVec;
																} 
																if (edge.VVertexB == Fortune.VVInfinite) {
																		edge.VVertexB = AdjustedVec;
																}
														}

														BorderCellMax = Max - Border;
														BorderCellMin = Border;

														if (edge.VVertexA [0] < BorderCellMin || edge.VVertexA [0] > BorderCellMax || edge.VVertexA [1] < BorderCellMin || edge.VVertexA [1] > BorderCellMax ||
																edge.VVertexB [0] < BorderCellMin || edge.VVertexB [0] > BorderCellMax || edge.VVertexB [1] < BorderCellMin || edge.VVertexB [1] > BorderCellMax) {
																c.IsBorder ();
														}

														if (!currentBorders.Contains (edge)) {
																currentBorders.Add (edge);
														}

														if (!c.neighborCells.Contains (edge.RightData)) {
																c.neighborCells.Add (edge.RightData);
														}
												} else if (edge.RightData == v) {
							
														if (!c.neighborCells.Contains (edge.LeftData)) {
																c.neighborCells.Add (edge.LeftData);
														}
												}
				
										}
										c.voronoiEdges = currentBorders;
										CellGraph.Add (v, c);
								}
						}

						//TODO make exceptions to handle errors during graph creation.
						if (CellGraph.Count > 0) {
								return true;
						} else {
								return false;
						}
				}

				// Lloyd Relax the points here to get more even distribution.
				// First we need to know the N number of Voronoi Verts that make up each cell.
				// Then we can find the centroid of each of those cells, and move the Site that created this cell to that centroid.
				public Vector[] LloydRelax (Vector[] sites, VoronoiGraph graph)
				{
						List<Vector> RelaxedSites = new List<Vector> ();
						for (int s =0; s < sites.Length; s++) {
								List<Vector> cellVerts = new List<Vector> ();

								foreach (VoronoiEdge edge in graph.Edges) {
										if (edge.LeftData == sites [s]) {
												// Check if one or more of the edge verts is infinity
												if (edge.IsPartlyInfinite) {
														if (edge.VVertexA == Fortune.VVInfinite) {
																edge.VVertexA = (edge.FixedPoint + (edge.DirectionVector * InfinityLength));
														}
														if (edge.VVertexB == Fortune.VVInfinite) {
																edge.VVertexB = (edge.FixedPoint + (edge.DirectionVector * InfinityLength));
														}
												}
												edge.Clamp (Border, Max - Border);
												cellVerts.Add (edge.VVertexA);
												cellVerts.Add (edge.VVertexB);
										}

										// If we didn't find a LeftData for this edge just put that site back uncentered.
										Vector centroid = sites [s];
										if (cellVerts.Count > 0) {
												centroid = math_utilities.Compute2DPolygonCentroid (cellVerts);
												centroid.Clamp (Border, Max - Border);// lets hope this works better.
										}
				
										if (!RelaxedSites.Contains (centroid)) {
												RelaxedSites.Add (centroid);
										}
								}
						}
						return RelaxedSites.ToArray ();
				}
		
				public Vector[] GenerateRandom (int seed, double numPoints, double min, double max)
				{
						//Debug.Log ("Starting to Generate Random Numbers, Max is: " + max);
						List<Vector> tempList = new List<Vector> ();
						UnityEngine.Random.seed = seed;
						Vector p;
						while (tempList.Count < numPoints) {
								p = new Vector (UnityEngine.Random.Range ((int)min, (int)max), UnityEngine.Random.Range ((int)min, (int)max));
								if (!tempList.Contains (p)) {
										tempList.Add (p);
								}
						}
						return tempList.ToArray ();
				}

	#endregion

	#region Voronoi Edges
		
	#endregion

	#region Voronoi Verts
		
	#endregion

	#region Cells
				public Dictionary<Vector,Cell> CellGraph = new Dictionary<Vector, Cell> ();
	#endregion

	#region Chunk Manager
		
	#endregion

		#region Initialize
				public void Restart ()
				{
						// Reset the System Random seed.
						r = new System.Random (Seed);
						Sites_World = null;
						Graph_World = null;
						CellGraph.Clear ();
				}
		#endregion

		#region StartGame
				public bool StartGame (double max, double min, int seed, int numberOfSites, 
		                       double ParsedContinentFrequency, double ParsedBiomeFrequency, int ParsedOctaves, double ParsedLacunarity, double ParsedPersistence)
				{
						Debug.Log ("Starting the game, Max is:" + max);
						Max = max;
						Min = min;
						Seed = seed;
						NumberOfSites = numberOfSites;
						baseContinentNoise = new Perlin (ParsedContinentFrequency, ParsedLacunarity, ParsedPersistence, ParsedOctaves, seed, LibNoise.Unity.QualityMode.Medium);
						baseContinentSubtractNoise = new Scale (3, 1, 3, baseContinentNoise);
						Subtract sub = new Subtract (baseContinentNoise, baseContinentSubtractNoise);
						perturbedContinentNoise = new Turbulence (1, sub);			


						mountainNoise = new RiggedMultifractal (ParsedBiomeFrequency, ParsedLacunarity, ParsedOctaves, seed, LibNoise.Unity.QualityMode.Medium);
						baseFlatTerrainNoise = new Billow (ParsedBiomeFrequency, ParsedLacunarity, ParsedPersistence, ParsedOctaves, seed, LibNoise.Unity.QualityMode.Medium);
						flatTerrainNoise = new ScaleBias (0.25, -0.75, baseFlatTerrainNoise);
						terrainTypeNoise = new Perlin (ParsedBiomeFrequency, ParsedLacunarity, ParsedPersistence, ParsedOctaves, seed, LibNoise.Unity.QualityMode.Low);
						finalTerrainNoise = new Select (flatTerrainNoise, mountainNoise, terrainTypeNoise);
						finalTerrainNoise.SetBounds (0, 1000);
						finalTerrainNoise.FallOff = .5;

						finalTerrainNoiseMinusContinentNoise = new Subtract(finalTerrainNoise,perturbedContinentNoise);

						heatNoise = new Perlin (ParsedBiomeFrequency, ParsedLacunarity, ParsedPersistence, ParsedOctaves, seed, LibNoise.Unity.QualityMode.Medium);
						moistureNoise = new Perlin (ParsedBiomeFrequency, ParsedLacunarity, ParsedPersistence, ParsedOctaves, -seed, LibNoise.Unity.QualityMode.Medium);

						return BuildGraph ();
				}
		#endregion

		}
}