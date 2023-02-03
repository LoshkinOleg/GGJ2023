using System.Collections.Generic;
using UnityEngine;

public class PoolsManager
{
	// TODO: Remove singleton and utilice a ServiceLocator
	private static PoolsManager _instance = null;

	private readonly Dictionary<int, IPool> _cache = new Dictionary<int, IPool>();

	public static PoolsManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new PoolsManager();
			}
			return _instance;
		}
	}

	public bool HasPool(string ID)
	{
		return _cache.ContainsKey(ID.GetHashCode());
	}

	public IPool<T> CreatePool<T>(string ID, bool autoAddInstances, T blueprint, Transform parent = null) where T : Object, IPoolableItem
	{
		if (HasPool(ID))
		{
			return _cache[ID.GetHashCode()] as IPool<T>;
		}
		var pool = new Pool<T>(autoAddInstances, blueprint, parent);
		_cache.Add(ID.GetHashCode(), pool);
		return pool;
	}

	public IPool GetPool(string ID)
	{
		if (_cache.ContainsKey(ID.GetHashCode()))
		{
			return _cache[ID.GetHashCode()];
		}
		return null;
	}

	public IPool<T> GetPool<T>(string ID) where T : IPoolableItem
	{
		IPool toReturn = GetPool(ID);
		return toReturn == null ? null : toReturn as IPool<T>;
	}

	public void RemovePool(string ID)
	{
		if (_cache.ContainsKey(ID.GetHashCode()))
		{
			_cache[ID.GetHashCode()].Clear();
			_cache.Remove(ID.GetHashCode());
		}
	}

	public void ReturnAllPools()
	{
		foreach (KeyValuePair<int, IPool> pool in _cache)
		{
			pool.Value.ReturnAllInUseInstances();
		}
	}
}

