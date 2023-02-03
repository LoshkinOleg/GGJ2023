
public interface IPool
{
	bool AutoAddInstances { get; set; }

	void Clear();
	void GenerateAvailableInstances(int amount);
	void ReturnAllInUseInstances();
	void ReturnInstance(IPoolableItem instance);
}

public interface IPool<T> : IPool where T : IPoolableItem
{
	T GetInstance();
	bool IsInstanceAvailable(T instance);
	void ReturnInstance(T instance);
}

