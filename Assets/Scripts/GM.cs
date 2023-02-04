using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
// using System.Runtime.Hosting;
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

    [SerializeField] private GameObject menuCanvas_;

    // private bool gameStarted_ = false;
    private bool paused_ = true;
    public bool Paused
    {
        get
        {
            return paused_;
        }
        set
        {
            paused_ = value;
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

    public void ShowMenu()
    {
        menuCanvas_.SetActive(true);
        paused_ = true;
    }

    public void HideMenu()
    {
        menuCanvas_.SetActive(false);
        paused_ = false;
    }

    public void Restart()
    {
        // Oleg@Nacho: reset map and root. Reset any potential score.
        // gameStarted_ = true;
        paused_ = false;
    }

    public void Quit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
