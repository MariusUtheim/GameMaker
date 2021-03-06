﻿using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace GRaff
{
    public static class Instance
    {
		private static readonly InstanceList _elements = new InstanceList();

		internal static bool NeedsSort { get; set; }

		internal static void Sort()
		{
			if (NeedsSort)
			{
				_elements.Sort();
				NeedsSort = false;
			}
		}

		public static TGameElement Create<TGameElement>(TGameElement instance) where TGameElement : GameElement
		{
            if (!instance.Exists)
            {
                _elements.Add(instance);
                instance.Exists = true;
            }

            return instance;
		}

        /// <summary>
		/// Destroys the instance of this GRaff.GameElement, removing it from the game.
		/// The instance will stop performing automatic actions such as Step and Draw,
		/// but the C# object is not garbage collected while it is still being referenced.
		/// </summary>
        /// <param name="element">The GRaff.GameElement to be destroyed.</param>
        /// <returns>true if the element was destroyed; otherwise, e.g. if the element is already destroyed, returns false</returns>
		public static bool Remove(GameElement instance)
		{
            return _elements.Remove(instance);
        }

		public static IEnumerable<GameElement> All
		{
            get
			{
                foreach (var element in _elements)
                    yield return element;
			}
		}

        public static IEnumerable<GameElement> Where(Func<GameElement, bool> predicate) => All.Where(predicate);
		public static IEnumerable<T> OfType<T>() => All.OfType<T>();
		public static T One<T>() where T : GameElement => All.OfType<T>().FirstOrDefault();
    }

	/// <summary>
	/// Provides static methods to interact with the instances of a specific type.
	/// </summary>
	/// <typeparam name="T">The type of GameObject.</typeparam>
	public static class Instance<T> where T : GameElement
	{
		private static bool _isAbstract;
		private static Func<T>? _parameterlessConstructor;
		private static Func<Point, T>? _locationConstructor;
		private static Func<double, double, T>? _xyConstructor;

		static Instance()
		{
			var type = typeof(T);

			_isAbstract = type.IsAbstract;

			var constructors = type.GetConstructors();
			var parameterTypes = constructors.Select(c => c.GetParameters().Select(p => p.ParameterType)).ToArray();

			var parameterlessMatch = constructors.FirstOrDefault(c => !c.GetParameters().Select(p => p.ParameterType).Any());
			var locationMatch = constructors.FirstOrDefault(c => c.GetParameters().Select(p => p.ParameterType).SequenceEqual(new[] { typeof(Point) }));
			var xyMatch = constructors.FirstOrDefault(c => c.GetParameters().Select(p => p.ParameterType).SequenceEqual(new[] { typeof(double), typeof(double) }));

            // If no suitable constructors are defined, everything is set to null
            if (parameterlessMatch == null && locationMatch == null && xyMatch == null)
                return;

            if (parameterlessMatch != null)
                _parameterlessConstructor = () => (T)parameterlessMatch.Invoke(new object[0]);
            else if (locationMatch != null)
                _parameterlessConstructor = () => (T)locationMatch.Invoke(new object[] { Point.Zero });
            else if (xyMatch != null)
                _parameterlessConstructor = () => (T)xyMatch.Invoke(new object[] { 0, 0 });

            if (locationMatch != null)
                _locationConstructor = location => (T)locationMatch.Invoke(new object[] { location });
            else if (xyMatch != null)
                _locationConstructor = location => (T)xyMatch.Invoke(new object[] { location.X, location.Y });
            else if (parameterlessMatch != null && typeof(GameObject).IsAssignableFrom(typeof(T)))
                _locationConstructor = location => {
                    var obj = (GameObject)parameterlessMatch.Invoke(new object[0]);
                    obj.Location = location;
                    return (obj as T)!;
                };

            if (xyMatch != null)
                _xyConstructor = (x, y) => (T)xyMatch.Invoke(new object[] { x, y });
            else if (locationMatch != null)
                _xyConstructor = (x, y) => (T)locationMatch.Invoke(new object[] { new Point(x, y) });
            else if (parameterlessMatch != null && typeof(GameObject).IsAssignableFrom(typeof(T)))
                _xyConstructor = (x, y) => {
                    var obj = (GameObject)parameterlessMatch.Invoke(new object[0]);
                    obj.Location = (x, y);
                    return (obj as T)!;
                };

		}

		/// <summary>
		/// Creates a new instance of TGameObject.
		/// </summary>
		/// <returns>The created TGameObject.</returns>
		/// <remarks>
		/// If TGameObject has a parameterless constructor, this constructor is used.
		/// Otherwise, if it has a constructor accepting two System.Double structures, this constructor is called with (0.0, 0.0).
		/// Otherwise, if it has a constructor accepting a GRaff.Point, this constructor is called with GRaff.Point.Zero.
		/// If TGameObject has none of these constructors, a System.InvalidOperationException is thrown when Instance´1 is used.
		/// </remarks>
		public static T Create()
		{
			if (_isAbstract)
				throw new InvalidOperationException($"Unable to create instances of the abstract type {nameof(T)}");
            if (_parameterlessConstructor == null)
				throw new InvalidOperationException($"Unable to create instances through {nameof(Instance<T>)}: Type {nameof(T)} must specify a parameterless constructor, a constructor taking a GRaff.Point structure or a constructor taking two System.Double structures.");
			try
			{
				return Instance.Create(_parameterlessConstructor());
			}
			catch (TargetInvocationException ex)
			{
                if (ex.InnerException != null)
    				ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                
                throw;
			}
		}

		/// <summary>
		/// Creates a new instance of TGameObject, using the specified GRaff.Point as the argument.
		/// </summary>
		/// <param name="location">The argument of the called constructor, or the location where the TGameObject will be placed.</param>
		/// <returns>The created TGameObject.</returns>
		/// <remarks>
		/// If TGameObject has a constructor accepting a GRaff.Point, this constructor is called with location.
		/// Otherwise, if it has a constructor accepting two System.Double structures, this constructor is called with (location.X, location.Y).
		/// Otherwise, if it has a parameterless constructor, this constructor is called; then the Location property of the created object is set to location.
		/// If TGameObject has none of these constructors, a System.InvalidOperationException is thrown when Instance´1 is used.</remarks>
		/// </remarks>
		public static T Create(Point location)
		{
			if (_isAbstract)
                throw new InvalidOperationException($"Unable to create instances of the abstract type {nameof(T)}");
			if (_locationConstructor == null)
				throw new InvalidOperationException($"Unable to create instances through {nameof(Instance<T>)}: Type {nameof(T)} must specify a parameterless constructor, a constructor taking a GRaff.Point structure or a constructor taking two System.Double structures.");

			try 
			{
				return Instance.Create(_locationConstructor(location));
			}
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();

                throw;
            }
        }

		/// <summary>
		/// Creates a new instance of TGameObject, using the specified x- and y-coordinates as the arguments.
		/// </summary>
		/// <param name="x">The first argument of the called constructor, or the x-coordinate where the TGameObject will be placed.</param>
		/// <param name="y">The second argument of the called constructor, or the y-coordinate where the TGameObject will be placed.</param>
		/// <returns>The created TGameObject</returns>
        /// <remarks>
		/// If TGameobject has a constructor accepting two System.Double structures, this constructor is called with (x, y).
		/// Otherwise, if TGameObject has a constructor accepting a GRaff.Point, this constructor is called with a new GRaff.Point using the specified (x, y) coordinates.
		/// Otherwise, if it has a parameterless constructor, this constructor is called; then the X property of the created object is set to x, and the Y property is set to y.
		/// If TGameObject has none of these constructors, a System.InvalidOperationException is thrown when Instance´1 is used.
		/// </remarks>
		public static T Create(double x, double y)
		{
			if (_isAbstract)
                throw new InvalidOperationException($"Unable to create instances of the abstract type {nameof(T)}");
			if (_xyConstructor == null)
				throw new InvalidOperationException(string.Format("Unable to create instances through {0}: Type {1} must specify a parameterless constructor, a constructor taking a GRaff.Point structure or a constructor taking two System.Double structures.", typeof(Instance<T>).Name, typeof(T).Name));

			try
            {
                return Instance.Create(_xyConstructor(x, y));
            }
            catch (TargetInvocationException ex)
            {
                if (ex.InnerException != null)
                    ExceptionDispatchInfo.Capture(ex.InnerException).Throw();

                throw;
            }
        }

		public static T _ => Enumerate().FirstOrDefault();

		/// <summary>
		/// Returns all instances of the specified type.
		/// </summary>
		public static IEnumerable<T> Enumerate() => Instance.All.OfType<T>();

		public static bool Any() => Enumerate().Any();
		public static bool Any(Func<T, bool> predicate) => Enumerate().Any(predicate);
		public static bool All(Func<T, bool> predicate) => Enumerate().All(predicate);
		public static IEnumerable<T> Where(Func<T, bool> predicate) => Enumerate().Where(predicate);
		public static IEnumerable<T> Where(Func<T, int, bool> predicate) => Enumerate().Where(predicate);

		/// <summary>
		/// Performs the action to each instance of the specified type.
		/// </summary>
		/// <param name="action">The action to perform</param>
		public static void Do(Action<T> action)
		{
			foreach (var obj in Enumerate())
				action.Invoke(obj);
		}

		public static bool DoOnce(Action<T> action)
		{
			foreach (var obj in Enumerate())
			{
				action.Invoke(obj);
				return true;
			}
			return false;
		}

		public static bool DoOnceIf(Func<T, bool> predicate, Action<T> action)
		{
			foreach (var obj in Where(predicate))
			{
				action.Invoke(obj);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Returns one instance of the specified type.
		/// </summary>
		public static T First() => Enumerate().First();
		public static T First(Func<T, bool> predicate) => Enumerate().First(predicate);
		public static T FirstOrDefault() => Enumerate().FirstOrDefault();
		public static T FirstOrDefault(Func<T, bool> predicate) => Enumerate().FirstOrDefault(predicate);
        public static T Single() => Enumerate().Single();
		public static T Single(Func<T, bool> predicate) => Enumerate().Single(predicate);
    }
}
