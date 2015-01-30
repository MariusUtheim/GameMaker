﻿using System;
using GRaff;
using GRaff.Synchronization;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace GameMaker.UnitTesting
{
	[TestClass]
	public class AsyncTest
	{
		
		[TestMethod]
		public void Async_Properties()
		{
			IAsyncOperation operation;

			operation = Async.Operation();
			Assert.AreEqual(AsyncOperationState.Completed, operation.State);

			operation = Async.Run(() => { });
			Assert.AreEqual(AsyncOperationState.Dispatched, operation.State);

			Async.HandleEvents();
			Assert.AreEqual(AsyncOperationState.Completed, operation.State);

			operation.Abort();
			Assert.AreEqual(AsyncOperationState.Aborted, operation.State);

			operation = Async.Run(() => { }).ThenSync(() => { });
			Assert.AreEqual(AsyncOperationState.Initial, operation.State);

			operation = Async.Run(() =>  { throw new Exception("Error"); });
			Async.HandleEvents();
			Assert.AreEqual(AsyncOperationState.Failed, operation.State);
		}
		
		[TestMethod]
		public void Async_ThenSync()
		{
			IAsyncOperation operation;
			int count = 0;

			operation = Async.Operation();
			for (int i = 0; i < 10; i++)
				operation = operation.ThenSync(() => { count++; });

			for (int i = 0; i < 10; i++)
			{
				Assert.AreEqual(i, count);
				Async.HandleEvents();
			}
		}

		[TestMethod]
		public void Async_ThenPasses()
		{
			IAsyncOperation<int> operation;

			operation = Async.Run(() => 0);
			Async.HandleEvents();

			for (int i = 0; i < 10; i++)
				operation = operation.ThenSync(count => count + 1);

			for (int i = 0; i < 10; i++)
				Async.HandleEvents();

			Assert.AreEqual(AsyncOperationState.Completed, operation.State);
			Assert.AreEqual(10, operation.Wait());
		}
		
		[TestMethod]
		public void Async_MultipleThen()
		{
			IAsyncOperation operation = Async.Operation();


			int count = 0;
			Func<int, Action> incrementBy = i => (() => count += i);
			for (int i = 0; i < 10; i++)
			{
				operation.ThenSync(incrementBy(i));
				operation = operation.ThenSync(incrementBy(i));
			}

			int expected = 0;
			for (int i = 0; i < 10; i++)
			{
				Assert.AreEqual(expected, count);

				Async.HandleEvents();
				expected += 2 * i;
			}
		}

		[TestMethod]
		public void Async_Then()
		{
			IAsyncOperation<int> operation = Async.Run(() => 0);
			Async.HandleEvents();

			for (int i = 0; i < 10; i++)
				operation = operation.Then(count => Async.Run(() => count + 1));

			for (int i = 0; i < 10; i++)
				Async.HandleEvents();

			Assert.AreEqual(AsyncOperationState.Completed, operation.State);
			Assert.AreEqual(10, operation.Wait());
		}

		[TestMethod]
		public void Async_ThenWait()
		{
			int count = 0;
			Async.Run(() => { count += 1; }).ThenWait(() => { count += 1; }).Done();

			Assert.AreEqual(0, count);
			Async.HandleEvents();
			Assert.AreEqual(2, count);
		}


		[TestMethod]
		public void Async_Wait()
		{
			IAsyncOperation operation = Async.Operation();
			int count = 0;
			for (int i = 0; i < 10; i++)
				operation = operation.ThenSync(() => { count++; });
			
			operation.Wait();
			Assert.AreEqual(10, count);
		}


		[TestMethod]
		[ExpectedException(typeof(AsyncException))]
		public void Async_Done()
		{
			IAsyncOperation operation = Async.Run(() => { throw new Exception("Error"); });

			Assert.IsFalse(operation.IsDone);
			operation.Done();
			Assert.IsTrue(operation.IsDone);
			Async.HandleEvents();

			try
			{
				Async.HandleEvents();
			}
			catch (AsyncException ex)
			{
				Assert.AreEqual(typeof(Exception), ex.InnerException.GetType());
				Assert.AreEqual("Error", ex.InnerException.Message);
				throw;
			}
		}

		[TestMethod]
		public void Async_Catch()
		{
			IAsyncOperation operation;
			bool caughtException;
			bool finished;

			// Exceptions get caught
			caughtException = finished = false;
			operation = Async
				.Run(() => { throw new Exception("Error"); })
				.Catch<Exception>(ex => { caughtException = true; })
				.ThenSync(() => { finished = true; });
			Async.HandleEvents();
			Async.HandleEvents();
			Assert.IsTrue(caughtException);
			Assert.IsTrue(finished);

			// An early exception might skip some work before it gets caught.
			caughtException = finished = false;
			operation = Async
				.Run(() => { throw new Exception("Error"); })
				.ThenSync(() => { finished = true; })
				.Catch<Exception>(ex => caughtException = true);
			Async.HandleEvents();
			Async.HandleEvents();
			Assert.IsTrue(caughtException);
			Assert.IsFalse(finished);

			// Subclasses of exceptions will be caught, superclasses will not.
			caughtException = finished = false;
			operation = Async
				.Run(() => { throw new ArithmeticException("Error"); })
				.Catch<DivideByZeroException>(ex => { caughtException = true; })
				.ThenSync(() => { finished = true; })
				.Catch<Exception>(ex => { caughtException = true; });
			Async.HandleEvents();
			Async.HandleEvents();
			Assert.IsTrue(caughtException);
			Assert.IsFalse(finished);
		}

		[TestMethod]
		[ExpectedException(typeof(InvalidOperationException))]
		public void Async_Deferred()
		{
			var completed = false;
			var deferredVoid = new Deferred();

			deferredVoid.Operation.ThenWait(() => completed = true);

			Assert.IsFalse(completed);
			Assert.IsFalse(deferredVoid.IsResolved);
			deferredVoid.Accept();
			Assert.IsTrue(completed);
			Assert.IsTrue(deferredVoid.IsResolved);


			var deferredInt = new Deferred<int>();
			int result = 0;
			deferredInt.Operation.ThenWait(i => result = i);

			Assert.AreEqual(0, result);
			Assert.IsFalse(deferredInt.IsResolved);
			deferredInt.Accept(10);
			Assert.AreEqual(10, result);
			Assert.IsTrue(deferredInt.IsResolved);


			deferredInt = new Deferred<int>();
			result = 0;
			deferredInt.Operation
				.Catch<ArithmeticException>(ex => -1)
				.ThenWait(i => { result = i; });

			deferredInt.Reject(new DivideByZeroException());
			Assert.AreEqual(-1, result);


			deferredInt.Accept(0); // Throw InvalidOperationException
		}
	}
}
