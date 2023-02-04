using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicSystem : MonoBehaviour
{
    [SerializeField] private LifeSystem _lifeSystem;
    [SerializeField] AudioSource _musicLifeHighest;
    [SerializeField] AudioSource _musicLifeHigh;
    [SerializeField] AudioSource _musicLifeLow;
    [SerializeField] AudioSource _musicLifeLowest;
    [SerializeField] float _timeForHighest = 60.0f;
    [SerializeField] float _timeForHigh = 40.0f;
    [SerializeField] float _timeForLow = 25.0f;
    [SerializeField] float _timeForLowest = 10.0f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var remainingTime = _lifeSystem.CurrentLife;
        
    }
}
