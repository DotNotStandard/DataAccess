/*
 * Copyright © 2022 DotNotStandard. All rights reserved.
 * 
 * See the LICENSE file in the root of the repo for licensing details.
 * 
 */
using System.Transactions;

namespace DotNotStandard.DataAccess.Core
{

    public static class TransactionScopeFactory
	{

		public static TransactionScope CreateRootReadCommittedTransaction()
		{
			return CreateReadCommittedTransaction(TransactionScopeOption.RequiresNew);
		}

		public static TransactionScope CreateSuppressedReadCommittedTransaction()
		{
			return CreateReadCommittedTransaction(TransactionScopeOption.Suppress);

		}

		public static TransactionScope CreateReadCommittedTransaction(TransactionScopeOption scopeOption)
		{
			TransactionOptions options = new TransactionOptions();

			options.IsolationLevel = IsolationLevel.ReadCommitted;
			options.Timeout = TimeSpan.FromSeconds(60);
			return new TransactionScope(scopeOption, options, TransactionScopeAsyncFlowOption.Enabled);
		}

	}

}
