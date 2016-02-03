using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using KVoxel;

public class Terrain_c : MonoBehaviour
{
		
		public QuadTree_c QuadTreeComponent;
		public MeshFilter fMesh;
		public MeshRenderer rMesh;
		public int Size = 256;
		public int step = 8;
		public int height = 256;
		public Map_c MapComponent;
		public int VertCount = 0;

		public void Start ()
		{
				fMesh = gameObject.AddComponent<MeshFilter> ();
				rMesh = gameObject.GetComponent<MeshRenderer> ();
		}

		private float GetHeight (float x, float z)
		{
				//Debug.Log ("Looking for: " + x + ":" + z);
				KeyValuePair<bool,Cell> c = QuadTreeComponent.QTree.Root.GetNearestCellToAPoint (new Vector2 (x, z));
				float y = 0;
				if (c.Key == true) {
						if (c.Value.ocean) {
								y = 0;
						} else {
								y = (float)MapComponent.ReMap (MapComponent.Map.finalTerrainNoise.GetValue (new Vector3 (x, 0, z)), - 5, 8, 0, 1);
						}
				} else {
						y = 0;
				}
				//Debug.Log ("h: " + y);
				return y * height;
		}
	
		public void BuildTerrain ()
		{
				List<Vector3> vertsArr = new List<Vector3> ();
				List<Vector3> normsArr = new List<Vector3> ();
				List<Vector2> uvsArr = new List<Vector2> ();
				List<int> trisArr = new List<int> ();
				Mesh mesh = new Mesh ();

				for (int x =0; x < Size; x+=2) {
						for (int z = 0; z < Size; z+=2) {
								//heights [x] = (float)MapComponent.ReMap (MapComponent.Map.finalTerrainNoise.GetValue (new Vector3 (x, 0, z)), - 5, 8, 0, 1);
								float steppedX = x * step;
								float steppedZ = z * step;

								float sharedY = GetHeight (steppedX + step, steppedZ + step);

								vertsArr.Add (new Vector3 (steppedX - step, GetHeight (steppedX - step, steppedZ - step), steppedZ - step));
								normsArr.Add (Vector3.up);
								uvsArr.Add (Vector2.zero);

								vertsArr.Add (new Vector3 (steppedX + step, sharedY, steppedZ + step));
								normsArr.Add (Vector3.up);
								uvsArr.Add (Vector2.zero);

								vertsArr.Add (new Vector3 (steppedX + step, GetHeight (steppedX + step, steppedZ - step), steppedZ - step));
								normsArr.Add (Vector3.up);
								uvsArr.Add (Vector2.zero);

								vertsArr.Add (new Vector3 (steppedX + step, sharedY, steppedZ + step));
								normsArr.Add (Vector3.up);
								uvsArr.Add (Vector2.zero);

								vertsArr.Add (new Vector3 (steppedX - step, GetHeight (steppedX - step, steppedZ - step), steppedZ - step));
								normsArr.Add (Vector3.up);
								uvsArr.Add (Vector2.zero);

				
								vertsArr.Add (new Vector3 (steppedX - step, GetHeight (steppedX - step, steppedZ + step), steppedZ + step));
								normsArr.Add (Vector3.up);
								uvsArr.Add (Vector2.zero);


						}
				}

				for (int i =0; i < vertsArr.Count; i++) {
						trisArr.Add (i);
				}

				mesh.vertices = vertsArr.ToArray ();
				mesh.normals = normsArr.ToArray ();
				mesh.uv = uvsArr.ToArray ();
				mesh.triangles = trisArr.ToArray ();
				fMesh.sharedMesh = mesh;

				VertCount = vertsArr.Count;
		}
	
		public void Update ()
		{
				if (Input.GetKeyUp (KeyCode.T)) {
						BuildTerrain ();
				}
		}
}
