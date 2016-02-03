using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Vectrosity;
using BenTools.Mathematics;

using KVoxel;

public class Map_c : MonoBehaviour
{
	#region Game
		public bool GameStarted;
		public bool CellFactoryStarted;
		public bool CellFactoryFinished;
		public bool MapDataFinished;
		public bool MapSetupFinished;
		public GameObject PaintBrushPrefab;
	#endregion

	#region Config
		public CSVReader Config;
		public List<Biome> BiomeDefinitions = new List<Biome> ();
	#endregion

	#region Data
		public Map Map;
	#endregion

	#region GUI
		public string MapNameString = "NewMap";
		public string SeedString = "256";
		public string NumberOfSitesString = "256";
		public int ParsedSeed = 256;
		public int ParsedNumberOfSites = 512;
		public double Max = 2000.0;
		public double Min = 0.0;
		public Rect SelectedBiomePaintIndexPosition;
		public bool SelectedBiomePaintIndexShowList;
		public int SelectedBiomePaintIndex = 0;
		public GUIContent SelectedBiomePaintIndexButtonContent;
		public GUIContent[] SelectedBiomePaintIndexListContent;
		public GUIStyle SelectedBiomePaintIndexListStyle;
		public string CurrentSelectedBiomePaintBrushLabelString = "none";
		public float CameraZoomSliderValue = 500.0F;
		public float CameraXSliderValue = 500f;
		public float CameraZSliderValue = 500f;
		public float MouseSensitivity = .06f;
		public float PaintBrushSizeSliderValue = 10;
		public string ContinentFrequencyString = "0.003";
		public string BiomeFrequencyString = "0.003";
		public string NoiseOctavesString = "1";
		public string NoiseLacunarityString = "0.5";
		public string NoisePersistenceString = "0.8";
		public float ParsedContinentFrequency = 0.003f;
		public float ParsedBiomeFrequency = 0.003f;
		public int ParsedOctaves = 1;
		public float ParsedLacunarity = 0.0f;
		public float ParsedPersistence = 0.0f;
		public bool FirstPerson = false;
		public GameObject NoiseVisualizerGameObject;
		public double EquatorLatitudeMin = .2;
		public double EquatorLatitudeMax = .2;
		public double ArticLatitude = .2 ;
		public double AntArticLatitude = .2;

		/// <summary>
		/// Temporary until we replace it with real scaleform.
		/// </summary>
		void OnGUI ()
		{

				GUI.Label (new Rect (10, 10, 60, 20), "Map Name");
				MapNameString = GUI.TextField (new Rect (100, 10, 200, 20), MapNameString, 40);

				GUI.Label (new Rect (10, 40, 60, 20), "Seed");
				SeedString = GUI.TextField (new Rect (100, 40, 70, 20), SeedString, 9);
				GUI.Label (new Rect (10, 70, 70, 20), "NumSites");
				NumberOfSitesString = GUI.TextField (new Rect (100, 70, 70, 20), NumberOfSitesString, 9);
				GUI.Label (new Rect (10, 100, 70, 20), "Continent Freq.");
				ContinentFrequencyString = GUI.TextField (new Rect (100, 100, 70, 20), ContinentFrequencyString, 9);
				GUI.Label (new Rect (10, 130, 70, 20), "Biome Freg.");
				BiomeFrequencyString = GUI.TextField (new Rect (100, 130, 70, 20), BiomeFrequencyString, 9);

				GUI.Label (new Rect (10, 160, 70, 20), "Octaves");
				NoiseOctavesString = GUI.TextField (new Rect (100, 160, 70, 20), NoiseOctavesString, 9);
				GUI.Label (new Rect (10, 190, 70, 20), "Lacunarity");
				NoiseLacunarityString = GUI.TextField (new Rect (100, 190, 70, 20), NoiseLacunarityString, 9);
				GUI.Label (new Rect (10, 220, 70, 20), "Persistence");
				NoisePersistenceString = GUI.TextField (new Rect (100, 220, 70, 20), NoisePersistenceString, 9);
				if (GUI.Button (new Rect (10, 260, 180, 30), "Build")) {
						Restart ();
				}

				if (GUI.Button (new Rect (10, 300, 180, 30), "Save")) {
						Restart ();
				}

				GUI.Label (new Rect (10, 330, 200, 20), "Brush Size: " + PaintBrushSizeSliderValue);
				PaintBrushSizeSliderValue = GUI.HorizontalSlider (new Rect (10, 360, 200, 20), PaintBrushSizeSliderValue, 1.0F, 250.0F);
				
				GUI.Label (new Rect (10, 390, 200, 20), "Current Biome Brush: " + CurrentSelectedBiomePaintBrushLabelString);
				if (Popup.List (new Rect (10, 430, 200, 30), ref SelectedBiomePaintIndexShowList, ref SelectedBiomePaintIndex, 
		                SelectedBiomePaintIndexButtonContent, BiomeHelper.GetBiomeGuiContent (), SelectedBiomePaintIndexListStyle)) {
						CurrentSelectedBiomePaintBrushLabelString = BiomeHelper.BiomeStringNamesArray [SelectedBiomePaintIndex];
				}

				if (GUI.Button (new Rect (10, Screen.height - 40, 80, 30), "Quit")) {
						Application.Quit ();
				}

				GUI.Label (new Rect (Screen.width - 210, 10, 200, 20), "Camera Zoom: " + CameraZoomSliderValue);
				GUI.Label (new Rect (Screen.width - 210, 40, 200, 20), "Camera X: " + CameraXSliderValue);
				GUI.Label (new Rect (Screen.width - 210, 70, 200, 20), "Camera Z: " + CameraZSliderValue);

				if (GUI.Button (new Rect (Screen.width - 210, 100, 180, 30), "Camera Center")) {
						CameraZSliderValue = (float)(Max * .5);
						CameraXSliderValue = (float)(Max * .5);
						CameraZoomSliderValue = (float)(Max * .5);
				}

		}

