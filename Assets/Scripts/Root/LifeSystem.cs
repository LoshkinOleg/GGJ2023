using UnityEngine;

public class LifeSystem : MonoBehaviour, IResetable
{
	[SerializeField]
	private float _initialLife = 5f;
	[SerializeField]
	private float _dmgPerSecond = .1f;


	[Header("Events")]
	[SerializeField]
	private FloatEvent _onDeath = null;
	[SerializeField]
	private FloatEvent _onAddLife = null;


	private float _currentLife = 0f;
	public float CurrentLife
	{
		get
		{
			return _currentLife;
		}
		set
		{
			_currentLife = value;
		}
	}

	public float InitialLife { get { return _initialLife; } }

	private void OnEnable()
	{
		_currentLife = _initialLife;

		if (_onAddLife != null)
		{
			_onAddLife.AddListener(AddLife);
		}
	}

	private void OnDisable()
	{
		if (_onAddLife != null)
		{
			_onAddLife.RemoveListener(AddLife);
		}
	}

	private void Update()
	{
		if (GM.Instance.Paused || _currentLife < 0f)
		{
			return;
		}

		_currentLife -= _dmgPerSecond * Time.deltaTime;


		if (_currentLife < 0f)
		{
			if (_onDeath != null)
			{
				_onDeath.Raise(0f);
			}
		}


	}

	private void AddLife(float value)
	{
		_currentLife += value;

	}

	public void ResetObject()
	{
		_currentLife = _initialLife;

	}
}
