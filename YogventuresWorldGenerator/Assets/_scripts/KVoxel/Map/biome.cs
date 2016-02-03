//-----------------------------------------------------------------------
// <copyright file="center.cs" company="Winterkewl Games LLC">
//     Copyright (c) Winterkewl Games LLC.  All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using UnityEngine;
using System;
using System.Collections.Generic;
using BenTools.Mathematics;

namespace KVoxel
{
		public class Biome
		{
				public float Latitude_North_Min;
				public float Latitude_North_Max;
				public float Latitude_South_Min;
				public float Latitude_South_Max;
				public float Inland_Min;
				public float Inland_Max;
				public float Elevation_Min;
				public float Elevation_Max;
				public float Heat_Min;
				public float Heat_Max;
				public float Moisture_Min;
				public float Moisture_Max;
				public float Soil_Nutrients_Min;
				public float Soil_Nutrients_Max;
				public float Mineral_Min;
				public float Mineral_Max;
				public float Insects_Min;
				public float Insects_Max;
				public float Reptiles_Min;
				public float Reptiles_Max;
				public float Amphibians_Min;
				public float Amphibians_Max;
				public float Small_Primates_Min;
				public float Small_Primates_Max;
				public float Large_Primates_Min;
				public float Large_Primates_Max;
				public float Small_Mammals_Min;
				public float Small_Mammals_Max;
				public float Large_Mammals_Min;
				public float Large_Mammals_Max;
				public float Small_Birds_Min;
				public float Small_Birds_Max;
				public float Large_Birds_Min;
				public float Large_Birds_Max;

				public Biome (float[] data)
				{
						Latitude_North_Min = data [0];
						Latitude_North_Max = data [1];
						Latitude_South_Min = data [2];
						Latitude_South_Max = data [3];
						Inland_Min = data [4];
						Inland_Max = data [5];
						Elevation_Min = data [6];
						Elevation_Max = data [7];
						Heat_Min = data [7];
						Heat_Max = data [8];
						Moisture_Min = data [9];
						Moisture_Max = data [10];
						Soil_Nutrients_Min = data [11];
						Soil_Nutrients_Max = data [12];
						Mineral_Min = data [13];
						Mineral_Max = data [14];
						Insects_Min = data [15];
						Insects_Max = data [16];
						Reptiles_Min = data [17];
						Reptiles_Max = data [18];
						Amphibians_Min = data [19];
						Amphibians_Max = data [20];
						Small_Primates_Min = data [21];
						Small_Primates_Max = data [22];
						Large_Primates_Min = data [23];
						Large_Primates_Max = data [24];
						Small_Mammals_Min = data [25];
						Small_Mammals_Max = data [26];
						Large_Mammals_Min = data [27];
						Large_Mammals_Max = data [28];
						Small_Birds_Min = data [29];
						Small_Birds_Max = data [30];
						Large_Birds_Min = data [31];
						Large_Birds_Max = data [32];
				}
		}

		public enum eBiome
		{
				water,
				stone,
				desert,
				snow,
				tundra,
				bare,
				scorched,
				taiga,
				shrubland,
				temperateDesert,
				temperateRainForest,
				temperateDeciduousForest,
				grassland,
				tropicalRainForest,
				tropicalSeasonalForest,
				subtropicalDesert,
				aridPlains,
				aridDesert,
				magical,
		none
		}

		public static class BiomeHelper
		{