	#endregion

	#region Vector Lines and Points

	#endregion

	#region Noise and Random
	
	#endregion
	
	#region Cells
		public GameObject CellPrefab;
		public List<GameObject> CellPrefabs = new List<GameObject> ();
		public System.Collections.Generic.Queue<Vector> CellCreationQueue = new System.Collections.Generic.Queue<Vector> ();
		public int MaxCellsPerFrame = 500;
		public Dictionary<Vector,int> SiteToCellPrefabLookupTable = new Dictionary<Vector, int> ();
	#endregion

	#region Chunk Manager
	
	#endregion

	#region Initialize
		public void Restart ()
		{
				BiomeHelper.SetupColors (); 
				MapSetupFinished = false;
				MapDataFinished = false;
				CellFactoryStarted = false;
				CellFactoryFinished = false;
				GameStarted = false;

				CameraZSliderValue = (float)(Max * .5);
				CameraXSliderValue = (float)(Max * .5);
				CameraZoomSliderValue = (float)(Max * .5);

				CellCreationQueue.Clear ();
				// First we must reset all the cells
				//Debug.Log ("I should Delete " + CellPrefabs.Count + " Cells now.");
				for (int i =0; i < CellPrefabs.Count; i++) {
						CellPrefabs [i].GetComponent<cell_c> ().DestroyYouself ();
				}

				CellPrefabs.Clear ();
				SiteToCellPrefabLookupTable.Clear ();
				Map.Restart ();
				StartGame ();
		}
	#endregion

	#region GameSetup
		// Use this for initialization
		void Awake ()
		{
				Map = new Map ();
				BiomeHelper.SetupColors ();

				SelectedBiomePaintIndexListStyle.normal.textColor = Color.white; 
				SelectedBiomePaintIndexListStyle.onHover.background = new Texture2D (2, 2);
				SelectedBiomePaintIndexListStyle.hover.background = new Texture2D (2, 2);
				SelectedBiomePaintIndexListStyle.padding.left = 20;
				SelectedBiomePaintIndexListStyle.padding.right = 20;
				SelectedBiomePaintIndexListStyle.padding.top = 4;
				SelectedBiomePaintIndexListStyle.padding.bottom = 4;
				Config = GetComponent<CSVReader> ();
				Config.ParseFile ();
				foreach (KeyValuePair<int,float[]>pair in Config.ParsedData) {
						Biome tmpBiome = new Biome (pair.Value);
						BiomeDefinitions.Add (tmpBiome);
				}

				//Debug.Log ("Config has " + BiomeDefinitions.Count + " biome definitions");
				foreach (Biome b in BiomeDefinitions) {
						//Debug.Log ("Heat Max:" + b.Heat_Max);
				}
		}

		void Start ()
		{
				StartCoroutine ("CellFactory");
		}

