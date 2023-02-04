using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeSystem : MonoBehaviour
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


	private void OnEnable()
	{
		_currentLife = _initialLife;

		if(_onAddLife!=null)
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
		if (_currentLife < 0f)
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


}
