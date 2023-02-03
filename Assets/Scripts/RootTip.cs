using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class RootTip : MonoBehaviour
{
    void print(object o)
    {
        UnityEngine.Debug.Log(o);
    }

    private float speed = 100.0f;
    private float inputX = 0.0f;
    private float inputY = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float inputX = Input.GetAxis("Horizontal");
        float inputY = 1.0f;// Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        float dt = UnityEngine.Time.deltaTime;
        float pos = transform.position.x; // += inputX * speed * dt;

        transform.position = new Vector3(transform.position.x + inputX * speed * dt, transform.position.y + inputY * speed * dt, transform.position.z);

        // transform.position.y; // += inputY * speed * dt;
        // print(pos);
    }
}