		void StartGame ()
		{
				
				try {
						if (int.TryParse (SeedString, out ParsedSeed)) {
								if (int.TryParse (NumberOfSitesString, out ParsedNumberOfSites)) {
										Debug.Log ("Trying to Start the game");

										float.TryParse (ContinentFrequencyString, out ParsedContinentFrequency);
										float.TryParse (BiomeFrequencyString, out ParsedBiomeFrequency);
										int.TryParse (NoiseOctavesString, out ParsedOctaves);
										float.TryParse (NoiseLacunarityString, out ParsedLacunarity);
										float.TryParse (NoisePersistenceString, out ParsedPersistence);

										MapDataFinished = Map.StartGame (Max, Min, 
					                                 ParsedSeed, ParsedNumberOfSites,
					                                 (double)ParsedContinentFrequency,
					                                 (double)ParsedBiomeFrequency,
					                                 ParsedOctaves, 
					                                 (double)ParsedLacunarity, (double)ParsedPersistence);
					
										//Setup the PrefabCellGraph
										if (MapDataFinished) {
												StartCreatingCells ();
												
										} else {
												throw new KVoxel.Exceptions.GameFailedToStartException ("The Game could not be started.");
										}
								} else {
										throw new KVoxel.Exceptions.GameFailedToStartException ("There was an error parsing Number of Sites field.");
								}
						} else {
								throw new KVoxel.Exceptions.GameFailedToStartException ("There was an error parsing Seed field.");
						}
				} catch (KVoxel.Exceptions.GameFailedToStartException ex) {
						System.Console.WriteLine (ex.Message);
						Debug.LogError (ex.Message);
				}
		}

		void StartCreatingCells ()
		{
				CellFactoryStarted = true;
				// Reuse where we can, instantiate new only if necessary.
				foreach (KeyValuePair<Vector,Cell>pair in Map.CellGraph) {	
						CellCreationQueue.Enqueue (pair.Key);
				}
		}

		void SetupSiteToCellPrefabLookupTable ()
		{
				SiteToCellPrefabLookupTable.Clear ();
				for (int i =0; i < CellPrefabs.Count; i++) {
						GameObject cellObj = CellPrefabs [i];
						cell_c comp = cellObj.GetComponent<cell_c> ();
						Cell c = comp.Cell;
						SiteToCellPrefabLookupTable.Add (c.site, i);
				}
		}

		// If your number X falls between A and B, and you would like Y to fall between C and D;
		// Y = (X-A)/(B-A) * (D-C) + C
		public double ReMap (double X, double A, double B, double C, double D)
		{
				return (X - A) / (B - A) * (D - C) + C;
		}

		bool IsInsideCircle (double radius, double centerX, double centerZ, double siteX, double siteZ)
		{
				double dx = centerX - siteX;
				double dy = centerZ - siteZ;
				dx *= dx;
				dy *= dy;
				double distanceSquared = dx + dy;
				double radiusSquared = radius * radius;
				return distanceSquared <= radiusSquared;
		}

		void SetupLandOcean ()
		{
				Debug.Log ("Setting up oceans. . . ");
				for (int i =0; i < CellPrefabs.Count; i++) {
						GameObject cellObj = CellPrefabs [i];
						cell_c comp = cellObj.GetComponent<cell_c> ();
						double siteX = comp.Cell.site [0];
						double siteZ = comp.Cell.site [1];
						double halfMap = Max * .5;
						double quarterMap = Max * .25;
						double radius = quarterMap;
						double centerX = halfMap;
						double centerZ = halfMap;
						// Insert Multiple Circles here to create separation between continents.			

						if (siteX <= halfMap) { // left side
								centerX = quarterMap + (quarterMap * .3);
								if (siteZ <= halfMap) { // left bottom
										centerZ = quarterMap + (quarterMap * .2);
										radius += (quarterMap * .2);
								} else { // left top
										centerZ = halfMap + quarterMap - (quarterMap * .2);
								}
						} else { // right side
								centerX = halfMap + quarterMap - (quarterMap * .2);
								if (siteZ <= halfMap) { // right bottom
										centerZ = quarterMap + (quarterMap * .2);
										radius += (quarterMap * .2);
								} else { // right top
										centerZ = halfMap + quarterMap - (quarterMap * .2);
								}
						}


						if (IsInsideCircle (radius, centerX, centerZ, siteX, siteZ)) {
								double noiseValue = ReMap (Map.perturbedContinentNoise.GetValue (comp.Cell.site.AsVector3 ()), -1, 1, 0, 1);
								if (!comp.Cell.border && noiseValue > .35) {
										comp.Cell.water = false;
										comp.Cell.ocean = false;
										comp.SetCellBiome (eBiome.stone, false);
								}
						}
				}
		}

