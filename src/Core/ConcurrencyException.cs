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
	/// Custom exception used to represent a concurrency exception
	/// </summary>
	[Serializable]
	public class ConcurrencyException : Exception
	{
		public ConcurrencyException()
		{
		}

		public ConcurrencyException(string message) : base(message)
		{
		}

		public ConcurrencyException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected ConcurrencyException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}

}