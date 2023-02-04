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
	}

	[SerializeField]
	private int _chainPoolCount = 100;
	private List<ChainElement> _elements = new List<ChainElement>();


	[SerializeField]
	private ChainElement _chainPrefab = null;
	[SerializeField]
	private ChainLine _linePrefab = null;

	private List<ChainLine> _lines = new List<ChainLine>();

	[SerializeField]
	private Root _head = null;
	[SerializeField]
	private float _distance = .5f;

	[SerializeField]
	private float _returnSpeed = 1f;
	[SerializeField]
	private float _returnChangeTarget = 1f;

	[SerializeField]
	private FloatEvent _onReturn = null;



	private float _timer = 0f;

	private readonly Queue<Vector3> _headPos = new Queue<Vector3>();
	private IPool<ChainElement> _poolChain = null;
	private IPool<ChainLine> _poolLines = null;


	private bool _returning = false;
	private int _returningCount = 0;

	private List<List<Vector3>> _elementsPosition = new List<List<Vector3>>();

	private void Start()
	{
		_poolChain = PoolsManager.Instance.CreatePool("Chain", true, _chainPrefab);
		_poolChain.GenerateAvailableInstances(_chainPoolCount);

		_poolLines = PoolsManager.Instance.CreatePool("Lines", true, _linePrefab);
		_poolLines.GenerateAvailableInstances(10);


		AddLine();
		AddChain();
	}

	private void OnEnable()
	{
		// TODO change this number
		for (int i = 0; i < 20; i++)
		{
			_headPos.Enqueue(Vector3.zero);
		}

		_head.Movement.OnAction += NewChain;


		if(_onReturn !=null)
		{
			_onReturn.AddListener(NewChain);
		}
	}

	private void OnDisable()
	{
		_head.Movement.OnAction -= NewChain;

		if (_onReturn != null)
		{
			_onReturn.RemoveListener(NewChain);
		}
	}


	private void Update()
	{
		if (GM.Instance.Paused) return;

		// This code is ugly :(
		if (_returning)
		{
			if (_returningCount < _elements.Count - 1)
			{
				_head.transform.position = Vector3.Lerp(_head.transform.position, _elements[_elements.Count - 1 - _returningCount].transform.position, _returnSpeed * Time.deltaTime);
			}
			_timer -= Time.deltaTime;
			if (_timer < 0f)
			{
				_returningCount++;
				if (_returningCount >= _elements.Count - 1)
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
			_headPos.Enqueue(_head.transform.position);
			_headPos.Dequeue();

			_timer -= Time.deltaTime;
			if (_timer < 0f)
			{
				/*for (int i = _elements.Count - 1; i > 0; i--)
				{
					_elements[i].transform.position = (_elements[i - 1].transform.position);
					_elementsPosition[_elementsPosition.Count - 1][i] = _elements[i].transform.position;
				}

				_elements[0].transform.position = (_head.transform.position);
				_elementsPosition[_elementsPosition.Count - 1][0] = _elements[0].transform.position;
				*/
				AddChain();

				_timer = _distance;
			}


			_elementsPosition[_elementsPosition.Count - 1][_elements.Count - 1] = _elements[_elements.Count - 1].transform.position = (_head.transform.position);

			if (_lines.Count > 0)
			{
				_lines[_lines.Count - 1].Line.positionCount = _elementsPosition[_elementsPosition.Count - 1].Count;
				_lines[_lines.Count - 1].Line.SetPositions(_elementsPosition[_elementsPosition.Count - 1].ToArray());
			}
		}
	}

	private void AddChain()
	{

		ChainElement newChainElement = _poolChain.GetInstance();
		newChainElement.gameObject.SetActive(true);
		_elements.Add(newChainElement);

		newChainElement.transform.position = _head.transform.position;
		_elementsPosition[_elementsPosition.Count - 1].Add(_head.transform.position);
	}

	private void AddLine()
	{

		ChainLine newLine = _poolLines.GetInstance();
		newLine.gameObject.SetActive(true);
		_lines.Add(newLine);

		List<Vector3> positions = new List<Vector3>();
		_elementsPosition.Add(positions);
	}

	public void NewChain()
	{
		if (_returning)
		{
			_returningCount = 0;
			_timer = 0f;
			_elements.Clear();

			AddLine();
			AddChain();
		}
		_returning = !_returning;
	}

	public void NewChain(float value)
	{
		NewChain();
	}
}
