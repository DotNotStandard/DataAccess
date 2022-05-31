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
	/// Custom exception used to represent an unknown return value from a database
	/// </summary>
    [Serializable]
	public class UnknownReturnValueException : Exception
	{
		public UnknownReturnValueException()
		{
		}

		public UnknownReturnValueException(string message) : base(message)
		{
		}

		public UnknownReturnValueException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected UnknownReturnValueException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}

}