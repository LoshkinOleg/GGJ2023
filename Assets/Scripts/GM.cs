using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
// using System.Runtime.Hosting;
using System.Runtime.InteropServices;
// using System.Security.Policy;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using Cinemachine;

public class GM : MonoBehaviour
{
    private void MyPrint(object o)
    {
        UnityEngine.Debug.Log(o);
    }

    // Singleton.
    private static GM _instance;
    public static GM Instance
    {
        get
        {
            return _instance;
        }
    }


    [Header("UI")]
    [SerializeField] private GameObject menuCanvas_;

    [Header("Cameras")]
    [SerializeField]
    private CinemachineVirtualCamera _menuCamera = null;
	[SerializeField]
	private CinemachineVirtualCamera _gameplayCamera = null;



	[SerializeField]
    private FloatEvent _onDeath = null;

    private InputActionsRoot _inputActions = null;


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

    private void OnEnable()
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

        if (_inputActions == null)
        {
            _inputActions = new InputActionsRoot();
        }
        _inputActions.Menu.Enable();
        _inputActions.Menu.Menu.performed += TogglePause;


        if(_onDeath!=null)
        {
            _onDeath.AddListener(OnDeath);
        }

        _gameplayCamera.gameObject.SetActive(false);
        _menuCamera.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
		_inputActions.Menu.Menu.performed -= TogglePause;

		if (_onDeath != null)
		{
			_onDeath.RemoveListener(OnDeath);
		}
	}

    private void TogglePause(InputAction.CallbackContext obj)
    {
        if(menuCanvas_.activeInHierarchy)
        {
            return;
        }

        if (!paused_)
        {
            Pause();
        }
        else
        {
            Unpause();
        }
    }

    public void Restart()
    {
        paused_ = false;


        var list = FindObjectsOfType<MonoBehaviour>().OfType<IResetable>();
        foreach (var item in list)
        {
            item.ResetObject();
        }
        paused_ = false;
        menuCanvas_.SetActive(false);

		_inputActions.Menu.Enable();


		_gameplayCamera.gameObject.SetActive(true);
		_menuCamera.gameObject.SetActive(false);
	}

    public void Unpause()
    {
        paused_ = false;
        menuCanvas_.SetActive(false);
    }

    public void Pause()
    {
        paused_ = true;
        menuCanvas_.SetActive(true);
    }

    public void Quit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }


    private void OnDeath( float value)
    {
		menuCanvas_.SetActive(true);
		_inputActions.Menu.Enable();

        Pause();

		_gameplayCamera.gameObject.SetActive(false);
		_menuCamera.gameObject.SetActive(true);
	}


}
