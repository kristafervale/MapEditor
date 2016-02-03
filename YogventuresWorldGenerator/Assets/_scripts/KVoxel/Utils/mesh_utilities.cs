using UnityEngine;
using System.Collections;

namespace KVoxel
{
		public static class mesh_utilities
		{
				// We need a way to pass in mesh data as a reference and change it in place
				// this will avoid allocating when we update mesh data
				// In order for this to work the mesh must contain the maximum amount of potential data
				// and just zero out unused data.
				// Two possible solutions, depending on if setting mesh.verticies = Vector3[] allocates
				// If it doesn't we'll do it that way allocating one Vector3[] array and changing those in place
				// then updating the mesh to point to the new array whenever a rebuild is required.
				// If it DOES reallocate, we need to see if we can pass mesh.verticies mesh.normals etc as a reference
				// into a method. If we can then we can use the ref modifier to tell mesh to update in place without 
				// reallocating.

		}
}
