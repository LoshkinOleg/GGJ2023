using UnityEngine;


public class PooleableMonoBehaviour : MonoBehaviour, IPoolableItem
{
	private IPool _pool = null;

	private void OnDestroy()
	{
		_pool = null;
	}

	public void ResetInstance()
	{
		transform.position = Vector3.zero;
		gameObject.SetActive(false);
	}

	public void ReturnToPool()
	{
		_pool.ReturnInstance(this);
	}

	public void SetPool(IPool pool)
	{
		_pool = pool;
	}
}

