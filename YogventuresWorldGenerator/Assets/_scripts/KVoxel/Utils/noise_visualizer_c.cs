using UnityEngine;
using System.Collections;

public class noise_visualizer_c : MonoBehaviour
{
		Mesh mesh;
		MeshFilter fMesh;
		public MeshRenderer rMesh;
		int Max = 1000;
		int Min = 0;
		int UVMin = 0;
		int UVMax = 1;
		public Vector3[] Verts;
		public Vector3[] Norms;
		public Vector2[] Uvs;
		public int[] Tris;
		public Texture2D texture;
		// Use this for initialization
		void Start ()
		{
				texture = new Texture2D (Max, Max, TextureFormat.ARGB32, false);

				Verts = new Vector3[]{
			new Vector3 (Min, 0, Min),
			new Vector3 (Max, 0, Min), 
			new Vector3 (Max, 0, Max), 
			new Vector3 (Min, 0, Max)};
		
				Norms = new Vector3[]{
			new Vector3 (Min, 1, Min),
			new Vector3 (Min, 1, Min), 
			new Vector3 (Min, 1, Min), 
			new Vector3 (Min, 1, Min)};
		
				Uvs = new Vector2[]{
			new Vector2 (UVMin, UVMin),
			new Vector2 (UVMax, UVMin), 
			new Vector2 (UVMax, UVMax), 
			new Vector2 (UVMin, UVMax)};

				Tris = new int[]{0,2,1,0,3,2};

				mesh = new Mesh ();
				mesh.vertices = Verts;
				mesh.normals = Norms;
				mesh.uv = Uvs;
				mesh.triangles = Tris;
				if (fMesh == null) {
						fMesh = GetComponent<MeshFilter> ();
				}
				fMesh.sharedMesh = mesh;
				for (int x =0; x < Max; x++) {
						for (int z = 0; z < Max; z++) {
								// set the pixel values
								texture.SetPixel (x, z, Color.black);
						}
				}
				// Apply all SetPixel calls
				texture.Apply ();

				if (rMesh == null) {
						rMesh = GetComponent<MeshRenderer> ();
				}
				// connect texture to material of GameObject this script is attached to
				rMesh.GetComponent<Renderer>().material.mainTexture = texture;

		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}

		public void VoidRedrawNoise ()
		{
				// set the pixel values
				texture.SetPixel (0, 0, new Color (1.0f, 1.0f, 1.0f, 0.5f));
				texture.SetPixel (1, 0, Color.clear);
				texture.SetPixel (0, 1, Color.white);
				texture.SetPixel (1, 1, Color.black);
		
				// Apply all SetPixel calls
				texture.Apply ();
		
				// connect texture to material of GameObject this script is attached to
				rMesh.GetComponent<Renderer>().material.mainTexture = texture;
		}
}