				public static List<Color> AllColors = new List<Color> ();
				public static Color cell_color_water;
				public static Color cell_color_stone;
				public static Color cell_color_desert;
				public static Color cell_color_snow;
				public static Color cell_color_tundra;
				public static Color cell_color_bare;
				public static Color cell_color_scorched;
				public static Color cell_color_taiga;
				public static Color cell_color_shrubland;
				public static Color cell_color_temperateDesert;
				public static Color cell_color_temperateRainForest;
				public static Color cell_color_temperateDeciduousForest;
				public static Color cell_color_grassland;
				public static Color cell_color_tropicalRainForest;
				public static Color cell_color_tropicalSeasonalForest;
				public static Color cell_color_subtropicalDesert;
				public static Color cell_color_aridPlains;
				public static Color cell_color_aridDesert;
				public static Color cell_color_magical;
				public static List<Material> AllMaterials = new List<Material> ();
				public static Material cell_material_water;
				public static Material cell_material_stone;
				public static Material cell_material_desert;
				public static Material cell_material_snow;
				public static Material cell_material_tundra;
				public static Material cell_material_bare;
				public static Material cell_material_scorched;
				public static Material cell_material_taiga;
				public static Material cell_material_shrubland;
				public static Material cell_material_temperateDesert;
				public static Material cell_material_temperateRainForest;
				public static Material cell_material_temperateDeciduousForest;
				public static Material cell_material_grassland;
				public static Material cell_material_tropicalRainForest;
				public static Material cell_material_tropicalSeasonalForest;
				public static Material cell_material_subtropicalDesert;
				public static Material cell_material_aridPlains;
				public static Material cell_material_aridDesert;
				public static Material cell_material_magical;
				public static System.Random rand;
				public static GUIContent[] BiomeGuiContent;
				public static string[] BiomeStringNamesArray;

				public static GUIContent[] GetBiomeGuiContent ()
				{
						return BiomeGuiContent;
				}

