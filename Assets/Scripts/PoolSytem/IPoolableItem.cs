
public interface IPoolableItem
{
	void ResetInstance();
	void ReturnToPool();
	void SetPool(IPool pool);
}

