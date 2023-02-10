using Palmmedia.ReportGenerator.Core.Parser.Analysis;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour, IResetable
{
	[System.Serializable]
	private struct ReturnBranch
	{
		public List<Vector3> positions;
		public int returningCount;
		public ChainLine line;
		public float width;
	}

	[SerializeField]
	private Root _head = null;

	[Header("Lines")]
	[SerializeField]
	private ChainLine _linePrefab = null;
	[SerializeField]
	private int _linesPoolSize = 10;
	[SerializeField]
	private float _distance = .5f;

	[SerializeField]
	private float _returnSpeed = 1f;
	[SerializeField]
	private float _returnChangeTarget = 1f;

	[SerializeField]
	private float _initialWidth = 1.5f;
	[SerializeField]
	private float _minWidth = .2f;

	[Header("Colors")]
	[SerializeField]
	private LifeSystem _lifeSystem = null;
	[SerializeField]
	private Gradient _lifeColor;

	[Header("Events")]
	[SerializeField]
	private FloatEvent _onReturn = null;
	[SerializeField]
	private FloatEvent _onDeath = null;

	private float _timer = 0f;
	private bool _returning = false;
	private int _returningCount = 0;

	private float _currentWidth = 0f;


	private IPool<ChainLine> _poolLines = null;
	private List<ChainLine> _lines = new List<ChainLine>();
	private List<Vector3> _elements = new List<Vector3>();
	private List<ReturnBranch> _returnBranches = new List<ReturnBranch>();


	private void Start()
	{
		_poolLines = PoolsManager.Instance.CreatePool("Lines", true, _linePrefab);
		_poolLines.GenerateAvailableInstances(_linesPoolSize);

		AddLine();
		AddChain();
	}

	public void ResetObject()
	{
		_returningCount = 0;
		_timer = -0.01f;
		_elements.Clear();

		_returning = false;
		_returnBranches.Clear();

		_poolLines.ReturnAllInUseInstances();


		_head.transform.position = Vector3.zero;
		AddLine();
		AddChain();

		_head.Movement.Activate = true;
	}

	private void OnEnable()
	{
		_head.Movement.OnAction += NewChain;


		if (_onReturn != null)
		{
			_onReturn.AddListener(NewChain);
		}

		if (_onDeath != null)
		{
			_onDeath.AddListener(NewChain);
		}

		_head.Movement.Activate = false;
	}

	private void OnDisable()
	{
		_head.Movement.OnAction -= NewChain;

		if (_onReturn != null)
		{
			_onReturn.RemoveListener(NewChain);
		}

		if (_onDeath != null)
		{
			_onDeath.RemoveListener(NewChain);
		}
	}

	private void Update()
	{
		if (GM.Instance.Paused)
		{
			return;
		}


		if (_returning)
		{
			if (_returningCount < _elements.Count - 1)
			{
				// TODO : clean this. IDK if I need this lerp
				_head.transform.position = Vector3.Lerp(_head.transform.position, _elements[_elements.Count - 1 - _returningCount], _returnSpeed * Time.deltaTime);
			}
			_timer -= Time.deltaTime;
			if (_timer < 0f)
			{
				_timer = _returnChangeTarget;
				_returningCount++;

				if (_returningCount >= _elements.Count - 1)
				{
					if (_returnBranches.Count > 0)
					{
						_returningCount = _returnBranches[^1].returningCount;
						_elements = _returnBranches[^1].positions;
						_currentWidth = _returnBranches[^1].width;

						_returnBranches.RemoveAt(_returnBranches.Count - 1);
					}
					else
					{
						_head.transform.position = _elements[0];
						NewZeroChain();
					}
				}

			}
		}


		for (int i = 0; i < _lines.Count; i++)
		{
			_lines[i].Line.startColor = _lines[i].Line.endColor = _lifeColor.Evaluate(_lifeSystem.CurrentLife / _lifeSystem.InitialLife);
		}


		if (!_returning)
		{
			_timer -= Time.deltaTime;
			if (_timer < 0f)
			{
				AddChain();

				_timer = _distance;
			}


			_elements[^1] = _head.transform.position;

			if (_lines.Count > 0)
			{
				// We update the full line positions if we don't match the length of the line, if not we update only the last element.
				LineRenderer line = _lines[^1].Line;
				if (line.positionCount == _elements.Count)
				{
					line.SetPosition(line.positionCount - 1, _elements[^1]);
				}
				else
				{
					line.positionCount = _elements.Count;
					line.SetPositions(_elements.ToArray());
				}
			}
		}
	}

	private void AddChain()
	{
		_elements.Add(_head.transform.position);
	}

	private void AddLine()
	{
		ChainLine newLine = _poolLines.GetInstance();
		newLine.gameObject.SetActive(true);
		_lines.Add(newLine);

		float maxWidth = _initialWidth;
		if (_returnBranches.Count > 0)
		{
			ReturnBranch branch = _returnBranches[^1];
			maxWidth = branch.width;

		}
		else
		{
			newLine.Line.widthMultiplier = _initialWidth;
		}
		float width = maxWidth;
		if (_elements.Count > 0)
		{
			width = maxWidth * _lines[^1].Line.widthCurve.Evaluate((float)(_elements.Count - _returningCount) / (float)_elements.Count);
		}

		if (width < _minWidth)
		{
			width = _minWidth;
		}
		newLine.Line.widthMultiplier = width;

	}
	public void NewChain(float value)
	{
		NewChainFull();
	}
	public void NewChain()
	{
		NewChainFull();
	}
	public void NewZeroChain()
	{
		NewChainFull(true);
	}

	public void NewChainFull(bool skip = false)
	{
		if (_returning)
		{
			if (!skip)
			{
				ReturnBranch newReturnBranch = new ReturnBranch
				{
					positions = new List<Vector3>(_elements),
					returningCount = _returningCount,
					line = _lines[^1],
					width = _lines[^1].Line.widthMultiplier
				};
				_returnBranches.Add(newReturnBranch);
			}
			AddLine();
			_elements.Clear();
			_timer = -0.01f;

			_returningCount = 0;


			AddChain();


			_head.Movement.Activate = true;
		}
		_returning = !_returning;

		if (_returning)
		{
			_head.Movement.Activate = false;
		}
	}
}
