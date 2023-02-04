using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractable : MonoBehaviour
{

    [SerializeField]
    private List<FloatEvent> _onActions = null;
	[SerializeField]
	private List<FloatEvent> _rewardActions = null;



	[SerializeField]
    private float _value = 0f;

    [SerializeField]
    private bool _onlyActivation = false;

    private bool _active = true;

    private void OnTriggerEnter(Collider other)
    {


        for (int i = 0; i < _onActions.Count; i++)
        {
			if (_onActions[i] != null)
			{
                _onActions[i].Raise(_value);
			}
		}

		if (_onlyActivation && !_active)
		{
			return;
		}

		for (int i = 0; i < _rewardActions.Count; i++)
		{
			if (_rewardActions[i] != null)
			{
				_rewardActions[i].Raise(_value);
			}
		}

		_active = false;
    }
}
