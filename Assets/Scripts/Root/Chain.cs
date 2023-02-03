using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : MonoBehaviour
{
    private struct PosAndRot
    {
        public Vector3 pos;
        public Quaternion rot;
    }


    [SerializeField]
    private List<ChainElement> _elements = null;
    [SerializeField]
    private ChainElement _chainPrefab = null;

    [SerializeField]
    private Transform _head = null;
    [SerializeField]
    private float _distance = .5f;

    private float _timer = 0f;

    private readonly Queue<PosAndRot> _headPos = new Queue<PosAndRot>();
    private IPool<ChainElement> _poolChain = null;

    private void Start()
    {
        _poolChain = PoolsManager.Instance.CreatePool("Chain", true, _chainPrefab);
        _poolChain.GenerateAvailableInstances(100); // TODO number
        AddChain();

	}

    private void OnEnable()
    {
        // TODO change this number
        for (int i = 0; i < 20; i++)
        {
            PosAndRot posAndRot = new PosAndRot();
            posAndRot.pos = Vector3.zero;
            posAndRot.rot = Quaternion.identity;
            _headPos.Enqueue(posAndRot);
        }
    }


    private void LateUpdate()
    {
        // this is not performant at all
        PosAndRot posAndRot = new PosAndRot();
        posAndRot.pos = _head.position;
        posAndRot.rot = _head.rotation;
        _headPos.Enqueue(posAndRot);
        _headPos.Dequeue();

        _timer -= Time.deltaTime;
        if (_timer < 0f)
        {
            for (int i = _elements.Count - 1; i > 0; i--)
            {
                _elements[i].transform.SetPositionAndRotation(_elements[i - 1].transform.position, _elements[i - 1].transform.rotation);
            }

            _elements[0].transform.SetPositionAndRotation(_head.transform.position, _head.transform.rotation);

            AddChain();

			_timer = _distance;
        }
    }


    private void AddChain()
    {

        ChainElement newChainElement = _poolChain.GetInstance();
        newChainElement.gameObject.SetActive(true);
		_elements.Add(newChainElement);
	}
}
