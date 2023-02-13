using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicSystem : MonoBehaviour
{
    [SerializeField] private LifeSystem _lifeSystem;
    [SerializeField] float _timeForHighest = 60.0f;
    [SerializeField] float _timeForLow = 25.0f;
    [SerializeField] float _timeForLowest = 10.0f;

    [Header("Snapshots")]
    [SerializeField] float _transitionTime = 1f;

    [SerializeField]
    private AudioMixerSnapshot _menuSnapshot;
    [SerializeField]
    private AudioMixerSnapshot _startSnapshot;
    [SerializeField]
    private AudioMixerSnapshot _deshydrateSnapshot;
    [SerializeField]
    private AudioMixerSnapshot _deadSnapshot;
    [SerializeField]
    private AudioMixerSnapshot _loseSnapshot;

    [Header("SFX")]
    [SerializeField]
    private AudioClipEvent _sfxEvent = null;
    [SerializeField]
    private AudioSource _sfxSource;

    [System.Serializable]
    public enum Status
    {
        NONE,
        MENU,
        START,
        DESHYDRATE,
        DEAD,
        DIE
    }

    private Status _currentStatus = Status.NONE;

    private void OnEnable()
    {
        ChangeStatus(Status.MENU);

        _sfxEvent.AddListener(PlaySFX);
    }

    private void OnDisable()
    {
		_sfxEvent.RemoveListener(PlaySFX);
	}

    private void Update()
    {

		if (GM.Instance.Paused)
		{
			//ChangeStatus(Status.MENU);
			return;
		}

		if (_lifeSystem.CurrentLife >= _timeForHighest)
        {
            ChangeStatus(Status.START);
        }else if(_lifeSystem.CurrentLife >= _timeForLow)
        {
            ChangeStatus(Status.DESHYDRATE);
        }else if (_lifeSystem.CurrentLife > _timeForLowest)
        {
            ChangeStatus(Status.DEAD);
        }if(_lifeSystem.CurrentLife < _timeForLowest)
        {
            ChangeStatus(Status.DIE);
        }


	}

    public void ChangeStatus(Status status)
    {
        if(status == _currentStatus)
        {
            return;
        }
        _currentStatus = status;
        switch (status)
        {
            case Status.MENU:
                _menuSnapshot.TransitionTo(_transitionTime);
                break;
            case Status.START:
                _startSnapshot.TransitionTo(_transitionTime);
                break;
            case Status.DESHYDRATE:
                _deshydrateSnapshot.TransitionTo(_transitionTime);
                break;
            case Status.DEAD:
                _deadSnapshot.TransitionTo(_transitionTime);
                break;
            case Status.DIE:
                _loseSnapshot.TransitionTo(_transitionTime);
                break;
			default:
                break;
        }
    }


    public void PlaySFX(AudioClip clip)
    {
		_sfxSource.PlayOneShot(clip);
	}
}
