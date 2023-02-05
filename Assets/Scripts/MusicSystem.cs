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

    public enum Status
    {
        NONE,
        MENU,
        START,
        DESHYDRATE,
        DEAD
    }

    private Status _currentStatus = Status.NONE;

    private void OnEnable()
    {
        ChangeStatus(Status.MENU);
    }

    private void Update()
    {
        if(GM.Instance.Paused)
        {
            ChangeStatus(Status.MENU);
            return;
        }
        if(_lifeSystem.CurrentLife >= _timeForHighest)
        {
            ChangeStatus(Status.START);
        }else if(_lifeSystem.CurrentLife >= _timeForLow)
        {
            ChangeStatus(Status.DESHYDRATE);
        }else if (_lifeSystem.CurrentLife > 0f)
        {
            ChangeStatus(Status.DEAD);
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
			default:
                break;
        }
    }
}
