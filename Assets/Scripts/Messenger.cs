// Messenger.cs v1.0 by Magnus Wolffelt, magnus.wolffelt@gmail.com
// Version 1.4 by Julie Iaccarino, biscuitWizard @ github.com
//
// Inspired by and based on Rod Hyde's Messenger:
// http://www.unifycommunity.com/wiki/index.php?title=CSharpMessenger
//
// This is a C# messenger (notification center). It uses delegates
// and generics to provide type-checked messaging between event producers and
// event consumers, without the need for producers or consumers to be aware of
// each other. The major improvement from Hyde's implementation is that
// there is more extensive error detection, preventing silent bugs.
//
// Usage example:
// Messenger<float>.AddListener("myEvent", MyEventHandler);
// ...
// Messenger<float>.Broadcast("myEvent", 1.0f);
//
// Callback example:
// Messenger<float>.AddListener<string>("myEvent", MyEventHandler);
// private string MyEventHandler(float f1) { return "Test " + f1; }
// ...
// Messenger<float>.Broadcast<string>("myEvent", 1.0f, MyEventCallback);
// private void MyEventCallback(string s1) { Debug.Log(s1"); }
//
// If preferred, change DEFAULT_MODE to not require listeners.

using System;
using System.Collections.Generic;
using System.Linq;

public enum Event : int
{
	NameChanged,
	SpeedMultiplierChanged,
	ScoreChanged
}

public enum MessengerMode
{
	DONT_REQUIRE_LISTENER,
	REQUIRE_LISTENER,
}

static internal class MessengerInternal
{
	private static readonly Delegate[] eventTable = new Delegate[Enum.GetNames(typeof(Event)).Length];
	static public MessengerMode DEFAULT_MODE = MessengerMode.REQUIRE_LISTENER;

	static public void AddListener(Event eventType, Delegate callback)
	{
		MessengerInternal.OnListenerAdding(eventType, callback);
		eventTable[(int)eventType] = Delegate.Combine(eventTable[(int)eventType], callback);
	}

	static public void RemoveListener(Event eventType, Delegate handler)
	{
		MessengerInternal.OnListenerRemoving(eventType, handler);
		eventTable[(int)eventType] = Delegate.Remove(eventTable[(int)eventType], handler);
	}

	static public T[] GetInvocationList<T>(Event eventType)
	{
		Delegate d = eventTable[(int)eventType];
		if (d != null)
		{
			try
			{
				return d.GetInvocationList().Cast<T>().ToArray();
			}
			catch
			{
				throw MessengerInternal.CreateBroadcastSignatureException(eventType);
			}
		}
		return new T[0];
	}

	static public void OnListenerAdding(Event eventType, Delegate listenerBeingAdded)
	{
		if (eventTable[(int)eventType] != null)
		{
			eventTable[(int)eventType] = null;
		}

		var d = eventTable[(int)eventType];
		if (d != null && d.GetType() != listenerBeingAdded.GetType())
		{
			throw new ListenerException(string.Format("Attempting to add listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being added has type {2}", eventType, d.GetType().Name, listenerBeingAdded.GetType().Name));
		}
	}

	static public void OnListenerRemoving(Event eventType, Delegate listenerBeingRemoved)
	{
		if (eventTable[(int)eventType] != null)
		{
			var d = eventTable[(int)eventType];

			if (d == null)
			{
				throw new ListenerException(string.Format("Attempting to remove listener with for event type {0} but current listener is null.", eventType));
			}
			else if (d.GetType() != listenerBeingRemoved.GetType())
			{
				throw new ListenerException(string.Format("Attempting to remove listener with inconsistent signature for event type {0}. Current listeners have type {1} and listener being removed has type {2}", eventType, d.GetType().Name, listenerBeingRemoved.GetType().Name));
			}
		}
		else
		{
			throw new ListenerException(string.Format("Attempting to remove listener for type {0} but Messenger doesn't know about this event type.", eventType));
		}
	}

