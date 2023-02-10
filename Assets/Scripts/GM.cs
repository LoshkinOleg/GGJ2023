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
	[SerializeField]
	private PanelFadeInOut _menuCanvas = null;
	[SerializeField]
	private PanelFadeInOut _endCanvas = null;

	[Header("Cameras")]
	[SerializeField]
	private CinemachineVirtualCamera _menuCamera = null;
	[SerializeField]
	private CinemachineVirtualCamera _gameplayCamera = null;
	[SerializeField]
	private CinemachineVirtualCamera _endCamera = null;



	public MusicSystem _music;


	[Header("flower")]
	[SerializeField]
	private Animator _flowerAnimator = null;


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

		if (_onDeath != null)
		{
			_onDeath.AddListener(OnDeath);
		}


		_menuCanvas.OnFadeOut += OnMenuFadeComplete;
		_endCanvas.OnFadeOut += ShowMenu;

		ShowMenu();

	}

	private void OnDisable()
	{
		if (_onDeath != null)
		{
			_onDeath.RemoveListener(OnDeath);
		}

		_menuCanvas.OnFadeOut -= OnMenuFadeComplete;
		_endCanvas.OnFadeOut -= ShowMenu;
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
		//_menuCanvas.gameObject.SetActive(false);
		_menuCanvas.FadeOut();

		_inputActions.Menu.Enable();


		_gameplayCamera.gameObject.SetActive(true);
		_menuCamera.gameObject.SetActive(false);
	}

	public void Unpause()
	{
		paused_ = false;
	}

	public void Pause()
	{
		paused_ = true;
	}

	public void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
		Application.Quit();
	}


	private void OnDeath(float value)
	{

		_inputActions.Menu.Enable();

		Pause();

		_menuCanvas.gameObject.SetActive(false);
		_endCanvas.gameObject.SetActive(true);
		_endCanvas.FadeIn();

		_gameplayCamera.gameObject.SetActive(false);
		_menuCamera.gameObject.SetActive(false);
		_endCamera.gameObject.SetActive(true);

		_flowerAnimator.SetTrigger("Death");
	}


	public void ShowMenu()
	{
		_endCanvas.gameObject.SetActive(false);
		_menuCanvas.gameObject.SetActive(true);
		_menuCanvas.FadeIn();


		_gameplayCamera.gameObject.SetActive(false);
		_menuCamera.gameObject.SetActive(true);
		_endCamera.gameObject.SetActive(false);


		_flowerAnimator.SetTrigger("Restart");

		_music.ChangeStatus(MusicSystem.Status.MENU);
	}

	public void HideEndScreen()
	{
		_endCanvas.FadeOut();
	}

	public void OnMenuFadeComplete()
	{
		_menuCanvas.gameObject.SetActive(false);
	}


}
