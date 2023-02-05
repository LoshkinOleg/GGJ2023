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

	[Header("Audio")]
	[SerializeField]
	private AudioSource _audioSource = null;
	[SerializeField]
	private AudioClip _actionSFX = null;
	[SerializeField]
	private AudioClip _rewardSFX = null;


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


		if (_actionSFX != null)
		{
			_audioSource.PlayOneShot(_actionSFX);
		}

		if (_onlyActivation && !_active)
		{
			return;
		}

		if (_rewardSFX != null)
		{
			_audioSource.PlayOneShot(_rewardSFX);
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