	static public void OnBroadcasting(Event eventType, MessengerMode mode)
	{
		if (mode == MessengerMode.REQUIRE_LISTENER && eventTable[(int)eventType] == null)
		{
			throw new MessengerInternal.BroadcastException(string.Format("Broadcasting message {0} but no listener found.", eventType));
		}
	}

	static public BroadcastException CreateBroadcastSignatureException(Event eventType)
	{
		return new BroadcastException(string.Format("Broadcasting message {0} but listeners have a different signature than the broadcaster.", eventType));
	}

	public class BroadcastException : Exception
	{
		public BroadcastException(string msg)
			: base(msg)
		{
		}
	}

	public class ListenerException : Exception
	{
		public ListenerException(string msg)
			: base(msg)
		{
		}
	}
}

// No parameters
static public class Messenger
{
	static public void AddListener(Event eventType, Action handler)
	{
		MessengerInternal.AddListener(eventType, handler);
	}

	static public void AddListener<TReturn>(Event eventType, Func<TReturn> handler)
	{
		MessengerInternal.AddListener(eventType, handler);
	}

	static public void RemoveListener(Event eventType, Action handler)
	{
		MessengerInternal.RemoveListener(eventType, handler);
	}

	static public void RemoveListener<TReturn>(Event eventType, Func<TReturn> handler)
	{
		MessengerInternal.RemoveListener(eventType, handler);
	}

	static public void Broadcast(Event eventType)
	{
		Broadcast(eventType, MessengerInternal.DEFAULT_MODE);
	}

	static public void Broadcast<TReturn>(Event eventType, Action<TReturn> returnCall)
	{
		Broadcast(eventType, returnCall, MessengerInternal.DEFAULT_MODE);
	}

	static public void Broadcast(Event eventType, MessengerMode mode)
	{
		MessengerInternal.OnBroadcasting(eventType, mode);
		var invocationList = MessengerInternal.GetInvocationList<Action>(eventType);

		foreach (var callback in invocationList)
			callback.Invoke();
	}

	static public void Broadcast<TReturn>(Event eventType, Action<TReturn> returnCall, MessengerMode mode)
	{
		MessengerInternal.OnBroadcasting(eventType, mode);
		var invocationList = MessengerInternal.GetInvocationList<Func<TReturn>>(eventType);

		foreach (var result in invocationList.Select(del => del.Invoke()).Cast<TReturn>())
		{
			returnCall.Invoke(result);
		}
	}
}

// One parameter
static public class Messenger<T>
{
	static public void AddListener(Event eventType, Action<T> handler)
	{
		MessengerInternal.AddListener(eventType, handler);
	}

	static public void AddListener<TReturn>(Event eventType, Func<T, TReturn> handler)
	{
		MessengerInternal.AddListener(eventType, handler);
	}

	static public void RemoveListener(Event eventType, Action<T> handler)
	{
		MessengerInternal.RemoveListener(eventType, handler);
	}

	static public void RemoveListener<TReturn>(Event eventType, Func<T, TReturn> handler)
	{
		MessengerInternal.RemoveListener(eventType, handler);
	}

	static public void Broadcast(Event eventType, T arg1)
	{
		Broadcast(eventType, arg1, MessengerInternal.DEFAULT_MODE);
	}

	static public void Broadcast<TReturn>(Event eventType, T arg1, Action<TReturn> returnCall)
	{
		Broadcast(eventType, arg1, returnCall, MessengerInternal.DEFAULT_MODE);
	}

	static public void Broadcast(Event eventType, T arg1, MessengerMode mode)
	{
		MessengerInternal.OnBroadcasting(eventType, mode);
		var invocationList = MessengerInternal.GetInvocationList<Action<T>>(eventType);

		foreach (var callback in invocationList)
			callback.Invoke(arg1);
	}

	static public void Broadcast<TReturn>(Event eventType, T arg1, Action<TReturn> returnCall, MessengerMode mode)
	{
		MessengerInternal.OnBroadcasting(eventType, mode);
		var invocationList = MessengerInternal.GetInvocationList<Func<T, TReturn>>(eventType);

		foreach (var result in invocationList.Select(del => del.Invoke(arg1)).Cast<TReturn>())
		{
			returnCall.Invoke(result);
		}
	}
}