		void SetupBeaches ()
		{
				Debug.Log ("Setting up beaches. . . ");
				for (int i =0; i < CellPrefabs.Count; i++) {
						cell_c comp = CellPrefabs [i].GetComponent<cell_c> ();
						if (!comp.Cell.ocean && !comp.Cell.border) {
								for (int j = 0; j < comp.Cell.neighborCells.Count; j++) {
										Cell neighborCell = CellPrefabs [SiteToCellPrefabLookupTable [comp.Cell.neighborCells [j]]].GetComponent<cell_c> ().Cell;
										if (neighborCell.ocean) {
												comp.Cell.coast = true;
												comp.SetCellBiome (eBiome.desert, false);
										}
								}
						} 
				}
		}

		void SetupElevation ()
		{
				Debug.Log ("Setting up ElevationMap . . .");

				for (int i =0; i < CellPrefabs.Count; i++) {
						GameObject cellObj = CellPrefabs [i];
						cell_c comp = cellObj.GetComponent<cell_c> ();
						if (!comp.Cell.water && !comp.Cell.border && !comp.Cell.coast) {
								comp.SetCellBiome (GetBiomeFromHeight (comp.Cell.site), false);
						} 
				}
		}

		bool IsOnTheEquator (double siteZ)
		{
				return (siteZ > (Max * .5) - (Max * EquatorLatitudeMin) && siteZ < (Max * .5) + (Max * EquatorLatitudeMax));
		}

		bool IsArtic (double siteZ)
		{
				return (siteZ >= (Max - (Max * ArticLatitude)));
		}

		bool IsAntArtic (double siteZ)
		{
				return (siteZ <= (Max * AntArticLatitude));
		}
	
		eBiome GetBiomeFromHeight (Vector site)
		{
				Vector3 SiteAsVect3 = site.AsVector3 ();

				double height = ReMap (Map.finalTerrainNoise.GetValue (SiteAsVect3), -5, 8, 0, 1);
				if (height <= 0.4) {
						if (IsOnTheEquator (site [1])) {
								return eBiome.desert;
						} else {
								return eBiome.grassland;
						}
				} else if (height <= 0.6) {

						if (IsOnTheEquator (site [1])) {
								return eBiome.temperateDesert;
						} else {
								return eBiome.temperateDeciduousForest;
						}
				} else if (height <= 0.8) {

						if (IsOnTheEquator (site [1])) {
								return eBiome.shrubland;
						} else {
								return eBiome.taiga;
						}
				} else if (height <= 0.9) {

						if (IsOnTheEquator (site [1])) {
								return eBiome.stone;
						} else {
								return eBiome.tundra;
						}
				} else {
						if (IsOnTheEquator (site [1])) {
								return eBiome.scorched;
						} else {
								return eBiome.snow;
						}
				}
		}

		void SetupBiomes ()
		{
				Debug.Log ("Setting up biomes . . .");

				for (int i =0; i < CellPrefabs.Count; i++) {
						GameObject cellObj = CellPrefabs [i];
						cell_c comp = cellObj.GetComponent<cell_c> ();
						if (!comp.Cell.water && !comp.Cell.border) {

								if (IsArtic (comp.Cell.site [1]) || IsAntArtic (comp.Cell.site [1])) {
										comp.SetCellBiome (eBiome.snow, false);
								}
						} 
				}

				MapSetupFinished = true;
		}

		void DrawHeatMap ()
		{
				noise_visualizer_c visualizer = NoiseVisualizerGameObject.GetComponent<noise_visualizer_c> ();
				List<Vector2> borders = new List<Vector2> ();
				for (int x = 0; x < Max; x++) {
						for (int z =0; z<Max; z++) {
								if (Map.CellGraph.ContainsKey (new Vector (x, z))) {
										if (Map.CellGraph [new Vector (x, z)].ocean) {
												visualizer.texture.SetPixel (x, z, Color.blue);
										}
								} else if (Map.baseContinentNoise.GetValue (x, 0, z) < -.05) {
										visualizer.texture.SetPixel (x, z, Color.red);
								} else if (Map.baseContinentNoise.GetValue (x, 0, z) < 0.1) {
										visualizer.texture.SetPixel (x, z, Color.blue);
								} else {
										visualizer.texture.SetPixel (x, z, Color.black);
								}
						}
				}


				visualizer.texture.Apply ();

				// connect texture to material of GameObject this script is attached to
				visualizer.rMesh.GetComponent<Renderer>().material.mainTexture = visualizer.texture;
		}

