using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MonoBehaviour
{
	[SerializeField]
	private RootMovement _movement = null;


	public RootMovement Movement { get { return _movement; } }

}