				public static void SetupColors ()
				{
						rand = new System.Random ();
						cell_color_water = new Color (0.102f, 0.31f, 0.722f);
						cell_color_stone = new Color (0.25f, 0.25f, 0.25f);
						cell_color_desert = new Color (0.941f, 0.941f, 0.078f);
						cell_color_snow = new Color (1f, 1f, 1f);
						cell_color_tundra = new Color (0.631f, 0.631f, 0.631f);
						cell_color_bare = new Color (0.243f, 0.125f, 0f);
						cell_color_scorched = new Color (0.722f, 0.02f, 0.02f);
						cell_color_taiga = new Color (0.38f, 0.71f, 0.286f);
						cell_color_shrubland = new Color (0.231f, 0.243f, 0.055f);
						cell_color_temperateDesert = new Color (0.384f, 0.357f, 0.024f);
						cell_color_temperateRainForest = new Color (0.063f, 0.227f, 0f);
						cell_color_temperateDeciduousForest = new Color (0.502f, 0.259f, 0f);
						cell_color_grassland = new Color (0.047f, 0.514f, 0.224f);
						cell_color_tropicalRainForest = new Color (0.02f, 0.075f, 0f);
						cell_color_tropicalSeasonalForest = new Color (0.78f, 0.498f, 0f);
						cell_color_subtropicalDesert = new Color (0.898f, 0.918f, 0.553f);
						cell_color_aridPlains = new Color (0.867f, 0.69f, 0.69f);
						cell_color_aridDesert = new Color (0.278f, 0.341f, 0.345f);
						cell_color_magical = new Color (0.776f, 0f, 0.859f);

						cell_material_water = new Material (Shader.Find ("Diffuse"));
						cell_material_stone = new Material (Shader.Find ("Diffuse"));
						cell_material_desert = new Material (Shader.Find ("Diffuse"));
						cell_material_snow = new Material (Shader.Find ("Diffuse"));
						cell_material_tundra = new Material (Shader.Find ("Diffuse"));
						cell_material_bare = new Material (Shader.Find ("Diffuse"));
						cell_material_scorched = new Material (Shader.Find ("Diffuse"));
						cell_material_taiga = new Material (Shader.Find ("Diffuse"));
						cell_material_shrubland = new Material (Shader.Find ("Diffuse"));
						cell_material_temperateDesert = new Material (Shader.Find ("Diffuse"));
						cell_material_temperateRainForest = new Material (Shader.Find ("Diffuse"));
						cell_material_temperateDeciduousForest = new Material (Shader.Find ("Diffuse"));
						cell_material_grassland = new Material (Shader.Find ("Diffuse"));
						cell_material_tropicalRainForest = new Material (Shader.Find ("Diffuse"));
						cell_material_tropicalSeasonalForest = new Material (Shader.Find ("Diffuse"));
						cell_material_subtropicalDesert = new Material (Shader.Find ("Diffuse"));
						cell_material_aridPlains = new Material (Shader.Find ("Diffuse"));
						cell_material_aridDesert = new Material (Shader.Find ("Diffuse"));
						cell_material_magical = new Material (Shader.Find ("Diffuse"));

						cell_material_water.color = cell_color_water;
						cell_material_desert.color = cell_color_desert;
						cell_material_stone.color = cell_color_stone;
						cell_material_tundra.color = cell_color_tundra;
						cell_material_snow.color = cell_color_snow;
						cell_material_bare.color = cell_color_bare;
						cell_material_scorched.color = cell_color_scorched;
						cell_material_taiga.color = cell_color_taiga;
						cell_material_shrubland.color = cell_color_shrubland;
						cell_material_temperateDesert.color = cell_color_temperateDesert;
						cell_material_temperateRainForest.color = cell_color_temperateRainForest;
						cell_material_temperateDeciduousForest.color = cell_color_temperateDeciduousForest;
						cell_material_grassland.color = cell_color_grassland;
						cell_material_tropicalRainForest.color = cell_color_tropicalRainForest;
						cell_material_tropicalSeasonalForest.color = cell_color_tropicalSeasonalForest;
						cell_material_subtropicalDesert.color = cell_color_subtropicalDesert;
						cell_material_aridPlains.color = cell_color_aridPlains;
						cell_material_aridDesert.color = cell_color_aridDesert;
						cell_material_magical.color = cell_color_magical;

						AllMaterials.Clear ();
						AllMaterials.Add (cell_material_water);
						AllMaterials.Add (cell_material_stone);
						AllMaterials.Add (cell_material_desert);
						AllMaterials.Add (cell_material_snow);
						AllMaterials.Add (cell_material_tundra);
						AllMaterials.Add (cell_material_bare);
						AllMaterials.Add (cell_material_scorched);
						AllMaterials.Add (cell_material_taiga);
						AllMaterials.Add (cell_material_shrubland);
						AllMaterials.Add (cell_material_temperateDesert);
						AllMaterials.Add (cell_material_temperateRainForest);
						AllMaterials.Add (cell_material_temperateDeciduousForest);
						AllMaterials.Add (cell_material_grassland);
						AllMaterials.Add (cell_material_tropicalRainForest);
						AllMaterials.Add (cell_material_tropicalSeasonalForest);
						AllMaterials.Add (cell_material_subtropicalDesert);
						AllMaterials.Add (cell_material_aridPlains);
						AllMaterials.Add (cell_material_aridDesert);
						AllMaterials.Add (cell_material_magical);

						AllColors.Clear ();
						AllColors.Add (cell_color_water);
						AllColors.Add (cell_color_stone);
						AllColors.Add (cell_color_desert);
						AllColors.Add (cell_color_snow);
						AllColors.Add (cell_color_tundra);
						AllColors.Add (cell_color_bare);
						AllColors.Add (cell_color_scorched);
						AllColors.Add (cell_color_taiga);
						AllColors.Add (cell_color_shrubland);
						AllColors.Add (cell_color_temperateDesert);
						AllColors.Add (cell_color_temperateRainForest);
						AllColors.Add (cell_color_temperateDeciduousForest);
						AllColors.Add (cell_color_grassland);
						AllColors.Add (cell_color_tropicalRainForest);
						AllColors.Add (cell_color_tropicalSeasonalForest);
						AllColors.Add (cell_color_subtropicalDesert);
						AllColors.Add (cell_color_aridPlains);
						AllColors.Add (cell_color_aridDesert);
						AllColors.Add (cell_color_magical);

						
						BiomeStringNamesArray = new string[19];
						BiomeStringNamesArray [0] = "Water";
						BiomeStringNamesArray [1] = "Stone";
						BiomeStringNamesArray [2] = "Desert";
						BiomeStringNamesArray [3] = "Snow";
						BiomeStringNamesArray [4] = "Tundra";
						BiomeStringNamesArray [5] = "Bare";
						BiomeStringNamesArray [6] = "Scorched";
						BiomeStringNamesArray [7] = "Taiga";
						BiomeStringNamesArray [8] = "Shrubland";
						BiomeStringNamesArray [9] = "Temperate Desert";
						BiomeStringNamesArray [10] = "Temperate Rain Forest";
						BiomeStringNamesArray [11] = "Temperate Deciduous Forest";
						BiomeStringNamesArray [12] = "grassland";
						BiomeStringNamesArray [13] = "Tropical Rain Forest";
						BiomeStringNamesArray [14] = "Tropical Seasonal Forest";
						BiomeStringNamesArray [15] = "Subtropical Desert";
						BiomeStringNamesArray [16] = "AridPlains";
						BiomeStringNamesArray [17] = "AridDesert";
						BiomeStringNamesArray [18] = "Magical";

						BiomeGuiContent = new GUIContent[19];
						BiomeGuiContent [0] = new GUIContent (BiomeStringNamesArray [0]);
						BiomeGuiContent [1] = new GUIContent (BiomeStringNamesArray [1]);
						BiomeGuiContent [2] = new GUIContent (BiomeStringNamesArray [2]);
						BiomeGuiContent [3] = new GUIContent (BiomeStringNamesArray [3]);
						BiomeGuiContent [4] = new GUIContent (BiomeStringNamesArray [4]);
						BiomeGuiContent [5] = new GUIContent (BiomeStringNamesArray [5]);
						BiomeGuiContent [6] = new GUIContent (BiomeStringNamesArray [6]);
						BiomeGuiContent [7] = new GUIContent (BiomeStringNamesArray [7]);
						BiomeGuiContent [8] = new GUIContent (BiomeStringNamesArray [8]);
						BiomeGuiContent [9] = new GUIContent (BiomeStringNamesArray [9]);
						BiomeGuiContent [10] = new GUIContent (BiomeStringNamesArray [10]);
						BiomeGuiContent [11] = new GUIContent (BiomeStringNamesArray [11]);
						BiomeGuiContent [12] = new GUIContent (BiomeStringNamesArray [12]);
						BiomeGuiContent [13] = new GUIContent (BiomeStringNamesArray [13]);
						BiomeGuiContent [14] = new GUIContent (BiomeStringNamesArray [14]);
						BiomeGuiContent [15] = new GUIContent (BiomeStringNamesArray [15]);
						BiomeGuiContent [16] = new GUIContent (BiomeStringNamesArray [16]);
						BiomeGuiContent [17] = new GUIContent (BiomeStringNamesArray [17]);
						BiomeGuiContent [18] = new GUIContent (BiomeStringNamesArray [18]);
			
				}

