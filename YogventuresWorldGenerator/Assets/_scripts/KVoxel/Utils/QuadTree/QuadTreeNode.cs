//-----------------------------------------------------------------------
// <copyright file="center.cs" company="Winterkewl Games LLC">
//     Copyright (c) Winterkewl Games LLC.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using UnityEngine;
using System.Collections.Generic;
using BenTools.Mathematics;
using BenTools.Data;

namespace KVoxel
{
		public class QuadTreeNode
		{
				// Maximum Number of Branches we want to make.
				public int MaxDepth;

				// The Branch Level this Node is in.
				public int Depth;

				// Each node in the quadtree has an ID which will identify
				// it to other nodes in the tree these ID's may be superfulous
				// Won't know until all the methods are complete.
				public Vector2 ID;

				// Every node except the root has a parent ID
				// We use this to walk up a tree from leaf to root
				public Vector2 ParentID;

				// Every node is a division of space
				// That division of space has a MinX, MaxX, MinZ and MaxZ
				// Rect has all of these and already does "contains" and "overlaps" 
				// So Rect is a good way to store the actual Bounds of the node.
				public Rect BoundingBox;

				// Every Node except Leaves have 4 children
				// This array stores the IDs of those children.
				public QuadTreeNode[] Children;

				// If a node is a Leaf i.e. as deep as the tree can go.
				// it will have a Cells stored in it.
				// TODO make some sort've lookup ID so we only store a pointer to the cell instead of a copy of the whole cell?
				public List<Cell> Cells;

				public QuadTreeNode (Rect bBox, Vector2 parentID, int maxDepth, int depth)
				{
						BoundingBox = bBox;
						ID = bBox.center;
						ParentID = parentID;
						MaxDepth = maxDepth;
						Depth = depth;

						Cells = new List<Cell> ();
				}

				/// <summary>
				/// Quads the contains any point in cell.
				/// No Matter how small the piece, even if it's only one VoronoiEdge point
				/// We need to know if the polygon of the cell overlaps the Quad, if so 
				/// Add that cell to this Quads list of cells to know about.
				/// This will lead to duplicate data, it would be better to just keep cells in the
				/// smallest box that will contain it completely, but for now I'm tired, and can't think.
				/// </summary>
				/// <returns><c>true</c>, if contains any point in cell was quaded, <c>false</c> otherwise.</returns>
				/// <param name="c">C.</param>
				public bool QuadContainsEveryPointInCell (Cell c)
				{
						if (!BoundingBox.Contains (c.site.AsVector2 ()))
								return false;

						foreach (VoronoiEdge edge in c.voronoiEdges) {
								if (edge.VVertexA != Fortune.VVInfinite) {
										if (!BoundingBox.Contains (edge.VVertexA.AsVector2 ()))
												return false;
								}

								if (edge.VVertexB != Fortune.VVInfinite) {
										if (!BoundingBox.Contains (edge.VVertexB.AsVector2 ()))
												return false;
								}

						}

						return true;
				}

				/// <summary>
				/// Insert the specified Cell In the tree.
				/// </summary>
				/// <param name="c">C.</param>
				public bool Insert (Cell c)
				{
						
						//Debug.Log ("Inserting Cell: " + c.site.AsVector2 () + " into Quad: " + ID + " at Depth: " + Depth + " Max Depth: " + MaxDepth);
						// First make sure this cell is in bounds.
						// It should always be, but just to be sure.
						//if (!BoundingBox.Contains (c.site.AsVector2 ()))
						if (!QuadContainsEveryPointInCell (c))
								return false; // object could not be added.
						
						Cells.Add (c);

						// If we don't already have children  
						if (Children == null) { // AND we aren't too deep in the tree. . .
								if ((Depth + 1) < MaxDepth) {
										Split (); // Split to make room.
										return true;
//								} else { // If we ARE at Max Depth stick the cell at this level.
//										Cells.Add (c);
//										return true;
								}
						}

						if (Children != null) {
								// If we split this quad . . . 
								// Now Check if the point fits in a Child.
								if (Children [0].Insert (c))
										return true;

								if (Children [1].Insert (c))
										return true;

								if (Children [2].Insert (c))
										return true;

								if (Children [3].Insert (c))
										return true;
						}
						// For some reason this cell couldn't be inserted, this should never happen!
						//Debug.Log ("Could Not Insert cell: " + c.site.AsVector2 ().ToString ());
						return false; 
				}

