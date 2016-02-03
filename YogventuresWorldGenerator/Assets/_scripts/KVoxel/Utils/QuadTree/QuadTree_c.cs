using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using BenTools;
using BenTools.Mathematics;
using BenTools.Data;

using KVoxel;

public class QuadTree_c : MonoBehaviour
{
		public bool ShowWholeTree = false;
		public bool ShowIntersectedTree = false;
		private List<QuadTreeNode_c> Nodes = new List<QuadTreeNode_c> ();
		private List<Rect> Tiles = new List<Rect> ();
		private List<Rect>CloseTiles = new List<Rect> ();
		private List<Rect>FarTiles = new List<Rect> ();
		public int Max = 4096;
		public float Levels = 1;
		public int TileCount = 0;
		public QuadTree QTree;
		public GameObject MapObject;
		public GameObject PBrush;
		public int SearchRadius = 10;
		public int CloseTileCount = 0;
		public int FarTileCount = 0;

		public void Redraw ()
		{
				QTree = null;
				QTree = new QuadTree (new Rect (0, 0, Max, Max), (int)Levels);

				if (MapObject != null) {
						Map_c MapComponent = MapObject.GetComponent<Map_c> ();

						if (MapComponent != null) {
								foreach (GameObject cellObject in MapComponent.CellPrefabs) {
										cell_c cellComponent = cellObject.GetComponent<cell_c> ();
										if (cellComponent != null) {
												QTree.Root.Insert (cellComponent.Cell);
										}
								}
						}
				}

				Tiles.Clear ();
				Tiles = QTree.Root.GetAllQuadsInRect (new Rect (-10, -10, Max + 10, Max + 10));
				TileCount = Tiles.Count;
		}

		Vector3 PreviousPositon = Vector3.zero;
		public Vector3 center ;
		public int close = 50;
		public int far = 500;
		public Rect CloseRect;
		public Rect FarRect;
		public bool OverOcean = false;
		public bool CellIsNull = true;
		public eBiome CellBiome = eBiome.none;

		void Update ()
		{
				if (Input.GetKeyUp (KeyCode.I)) {
						Redraw ();
				}

				if (QTree != null && PBrush.transform.position != PreviousPositon) {

						center = new Vector2 (PBrush.transform.position.x, PBrush.transform.position.z);
						CloseRect = new Rect (center.x - close, center.y - close, close * 2, close * 2);
						FarRect = new Rect (center.x - far, center.y - far, far * 2, far * 2);

						CloseTiles.Clear ();
						FarTiles.Clear ();
						
						CloseTiles = QTree.Root.GetAllQuadsInRectCorners (CloseRect);
						CloseTileCount = CloseTiles.Count;
						PreviousPositon = PBrush.transform.position;

						FarTiles = QTree.Root.GetAllQuadsInRectCorners (FarRect);
						FarTileCount = FarTiles.Count;

						KeyValuePair<bool,Cell> c = QTree.Root.GetNearestCellToAPoint (new Vector2 (PBrush.transform.position.x, PBrush.transform.position.z));
						if (c.Key == false) {
								CellIsNull = true;
								CellBiome = eBiome.none;
						} else {
								CellIsNull = false;
								CellBiome = c.Value.biome;
								if (c.Value.ocean) {
										OverOcean = true;
								} else {
										OverOcean = false;
								}
						}
		
				}
		}

		void OnDrawGizmos ()
		{

				if (Tiles.Count > 0 && ShowWholeTree) {
						Gizmos.color = Color.green;
						foreach (Rect GameBoard in Tiles) {
								Gizmos.DrawLine (new Vector3 (GameBoard.xMin, 0, GameBoard.yMin), new Vector3 (GameBoard.xMax, 0, GameBoard.yMin));
								Gizmos.DrawLine (new Vector3 (GameBoard.xMax, 0, GameBoard.yMin), new Vector3 (GameBoard.xMax, 0, GameBoard.yMax));
								Gizmos.DrawLine (new Vector3 (GameBoard.xMax, 0, GameBoard.yMax), new Vector3 (GameBoard.xMin, 0, GameBoard.yMax));
								Gizmos.DrawLine (new Vector3 (GameBoard.xMin, 0, GameBoard.yMax), new Vector3 (GameBoard.xMin, 0, GameBoard.yMin));
						}
				}

				Gizmos.color = Color.red;
				Gizmos.DrawLine (new Vector3 (CloseRect.xMin, 0, CloseRect.yMin), new Vector3 (CloseRect.xMax, 0, CloseRect.yMin));
				Gizmos.DrawLine (new Vector3 (CloseRect.xMax, 0, CloseRect.yMin), new Vector3 (CloseRect.xMax, 0, CloseRect.yMax));
				Gizmos.DrawLine (new Vector3 (CloseRect.xMax, 0, CloseRect.yMax), new Vector3 (CloseRect.xMin, 0, CloseRect.yMax));
				Gizmos.DrawLine (new Vector3 (CloseRect.xMin, 0, CloseRect.yMax), new Vector3 (CloseRect.xMin, 0, CloseRect.yMin));

				if (CloseTiles.Count > 0 && ShowIntersectedTree) {
						Gizmos.color = Color.red;
						foreach (Rect Tile in CloseTiles) {
								Gizmos.DrawLine (new Vector3 (Tile.xMin, 0, Tile.yMin), new Vector3 (Tile.xMax, 0, Tile.yMin));
								Gizmos.DrawLine (new Vector3 (Tile.xMax, 0, Tile.yMin), new Vector3 (Tile.xMax, 0, Tile.yMax));
								Gizmos.DrawLine (new Vector3 (Tile.xMax, 0, Tile.yMax), new Vector3 (Tile.xMin, 0, Tile.yMax));
								Gizmos.DrawLine (new Vector3 (Tile.xMin, 0, Tile.yMax), new Vector3 (Tile.xMin, 0, Tile.yMin));
						}
				}

				Gizmos.color = Color.yellow;
				Gizmos.DrawLine (new Vector3 (FarRect.xMin, 0, FarRect.yMin), new Vector3 (FarRect.xMax, 0, FarRect.yMin));
				Gizmos.DrawLine (new Vector3 (FarRect.xMax, 0, FarRect.yMin), new Vector3 (FarRect.xMax, 0, FarRect.yMax));
				Gizmos.DrawLine (new Vector3 (FarRect.xMax, 0, FarRect.yMax), new Vector3 (FarRect.xMin, 0, FarRect.yMax));
				Gizmos.DrawLine (new Vector3 (FarRect.xMin, 0, FarRect.yMax), new Vector3 (FarRect.xMin, 0, FarRect.yMin));

		}
}
