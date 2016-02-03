using UnityEngine;
using System;

namespace KVoxel
{
		namespace Exceptions
		{
				[Serializable()]
				public class GameFailedToStartException : System.Exception
				{
						public GameFailedToStartException () : base()
						{
						}

						public GameFailedToStartException (string message) : base(message)
						{
						}

						public GameFailedToStartException (string message, System.Exception inner) : base(message, inner)
						{
						}
		
						// A constructor is needed for serialization when an 
						// exception propagates from a remoting server to the client.  
						protected GameFailedToStartException (System.Runtime.Serialization.SerializationInfo info,
		                                     System.Runtime.Serialization.StreamingContext context)
						{
						}
				}

		}	
}
