using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private float distance_ = 250.0f;
    [SerializeField] private GameObject target_;

    private void print(object o)
    {
        UnityEngine.Debug.Log(o);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var targetPos = target_.transform.position;
        print(targetPos);
    }
}
