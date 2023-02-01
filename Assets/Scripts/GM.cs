using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public class GM : MonoBehaviour
{
    // Singleton.
    private static GM _instance;
    public static GM Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            UnityEngine.Debug.Log("Deleting extra GM.");
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void Register(object caller)
    {
        
    }
}
