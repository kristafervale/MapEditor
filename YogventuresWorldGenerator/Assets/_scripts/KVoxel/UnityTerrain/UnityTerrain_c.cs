using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using KVoxel;

public class UnityTerrain_c : MonoBehaviour
{
		public Terrain terrain;
		public Map_c MapComponent;
		public QuadTree_c QuadTreeComponent;

		public void BuildTerrain ()
		{
				float[,] heights = terrain.terrainData.GetHeights (0, 0, terrain.terrainData.heightmapWidth, terrain.terrainData.heightmapHeight);

				for (int x =0; x < heights.GetLength(0); x++) {
						for (int z = 0; z < heights.GetLength(1); z++) {

								//heights [x, z] = (float)MapComponent.ReMap (MapComponent.Map.finalTerrainNoise.GetValue (new Vector3 (x, 0, z)), - 5, 8, 0, 1);
								KeyValuePair<bool,Cell> c = QuadTreeComponent.QTree.Root.GetNearestCellToAPoint (new Vector2 (x, z));
								
								if (c.Key == true) {
										if (c.Value.ocean) {
												heights [x, z] = 0;
										} else {
												heights [x, z] = .5f;
												//heights [x, z] = (float)MapComponent.ReMap (MapComponent.Map.finalTerrainNoiseMinusContinentNoise.GetValue (new Vector3 (x * 4, 0, z * 4)), - 5, 8, 0.01, .65);
										}
								} else {
										heights [x, z] = 0;
								}
						}
				}



				terrain.terrainData.SetHeights (0, 0, heights);
				//terrain.gameObject.isStatic = false;
				//terrain.gameObject.transform.position = new Vector3 (1024, 0, 1024);
				//terrain.gameObject.isStatic = true;
		}

		public void Update ()
		{
				if (Input.GetKeyUp (KeyCode.T)) {
						BuildTerrain ();
				}
		}
}