				public static Material GetMaterialFromBiome (eBiome b)
				{
						Material m;
						switch (b) {
						case eBiome.water:
								m = cell_material_water;
								break;
						case eBiome.stone:
								m = cell_material_stone;
								break;
						case eBiome.desert:
								m = cell_material_desert;
								break;
						case eBiome.snow:
								m = cell_material_snow;		
								break;
						case eBiome.tundra:
								m = cell_material_tundra;
								break;
						case eBiome.bare:
								m = cell_material_bare;
								break;
						case eBiome.scorched:
								m = cell_material_scorched;
								break;
						case eBiome.taiga:
								m = cell_material_taiga;
								break;
						case eBiome.shrubland:
								m = cell_material_shrubland;
								break;
						case eBiome.temperateDesert:
								m = cell_material_temperateDesert;
								break;
						case eBiome.temperateRainForest:
								m = cell_material_temperateRainForest;
								break;
						case eBiome.temperateDeciduousForest:
								m = cell_material_temperateDeciduousForest;
								break;
						case eBiome.grassland:
								m = cell_material_grassland;
								break;
						case eBiome.tropicalRainForest:
								m = cell_material_tropicalRainForest;
								break;
						case eBiome.tropicalSeasonalForest:
								m = cell_material_tropicalSeasonalForest;
								break;
						case eBiome.subtropicalDesert:
								m = cell_material_subtropicalDesert;
								break;
						case eBiome.aridPlains:
								m = cell_material_aridPlains;
								break;
						case eBiome.aridDesert:
								m = cell_material_aridDesert;
								break;
						case eBiome.magical:
								m = cell_material_magical;
								break;
						default:
								m = cell_material_water;
								break;
						}
			
						return m;
				}

