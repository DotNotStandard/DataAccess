/*
 * Copyright © 2022 DotNotStandard. All rights reserved.
 * 
 * See the LICENSE file in the root of the repo for licensing details.
 * 
 */
using System.Runtime.Serialization;

namespace DotNotStandard.DataAccess.Core
{

	/// <summary>
	/// Custom exception used to represent that the incorrect number of rows were affected by a data access operation
	/// </summary>
    [Serializable]
	public class IncorrectRowCountException : Exception
	{
		public IncorrectRowCountException()
		{
		}

		public IncorrectRowCountException(string message) : base(message)
		{
		}

		public IncorrectRowCountException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected IncorrectRowCountException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}

}