				public void Split ()
				{
						//Debug.Log ("Spliting Quad: " + ID);
						Children = new QuadTreeNode[4];
						float HalfLength = BoundingBox.width / 2;
						Rect c0Rect = new Rect (BoundingBox.xMin, BoundingBox.yMin, HalfLength, HalfLength);
						Rect c1Rect = new Rect (BoundingBox.xMin + HalfLength, BoundingBox.yMin, HalfLength, HalfLength);
						Rect c2Rect = new Rect (BoundingBox.xMin, BoundingBox.yMin + HalfLength, HalfLength, HalfLength);
						Rect c3Rect = new Rect (BoundingBox.xMin + HalfLength, BoundingBox.yMin + HalfLength, HalfLength, HalfLength);

						Children [0] = new QuadTreeNode (c0Rect, ID, MaxDepth, Depth + 1);
						Children [1] = new QuadTreeNode (c1Rect, ID, MaxDepth, Depth + 1);
						Children [2] = new QuadTreeNode (c2Rect, ID, MaxDepth, Depth + 1);
						Children [3] = new QuadTreeNode (c3Rect, ID, MaxDepth, Depth + 1);
				}

				// Find all points which appear within a range
				public List<Rect> GetAllQuadsInRect (Rect bbox)
				{
						// Prepare an array of results
						List<Rect> foundQuads = new List<Rect> ();
						// Automatically abort if the range does not collide with this quad
						if (!BoundingBox.Overlaps (bbox))
								return foundQuads;
						// Check objects at this quad level
						if (bbox.Contains (new Vector3 (ID.x, 0, ID.y)))
								foundQuads.Add (BoundingBox);
						// Terminate here, if there are no children
						if (Children == null)
								return foundQuads;
						// Otherwise, add the points from the children
						foundQuads.AddRange (Children [0].GetAllQuadsInRect (bbox));
						foundQuads.AddRange (Children [1].GetAllQuadsInRect (bbox));
						foundQuads.AddRange (Children [2].GetAllQuadsInRect (bbox));
						foundQuads.AddRange (Children [3].GetAllQuadsInRect (bbox));

						return foundQuads;
				}


				// Find all points which appear within a range
				public List<Rect> GetAllQuadsInRectCorners (Rect bbox)
				{
						// Prepare an array of results
						List<Rect> foundQuads = new List<Rect> ();
						// Automatically abort if the range does not collide with this quad
						if (!BoundingBox.Contains (new Vector2 (bbox.xMin, bbox.yMin)) && !BoundingBox.Contains (new Vector2 (bbox.xMin, bbox.yMax)) &&
								!BoundingBox.Contains (new Vector2 (bbox.xMax, bbox.yMin)) && !BoundingBox.Contains (new Vector2 (bbox.xMax, bbox.yMax))
								&& !BoundingBox.Overlaps (bbox))
								return foundQuads;

						// Check objects at this quad level
						if (bbox.Overlaps (BoundingBox)
								|| bbox.Contains (new Vector2 (BoundingBox.xMin, BoundingBox.yMin))
								|| bbox.Contains (new Vector2 (BoundingBox.xMin, BoundingBox.yMax))
								|| bbox.Contains (new Vector2 (BoundingBox.xMax, BoundingBox.yMin))
								|| bbox.Contains (new Vector2 (BoundingBox.xMax, BoundingBox.yMax)) 
								|| bbox.Contains (BoundingBox.center))
								foundQuads.Add (BoundingBox);

						// Terminate here, if there are no children
						if (Children == null)
								return foundQuads;
						// Otherwise, add the points from the children
						foundQuads.AddRange (Children [0].GetAllQuadsInRectCorners (bbox));
						foundQuads.AddRange (Children [1].GetAllQuadsInRectCorners (bbox));
						foundQuads.AddRange (Children [2].GetAllQuadsInRectCorners (bbox));
						foundQuads.AddRange (Children [3].GetAllQuadsInRectCorners (bbox));
			
						return foundQuads;
				}