// Two parameters
static public class Messenger<T, U>
{
	static public void AddListener(Event eventType, Action<T, U> handler)
	{
		MessengerInternal.AddListener(eventType, handler);
	}

	static public void AddListener<TReturn>(Event eventType, Func<T, U, TReturn> handler)
	{
		MessengerInternal.AddListener(eventType, handler);
	}

	static public void RemoveListener(Event eventType, Action<T, U> handler)
	{
		MessengerInternal.RemoveListener(eventType, handler);
	}

	static public void RemoveListener<TReturn>(Event eventType, Func<T, U, TReturn> handler)
	{
		MessengerInternal.RemoveListener(eventType, handler);
	}

	static public void Broadcast(Event eventType, T arg1, U arg2)
	{
		Broadcast(eventType, arg1, arg2, MessengerInternal.DEFAULT_MODE);
	}

	static public void Broadcast<TReturn>(Event eventType, T arg1, U arg2, Action<TReturn> returnCall)
	{
		Broadcast(eventType, arg1, arg2, returnCall, MessengerInternal.DEFAULT_MODE);
	}

	static public void Broadcast(Event eventType, T arg1, U arg2, MessengerMode mode)
	{
		MessengerInternal.OnBroadcasting(eventType, mode);
		var invocationList = MessengerInternal.GetInvocationList<Action<T, U>>(eventType);

		foreach (var callback in invocationList)
			callback.Invoke(arg1, arg2);
	}

	static public void Broadcast<TReturn>(Event eventType, T arg1, U arg2, Action<TReturn> returnCall, MessengerMode mode)
	{
		MessengerInternal.OnBroadcasting(eventType, mode);
		var invocationList = MessengerInternal.GetInvocationList<Func<T, U, TReturn>>(eventType);

		foreach (var result in invocationList.Select(del => del.Invoke(arg1, arg2)).Cast<TReturn>())
		{
			returnCall.Invoke(result);
		}
	}
}


// Three parameters
static public class Messenger<T, U, V>
{
	static public void AddListener(Event eventType, Action<T, U, V> handler)
	{
		MessengerInternal.AddListener(eventType, handler);
	}

	static public void AddListener<TReturn>(Event eventType, Func<T, U, V, TReturn> handler)
	{
		MessengerInternal.AddListener(eventType, handler);
	}

	static public void RemoveListener(Event eventType, Action<T, U, V> handler)
	{
		MessengerInternal.RemoveListener(eventType, handler);
	}

	static public void RemoveListener<TReturn>(Event eventType, Func<T, U, V, TReturn> handler)
	{
		MessengerInternal.RemoveListener(eventType, handler);
	}

	static public void Broadcast(Event eventType, T arg1, U arg2, V arg3)
	{
		Broadcast(eventType, arg1, arg2, arg3, MessengerInternal.DEFAULT_MODE);
	}

	static public void Broadcast<TReturn>(Event eventType, T arg1, U arg2, V arg3, Action<TReturn> returnCall)
	{
		Broadcast(eventType, arg1, arg2, arg3, returnCall, MessengerInternal.DEFAULT_MODE);
	}

	static public void Broadcast(Event eventType, T arg1, U arg2, V arg3, MessengerMode mode)
	{
		MessengerInternal.OnBroadcasting(eventType, mode);
		var invocationList = MessengerInternal.GetInvocationList<Action<T, U, V>>(eventType);

		foreach (var callback in invocationList)
			callback.Invoke(arg1, arg2, arg3);
	}

	static public void Broadcast<TReturn>(Event eventType, T arg1, U arg2, V arg3, Action<TReturn> returnCall, MessengerMode mode)
	{
		MessengerInternal.OnBroadcasting(eventType, mode);
		var invocationList = MessengerInternal.GetInvocationList<Func<T, U, V, TReturn>>(eventType);

		foreach (var result in invocationList.Select(del => del.Invoke(arg1, arg2, arg3)).Cast<TReturn>())
		{
			returnCall.Invoke(result);
		}
	}
}