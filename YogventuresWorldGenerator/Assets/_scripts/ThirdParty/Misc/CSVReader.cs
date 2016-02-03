/*
	CSVReader by Dock. (24/8/11)
	http://starfruitgames.com
	usage: 
	CSVReader.SplitCsvGrid(textString)
	returns a 2D string array. 
	Drag onto a gameobject for a demo of CSV parsing.
*/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CSVReader : MonoBehaviour
{
		public TextAsset csvFile;
		public Dictionary<int,float[]> ParsedData = new Dictionary<int, float[]> ();

//		void Start ()
//		{
//				ParseFile ();
//		}

		public void ParseFile ()
		{
				ParsedData = ConvertCVSToFloatDictionary (csvFile.text);
				if (ParsedData.Count > 0) {
						//DebugOutputGrid ();
				} else {
						Debug.Log ("Something went wrong parsing the .csv");
				}
		}
		// outputs the content of a 2D array, useful for checking the importer
		public void DebugOutputGrid ()
		{
				foreach (KeyValuePair<int,float[]>pair in ParsedData) {
			string output = "";
						output+=(pair.Key + ":");
						for (int i =0; i < pair.Value.Length; i++) {
				output+= (pair.Value [i]+"|");
						}
			Debug.Log(output);
				}
		}

		// splits a CSV file into a dictionary with key of line number and value list of floats.
		public Dictionary<int,float[]> ConvertCVSToFloatDictionary (string csvText)
		{
				string[] lines = csvText.Split ("\n" [0]); 
				Dictionary<int,float[]> Data = new Dictionary<int, float[]> ();
				for (int i = 0; i < lines.Length; i++) {
						string[] col = lines [i].Split ("," [0]);
						float[] Values = new float[col.Length];
						for (int k = 0; k<col.Length; k++) {
								Values [k] = float.Parse (col [k]);
						}
						Data.Add (i, Values);
				}
				return Data;
		}
}