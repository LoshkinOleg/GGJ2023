using System.Collections.Generic;
using UnityEngine;

public class EventBase<T> : ScriptableObject
{
	protected readonly List<T> _listeners = new List<T>();
	protected readonly List<T> _toRemove = new List<T>();

	private void OnDisable()
	{
		Clear();
	}

	public void AddListener(T listener)
	{
		if (_toRemove.Contains(listener))
		{
			_toRemove.Remove(listener);
			return;
		}

		if (!_listeners.Contains(listener))
		{
			_listeners.Add(listener);
		}
	}

	public void RemoveListener(T listener)
	{
		if (_listeners.Contains(listener))
		{
			if (!_toRemove.Contains(listener))
			{
				_toRemove.Add(listener);
			}
		}
	}

	public void Clear()
	{
		_listeners.Clear();
		_toRemove.Clear();
	}

	public void RemoveQueuedListeners()
	{
		for (int i = 0; i < _toRemove.Count; i++)
		{
			_listeners.Remove(_toRemove[i]);
		}
		_toRemove.Clear();
	}

	protected bool CanRaise()
	{
		RemoveQueuedListeners();
		return true;
	}
}

public abstract class EventRaisable<T> : EventBase<System.Action<T>>
{
	public void Raise(T value)
	{
		if (CanRaise() == false)
		{
			return;
		}

		for (int i = 0; i < _listeners.Count; i++)
		{
			_listeners[i].Invoke(value);
		}
	}
}

