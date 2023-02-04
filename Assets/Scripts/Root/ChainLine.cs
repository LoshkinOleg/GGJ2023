using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLine : PooleableMonoBehaviour
{
	[SerializeField]
	private LineRenderer _line = null;


	public LineRenderer Line { get { return _line; } }
}
