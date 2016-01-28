﻿using System;
using System.Diagnostics.Contracts;
using System.Threading.Tasks;

namespace GRaff.Synchronization
{
	[ContractClass(typeof(AsyncOperationContract))]
	/// <summary>
	/// Represents an asynchronous operation that will not return a value.
	/// </summary>
	public interface IAsyncOperation
	{
		// Gets the state of this IAsyncOperation
		AsyncOperationState State { get; }

		// Aborts the IAsyncOperation
		void Abort();

		// Marks this IAsyncOperation as Done
		void Done();

		// Gets whether this IAsyncOperation is Done
		bool IsDone { get; }

		// Blocks until the IAsyncOperation completes
		void Wait();

		// When this IAsyncOperation completes, dispatch the IAsyncOperation generated by the specified function.
		IAsyncOperation ThenRun(Func<IAsyncOperation> action);

		// When this IAsyncOperation completes, dispatch the IAsyncOperation<TNext> generated by the specified function.
		IAsyncOperation<TNext> ThenRun<TNext>(Func<IAsyncOperation<TNext>> action);

		// When this IAsyncOperation completes, execute the specified action immediately.
		IAsyncOperation ThenWait(Action action);

		// When this IAsyncOperation completes, execute the specified function immediately.
		IAsyncOperation<TNext> ThenWait<TNext>(Func<TNext> action);

		// When this IAsyncOperation completes, dispatch the specified action in the next Async step.
		IAsyncOperation Then(Action action);

		// When this IAsyncOperation completes, dispatch the specified function in the next Async step.
		IAsyncOperation<TNext> Then<TNext>(Func<TNext> action);

		// When this IAsyncOperation completes, dispatch the Task generated by the specified function asynchronously (possibly on a different thread).
		IAsyncOperation ThenAsync(Func<Task> action);

		// When this IAsyncOperation completes, dispatch the Task<TNext> generated by the specified function asynchronously (possibly on a different thread).
		IAsyncOperation<TNext> ThenAsync<TNext>(Func<Task<TNext>> action);

		// If a TException would be thrown during dispatching this IAsyncOperation, handle that operation and instead return a success.
		IAsyncOperation Catch<TException>(Action<TException> handler) where TException : Exception;

		// Returns an operation that will resolve if this IAsyncOperation fails with an Exception of the specified type.
		// That operation will resolve after the other continuations have been rejected.
		IAsyncOperation<Exception> Otherwise();

		void Dispatch(object value);
	}

	/// <summary>
 /// Represents an asynchronous operation that will return the specified type.
 /// </summary>
	public interface IAsyncOperation<TPass> : IAsyncOperation
	{
		IAsyncOperation ThenRun(Func<TPass, IAsyncOperation> action);
		IAsyncOperation<TNext> ThenRun<TNext>(Func<TPass, IAsyncOperation<TNext>> action);
		IAsyncOperation ThenWait(Action<TPass> action);
		IAsyncOperation<TNext> ThenWait<TNext>(Func<TPass, TNext> action);
		IAsyncOperation Then(Action<TPass> action);
		IAsyncOperation<TNext> Then<TNext>(Func<TPass, TNext> action);
		IAsyncOperation ThenAsync(Func<TPass, Task> action);
		IAsyncOperation<TNext> ThenAsync<TNext>(Func<TPass, Task<TNext>> action);
		IAsyncOperation<TPass> Catch<TException>(Func<TException, TPass> handler) where TException : Exception;
		new TPass Wait();
	}

	[ContractClassFor(typeof(IAsyncOperation))]
	abstract class AsyncOperationContract : IAsyncOperation
	{
		public bool IsDone => default(bool);

		public AsyncOperationState State => default(AsyncOperationState);

		public void Abort() {}

		public IAsyncOperation Catch<TException>(Action<TException> handler) where TException : Exception
		{
			Contract.Requires(handler != null);
			Contract.Ensures(Contract.Result<IAsyncOperation>() != null);
			return default(IAsyncOperation);
		}

		public void Dispatch(object value) { }

		public void Done() { }

		public IAsyncOperation<Exception> Otherwise()
		{
			Contract.Ensures(Contract.Result<IAsyncOperation<Exception>>() != null);
			return default(IAsyncOperation<Exception>);
		}

		public IAsyncOperation Then(Action action)
		{
			Contract.Requires(action != null);
			Contract.Ensures(Contract.Result<IAsyncOperation>() != null);
			return default(IAsyncOperation);
		}

		public IAsyncOperation<TNext> Then<TNext>(Func<TNext> action)
		{
			Contract.Requires(action != null);
			Contract.Ensures(Contract.Result<IAsyncOperation<TNext>>() != null);
			return default(IAsyncOperation<TNext>);
		}

		public IAsyncOperation ThenAsync(Func<Task> action)
		{
			Contract.Requires(action != null);
			Contract.Ensures(Contract.Result<IAsyncOperation>() != null);
			return default(IAsyncOperation);
		}

		public IAsyncOperation<TNext> ThenAsync<TNext>(Func<Task<TNext>> action)
		{
			Contract.Requires(action != null);
			Contract.Ensures(Contract.Result<IAsyncOperation<TNext>>() != null);
			return default(IAsyncOperation<TNext>);
		}

		public IAsyncOperation ThenRun(Func<IAsyncOperation> action)
		{
			Contract.Requires(action != null);
			Contract.Ensures(Contract.Result<IAsyncOperation>() != null);
			return default(IAsyncOperation);
		}

		public IAsyncOperation<TNext> ThenRun<TNext>(Func<IAsyncOperation<TNext>> action)
		{
			Contract.Requires(action != null);
			Contract.Ensures(Contract.Result<IAsyncOperation<TNext>>() != null);
			return default(IAsyncOperation<TNext>);
		}

		public IAsyncOperation ThenWait(Action action)
		{
			Contract.Requires(action != null);
			Contract.Ensures(Contract.Result<IAsyncOperation>() != null);
			return default(IAsyncOperation);
		}

		public IAsyncOperation<TNext> ThenWait<TNext>(Func<TNext> action)
		{
			Contract.Requires(action != null);
			Contract.Ensures(Contract.Result<IAsyncOperation<TNext>>() != null);
			return default(IAsyncOperation<TNext>);
		}

		public void Wait()
		{
		}
	}
}