				// Find all points which appear within a range
				public List<Rect> GetAllQuadsPointIsInside (Vector2 point)
				{
						// Prepare an array of results
						List<Rect> foundQuads = new List<Rect> ();

						// Automatically abort if the range does not collide with this quad
						if (!BoundingBox.Contains (point))
								return foundQuads;

						// Check objects at this quad level
						foundQuads.Add (BoundingBox);

						// Terminate here, if there are no children
						if (Children == null)
								return foundQuads;
						// Otherwise, add the points from the children
						foundQuads.AddRange (Children [0].GetAllQuadsPointIsInside (point));
						foundQuads.AddRange (Children [1].GetAllQuadsPointIsInside (point));
						foundQuads.AddRange (Children [2].GetAllQuadsPointIsInside (point));
						foundQuads.AddRange (Children [3].GetAllQuadsPointIsInside (point));
			
						return foundQuads;
				}

				public KeyValuePair<bool,Cell> GetNearestCellToAPoint (Vector2 point)
				{
						List<Cell> candidates = new List<Cell> ();
						candidates = FindNearestCellsToAPoint (point);
						float MinDistance = 100000;
						float currentDistance = 100000;
						Cell foundCell = null;
						foreach (Cell c in candidates) {
								currentDistance = Vector2.Distance (point, c.site.AsVector2 ());
								if (currentDistance < MinDistance) {
										MinDistance = currentDistance;
										foundCell = c;
								}
						}
						if (foundCell != null) {
								return new KeyValuePair<bool, Cell> (true, foundCell);
						} else {
								return new KeyValuePair<bool, Cell> (false, null);
						}
				}
		
				public List<Cell> FindNearestCellsToAPoint (Vector2 point)
				{
						List<Cell> foundCells = new List<Cell> ();

						// Automatically abort if the range does not collide with this quad
						if (!BoundingBox.Contains (point)) {
								return foundCells;
						}
			
						// Check objects at this quad level
						if (Cells.Count > 0) {
								foreach (Cell c in Cells) {
										foundCells.Add (c);
								}
						}
			
						// Terminate here, if there are no children
						if (Children == null)
								return foundCells;

						// Otherwise, add the points from the children
						foundCells.AddRange (Children [0].FindNearestCellsToAPoint (point));
						foundCells.AddRange (Children [1].FindNearestCellsToAPoint (point));
						foundCells.AddRange (Children [2].FindNearestCellsToAPoint (point));
						foundCells.AddRange (Children [3].FindNearestCellsToAPoint (point));
			
						return foundCells;
				}

				public List<Cell> FindNearestCellsToARect (Rect bbox)
				{
						List<Cell> foundCells = new List<Cell> ();

						// Automatically abort if the range does not collide with this quad
						if (!BoundingBox.Contains (new Vector2 (bbox.xMin, bbox.yMin)) && !BoundingBox.Contains (new Vector2 (bbox.xMin, bbox.yMax)) &&
								!BoundingBox.Contains (new Vector2 (bbox.xMax, bbox.yMin)) && !BoundingBox.Contains (new Vector2 (bbox.xMax, bbox.yMax))
								&& !BoundingBox.Overlaps (bbox))
								return foundCells;
			
						// Check objects at this quad level
						if (bbox.Overlaps (BoundingBox)
								|| bbox.Contains (new Vector2 (BoundingBox.xMin, BoundingBox.yMin))
								|| bbox.Contains (new Vector2 (BoundingBox.xMin, BoundingBox.yMax))
								|| bbox.Contains (new Vector2 (BoundingBox.xMax, BoundingBox.yMin))
								|| bbox.Contains (new Vector2 (BoundingBox.xMax, BoundingBox.yMax)) 
								|| bbox.Contains (BoundingBox.center)) {
								if (Cells.Count > 0) {
										foreach (Cell c in Cells) {
												foundCells.Add (c);
										}
								}
						}

						// Terminate here, if there are no children
						if (Children == null)
								return foundCells;

						// Otherwise, add the points from the children
						foundCells.AddRange (Children [0].FindNearestCellsToARect (bbox));
						foundCells.AddRange (Children [1].FindNearestCellsToARect (bbox));
						foundCells.AddRange (Children [2].FindNearestCellsToARect (bbox));
						foundCells.AddRange (Children [3].FindNearestCellsToARect (bbox));
			
						return foundCells;
				}
		
		
		
		}
}