		IEnumerator CellFactory ()
		{
				while (true) {
						if (CellCreationQueue.Count > 0) {
								for (int i = 0; i < Mathf.Min(CellCreationQueue.Count, 64); i++) {
										if (CellCreationQueue.Count >= i) {
												Vector Site = CellCreationQueue.Dequeue ();
												Cell compCell = Map.CellGraph [Site];
												compCell.water = true;
												compCell.ocean = true;

												GameObject Instance = (GameObject)Instantiate (CellPrefab);
												Instance.transform.parent = transform;
												cell_c cellComponent = Instance.gameObject.GetComponent<cell_c> ();
												cellComponent.Max = Max;
												cellComponent.Cell = compCell;
												cellComponent.SetCellBiome (eBiome.water, false);
												CellPrefabs.Add (Instance);
										}
								}
						}
						yield return null;
				}
		}
	#endregion

	#region Gameplay


		void Update ()
		{
				if (CellFactoryStarted) {
						CellFactoryFinished = (CellCreationQueue.Count == 0); // The game hasn't actually started yet

						if (CellFactoryFinished) { // Wait for the map to finish creating the cells.
								if (!MapSetupFinished) { // The cells are finished but now we need to put data in them.
										SetupSiteToCellPrefabLookupTable ();
										SetupLandOcean ();
										SetupBeaches ();
										SetupElevation ();
										SetupBiomes ();
								} else {  // You are now free to play the game.
										GameStarted = true;
										HandleInput ();
								}
						}
				}
		}

	#region INPUT
		paintbrush pBrush;

		void HandleInput ()
		{
				if (Input.GetButtonDown ("Fire1")) {  // Simple first pass at cell interaction, 

				}
				if (Input.GetMouseButton (2)) {
						CameraXSliderValue -= (Input.GetAxis ("Mouse X") * (CameraZoomSliderValue * MouseSensitivity));
						CameraZSliderValue -= (Input.GetAxis ("Mouse Y") * (CameraZoomSliderValue * MouseSensitivity));
//						Debug.Log (CameraXSliderValue);
				}

				CameraXSliderValue = Mathf.Clamp (CameraXSliderValue, 0, (float)Max);
				CameraZSliderValue = Mathf.Clamp (CameraZSliderValue, 0, (float)Max);

				CameraZoomSliderValue -= (Input.GetAxis ("Wheel ScrollWheel") * 200);
				CameraZoomSliderValue = Mathf.Clamp (CameraZoomSliderValue, 5.0f, (float)Max);


				Vector3 camPos = new Vector3 (CameraXSliderValue, Camera.main.transform.position.y, CameraZSliderValue);
				if (!FirstPerson) {
						Camera.main.transform.position = camPos;
						Camera.main.orthographicSize = CameraZoomSliderValue;
				}
				if (pBrush == null) {
						pBrush = PaintBrushPrefab.GetComponent<paintbrush> ();
				}
				pBrush.IsPainting = Input.GetMouseButton (0);
				pBrush.CurrentBiome = BiomeHelper.GetBiomeFromIndex (SelectedBiomePaintIndex);    //
				PaintBrushPrefab.transform.position = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, pBrush.depth)); 
				PaintBrushPrefab.GetComponent<Renderer>().material = BiomeHelper.GetMaterialFromBiome (pBrush.CurrentBiome);
				PaintBrushPrefab.transform.localScale = new Vector3 (PaintBrushSizeSliderValue, 10.0f, PaintBrushSizeSliderValue);

		}
	#endregion

	#region EXPERIMENTAL

		void PaintCell ()
		{
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) {
						cell_c cellComponet = hit.transform.gameObject.GetComponent<cell_c> ();
						cellComponet.SetCellBiome (BiomeHelper.GetBiomeFromIndex (SelectedBiomePaintIndex), true);
						Debug.Log ("You hit " + cellComponet.Cell.site);
						Debug.Log ("biome:" + cellComponet.Cell.biome);			
						//Instantiate (particle, transform.position, transform.rotation);
				}
		}
	#endregion



	#endregion
}
