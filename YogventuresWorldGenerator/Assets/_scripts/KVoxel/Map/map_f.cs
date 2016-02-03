using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

using BenTools;
using BenTools.Data;
using BenTools.Mathematics;

using KVoxel;

public class map_f : MonoBehaviour
{
	
		public int ReadSeed;
		public int ReadNumberOfSites;
		public double ReadContinentFrequency;
		public double ReadBiomeFrequency;
		public int ReadOctaves;
		public double ReadLacunarity;
		public double ReadPersistence;
		public Dictionary<Vector,int>ReadCellOverrides = new Dictionary<Vector, int> ();

		public void WriteBinaryFileToDisk (Map_c Map)
		{
				string FilePath = Application.persistentDataPath + "/Saved Games/" + Map.MapNameString;
				string FileName = FilePath + "/" + Map.MapNameString + ".map";
				FileMode mode = FileMode.Create;
				FileStream filestream = new FileStream (FileName, mode);
				using (BinaryWriter sWriter = new BinaryWriter(filestream)) {
						//Map Info;
						sWriter.Write (Map.ParsedSeed);
						sWriter.Write (Map.ParsedNumberOfSites);
						sWriter.Write (Map.ParsedContinentFrequency);
						sWriter.Write (Map.ParsedBiomeFrequency);
						sWriter.Write (Map.ParsedOctaves);
						sWriter.Write (Map.ParsedLacunarity);
						sWriter.Write (Map.ParsedPersistence);

						Dictionary<Vector, int> cellOverrides = new Dictionary<Vector, int> ();
						// Cell Override;
						foreach (GameObject CellObj in Map.CellPrefabs) {
								cell_c cellComponent = CellObj.GetComponent<cell_c> ();
								if (cellComponent.Cell.BiomeOverride) {
										cellOverrides.Add (cellComponent.Cell.site, (int)cellComponent.Cell.biome);
								}
						}

						if (cellOverrides.Count > 0) {
								sWriter.Write (cellOverrides.Count);
								foreach (KeyValuePair<Vector,int>pair in cellOverrides) {
										sWriter.Write (pair.Key [0]);
										sWriter.Write (pair.Key [1]);
										sWriter.Write (pair.Value);
								}
						} else {
								sWriter.Write (0);
						}

						sWriter.Close ();
				}
		}

		public void ReadBinaryFileFromDisk (string MapName)
		{
				string FilePath = Application.persistentDataPath + "/Saved Games/" + MapName;
				string FileName = FilePath + "/" + MapName + ".map";

				if (System.IO.File.Exists (FileName)) {
						FileMode mode = FileMode.Open;
						using (BinaryReader reader = new BinaryReader(File.Open(FileName, mode))) {
								try {
										// Reset the position in the stream to zero.
										reader.BaseStream.Seek (0, SeekOrigin.Begin);
										ReadSeed = reader.ReadInt32 ();
										ReadNumberOfSites = reader.ReadInt32 ();
										ReadContinentFrequency = reader.ReadDouble ();
										ReadBiomeFrequency = reader.ReadDouble ();
										ReadOctaves = reader.ReadInt32 ();
										ReadLacunarity = reader.ReadDouble ();
										ReadPersistence = reader.ReadDouble ();

										ReadCellOverrides.Clear ();
										int counter = reader.ReadInt32 ();
							
										for (int i = 0; i < counter; i++) {
												Vector site = new Vector ();
												site [0] = reader.ReadDouble ();
												site [1] = reader.ReadDouble ();
												int biomeOverride = reader.ReadInt32 ();
												ReadCellOverrides.Add (site, biomeOverride);
										}

								} catch (EndOfStreamException e) {
										//Debug.Log(e.GetType().Name + " caught and ignored. Using default values.");
								} finally {
										reader.Close ();
								}
						}
				}
		}

		// Use this for initialization
		void Start ()
		{

		}
	
		// Update is called once per frame
		void Update ()
		{
	
		}
}
