using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Chain : MonoBehaviour
{
	private struct PosAndRot
	{
		public Vector3 pos;
		public Quaternion rot;
	}


	[SerializeField]
	private List<ChainElement> _elements = null;
	[SerializeField]
	private ChainElement _chainPrefab = null;

	[SerializeField]
	private Root _head = null;
	[SerializeField]
	private float _distance = .5f;

	[SerializeField]
	private float _returnSpeed = 1f;
	[SerializeField]
	private float _returnChangeTarget = 1f;


	private float _timer = 0f;

	private readonly Queue<PosAndRot> _headPos = new Queue<PosAndRot>();
	private IPool<ChainElement> _poolChain = null;


	private bool _returning = false;
	private int _returningCount = 0;


	private void Start()
	{
		_poolChain = PoolsManager.Instance.CreatePool("Chain", true, _chainPrefab);
		_poolChain.GenerateAvailableInstances(100); // TODO number

		AddChain();
	}

	private void OnEnable()
	{
		// TODO change this number
		for (int i = 0; i < 20; i++)
		{
			PosAndRot posAndRot = new PosAndRot
			{
				pos = Vector3.zero,
				rot = Quaternion.identity
			};
			_headPos.Enqueue(posAndRot);
		}

		_head.Movement.OnAction += NewChain;
	}

	private void OnDisable()
	{
		_head.Movement.OnAction -= NewChain;
	}


	private void Update()
	{
		// This code is ugly :(
		if (_returning)
		{
			if (_returningCount < _elements.Count -1)
			{
				_head.transform.position = Vector3.Lerp(_head.transform.position, _elements[_returningCount].transform.position, _returnSpeed * Time.deltaTime);
			}
			_timer -= Time.deltaTime;
			if (_timer < 0f)
			{
				_returningCount++;
				if (_returningCount >= _elements.Count -1)
				{

					NewChain();
				}
				_timer = _returnChangeTarget;
			}
		}
	}

	private void LateUpdate()
	{
		if (!_returning)
		{

			// this is not performant at all
			PosAndRot posAndRot = new PosAndRot();
			posAndRot.pos = _head.transform.position;
			posAndRot.rot = _head.transform.rotation;
			_headPos.Enqueue(posAndRot);
			_headPos.Dequeue();

			_timer -= Time.deltaTime;
			if (_timer < 0f)
			{
				for (int i = _elements.Count - 1; i > 0; i--)
				{
					_elements[i].transform.SetPositionAndRotation(_elements[i - 1].transform.position, _elements[i - 1].transform.rotation);
				}

				_elements[0].transform.SetPositionAndRotation(_head.transform.position, _head.transform.rotation);

				AddChain();

				_timer = _distance;
			}
		}
	}

	private void AddChain()
	{

		ChainElement newChainElement = _poolChain.GetInstance();
		newChainElement.gameObject.SetActive(true);
		_elements.Add(newChainElement);
	}

	public void NewChain()
	{
		if (_returning)
		{
			_returningCount = 0;
			_timer = 0f;
			_elements.Clear();
			AddChain();
		}
		_returning = !_returning;
	}
}
