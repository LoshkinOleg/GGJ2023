using System.Collections.Generic;
using UnityEngine;

public class Pool<T> : IPool<T> where T : Object, IPoolableItem
{
	private readonly Queue<T> _available = new Queue<T>();
	private readonly List<T> _inUse = new List<T>();
	private readonly T _blueprint = null;
	protected Transform _parent = null;

	public bool AutoAddInstances { get; set; }

	public Pool(bool autoAddInstances, T blueprint, Transform parent = null)
	{
		AutoAddInstances = autoAddInstances;
		_blueprint = blueprint;
		_parent = parent;
	}

	public void GenerateAvailableInstances(int amount)
	{
		if (amount <= 0 || _available.Count > 0)
		{
			return;
		}

		for (int i = amount; i > 0; i--)
		{
			_available.Enqueue(CreateInstance());
		}
	}

	public void Clear()
	{
		while (_available.Count > 0)
		{
			DismissInstance(_available.Dequeue());
		}
		_available.Clear();
		for (int i = 0; i < _inUse.Count; i++)
		{
			DismissInstance(_inUse[i]);
		}
		_inUse.Clear();
	}

	public T GetInstance()
	{
		T toReturn = default;
		if (_available.Count > 0)
		{
			toReturn = _available.Dequeue();
		}
		else if (AutoAddInstances)
		{
			toReturn = CreateInstance();
		}

		if (toReturn != null)
		{
			_inUse.Add(toReturn);
		}

		return toReturn;
	}

	public bool IsInstanceAvailable(T instance)
	{
		if (_available.Contains(instance))
		{
			return true;
		}
		return false;
	}

	public void ReturnAllInUseInstances()
	{
		for (int i = _inUse.Count - 1; i >= 0; i--)
		{
			ReturnInstance(_inUse[i]);
		}
	}

	public void ReturnInstance(IPoolableItem instance)
	{
		ReturnInstance((T)instance);
	}

	public void ReturnInstance(T instance)
	{

		instance.ResetInstance();
		bool inList = _inUse.Remove(instance);

		if (inList)
		{
			_available.Enqueue(instance);
		}
		else
		{
			if (_available.Contains(instance))
			{
				return;
			}

			DismissInstance(instance);
		}
	}

	private T CreateInstance()
	{
		T instance = CreateInstanceInternal();
		instance.ResetInstance();
		instance.SetPool(this);
		return instance;
	}

	protected T CreateInstanceInternal()
	{
		return Object.Instantiate(_blueprint, _parent);
	}

	protected void DismissInstance(T instance)
	{
		Component component = instance as Component;
		if (component != null)
		{
			Object.Destroy(component.gameObject);
			return;
		}
		Object.Destroy(instance);
	}
}

