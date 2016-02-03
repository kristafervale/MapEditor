using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using BenTools.Mathematics;

namespace KVoxel
{
		public static class math_utilities
		{


				


				public static T ClampAnything<T> (T value, T max, T min)
		where T : System.IComparable<T>
				{     
						T result = value;
						if (value.CompareTo (max) > 0)
								result = max;
						if (value.CompareTo (min) < 0)
								result = min;
						return result;
				}
		
				public static Vector Compute2DPolygonCentroid (List<Vector> vertices)
				{
						Vector centroid = new Vector (0.0, 0.0);
						double signedArea = 0.0;
						double x0 = 0.0; // Current vertex X
						double y0 = 0.0; // Current vertex Y
						double x1 = 0.0; // Next vertex X
						double y1 = 0.0; // Next vertex Y
						double a = 0.0;  // Partial signed area
			
						// For all vertices except last
						int i = 0;
						for (i = 0; i < vertices.Count - 2; ++i) {
								x0 = vertices [i] [0];
								y0 = vertices [i] [1];
								x1 = vertices [i + 1] [0];
								y1 = vertices [i + 1] [1];
								a = x0 * y1 - x1 * y0;
								signedArea += a;
								centroid [0] += (x0 + x1) * a;
								centroid [1] += (y0 + y1) * a;
						}
			
						// Do last vertex
						x0 = vertices [i] [0];
						y0 = vertices [i] [1];
						x1 = vertices [0] [0];
						y1 = vertices [0] [1];
						a = x0 * y1 - x1 * y0;
						signedArea += a;
						centroid [0] += (x0 + x1) * a;
						centroid [1] += (y0 + y1) * a;
			
						signedArea *= 0.5;
						centroid [0] /= (6 * signedArea);
						centroid [1] /= (6 * signedArea);
			
						return centroid;
				}
		}

}
