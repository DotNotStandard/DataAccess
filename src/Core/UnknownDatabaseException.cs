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
	/// Custom exception used to represent an unknown exception in the database
	/// </summary>
    [Serializable]
	public class UnknownDatabaseException : Exception
	{
		public UnknownDatabaseException()
		{
		}

		public UnknownDatabaseException(string message) : base(message)
		{
		}

		public UnknownDatabaseException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected UnknownDatabaseException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}

}