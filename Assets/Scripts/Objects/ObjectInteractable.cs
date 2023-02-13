using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteractable : MonoBehaviour, IResetable
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
	private AudioClipEvent _audioEvent = null;
	[SerializeField]
	private AudioClip _actionSFX = null;
	[SerializeField]
	private AudioClip _rewardSFX = null;

	[Header("Animation")]
	[SerializeField]
	private Transform _graphic = null;
	[SerializeField]
	private float _punchIntensity = 1f;
	[SerializeField]
	private float _punchDuration = .3f;

	private bool _active = true;

	public void ResetObject()
	{
		_graphic.DOKill();
		_graphic.localRotation = Quaternion.identity;

		_active = true;
	}

	private void OnTriggerEnter(Collider other)
	{
		_graphic.DOKill();
		_graphic.localRotation = Quaternion.identity;
		_graphic.DOPunchRotation(Vector3.one * _punchIntensity, _punchDuration);

		for (int i = 0; i < _onActions.Count; i++)
		{
			if (_onActions[i] != null)
			{
				_onActions[i].Raise(_value);
			}
		}



		if (_onlyActivation && _active)
		{
			if (_rewardSFX != null && _audioEvent != null)
			{
				_audioEvent.Raise(_rewardSFX);
			}

			for (int i = 0; i < _rewardActions.Count; i++)
			{
				if (_rewardActions[i] != null)
				{
					_rewardActions[i].Raise(_value);
				}
			}



		}else
		{
			if (_actionSFX != null && _audioEvent != null)
			{
				_audioEvent.Raise(_actionSFX);
			}
		}
		_active = false;

	}
}