				public static Color GetColorFromBiome (eBiome b)
				{
						Color c;
						switch (b) {
						case eBiome.water:
								c = cell_color_water;
								break;
						case eBiome.stone:
								c = cell_color_stone;
								break;
						case eBiome.desert:
								c = cell_color_desert;
								break;
						case eBiome.snow:
								c = cell_color_snow;		
								break;
						case eBiome.tundra:
								c = cell_color_tundra;
								break;
						case eBiome.bare:
								c = cell_color_bare;
								break;
						case eBiome.scorched:
								c = cell_color_scorched;
								break;
						case eBiome.taiga:
								c = cell_color_taiga;
								break;
						case eBiome.shrubland:
								c = cell_color_shrubland;
								break;
						case eBiome.temperateDesert:
								c = cell_color_temperateDesert;
								break;
						case eBiome.temperateRainForest:
								c = cell_color_temperateRainForest;
								break;
						case eBiome.temperateDeciduousForest:
								c = cell_color_temperateDeciduousForest;
								break;
						case eBiome.grassland:
								c = cell_color_grassland;
								break;
						case eBiome.tropicalRainForest:
								c = cell_color_tropicalRainForest;
								break;
						case eBiome.tropicalSeasonalForest:
								c = cell_color_tropicalSeasonalForest;
								break;
						case eBiome.subtropicalDesert:
								c = cell_color_subtropicalDesert;
								break;
						case eBiome.aridPlains:
								c = cell_color_aridPlains;
								break;
						case eBiome.aridDesert:
								c = cell_color_aridDesert;
								break;
						case eBiome.magical:
								c = cell_color_magical;
								break;
						default:
								c = Color.cyan;
								break;
						}

						return c;
				}

				public static eBiome GetBiomeFromIndex (int index)
				{
						eBiome b;
						switch (index) {
						case 0:
								b = eBiome.water;
								break;
						case 1:
								b = eBiome.stone;
								break;
						case 2:
								b = eBiome.desert;
								break;
						case 3:
								b = eBiome.snow;		
								break;
						case 4:
								b = eBiome.tundra;
								break;
						case 5:
								b = eBiome.bare;
								break;
						case 6:
								b = eBiome.scorched;
								break;
						case 7:
								b = eBiome.taiga;
								break;
						case 8:
								b = eBiome.shrubland;
								break;
						case 9:
								b = eBiome.temperateDesert;
								break;
						case 10:
								b = eBiome.temperateRainForest;
								break;
						case 11:
								b = eBiome.temperateDeciduousForest;
								break;
						case 12:
								b = eBiome.grassland;
								break;
						case 13:
								b = eBiome.tropicalRainForest;
								break;
						case 14:
								b = eBiome.tropicalSeasonalForest;
								break;
						case 15:
								b = eBiome.subtropicalDesert;
								break;
						case 16:
								b = eBiome.aridPlains;
								break;
						case 17:
								b = eBiome.aridDesert;
								break;
						case 18:
								b = eBiome.magical;
								break;
						default:
								b = eBiome.water;
								break;
						}
			
						return b;
				}
		
				public static Color GetRandomColor ()
				{
						return AllColors [rand.Next (0, AllColors.Count)];
				}

				public static Material GetRandomMaterial ()
				{
						return AllMaterials [rand.Next (0, AllMaterials.Count)];
				}

				public static eBiome GetRandomBiome ()
				{
						int maxBiomeIndex = System.Enum.GetValues (typeof(KVoxel.eBiome)).Length;
						return (eBiome)rand.Next (0, maxBiomeIndex);
				}

		}
}
