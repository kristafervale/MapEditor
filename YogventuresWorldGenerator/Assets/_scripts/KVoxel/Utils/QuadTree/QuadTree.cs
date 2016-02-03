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
		public class QuadTree
		{
				public QuadTreeNode Root;

				public QuadTree (Rect bBox, int maxDepth)
				{
						Root = new QuadTreeNode (bBox, Vector2.zero, maxDepth, 0);
				}
		}
}