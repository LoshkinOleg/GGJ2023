using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RootMovement : MonoBehaviour
{
	[SerializeField]
	private float _speed = 0f;
	[SerializeField]
	private float _rotationSpeed = 40f;

	public UnityEvent _onAction = null;


	private InputActionsRoot _inputActions = null;
	private Vector3 _moveDirection = Vector3.down;
	private Vector2 _input = Vector2.down;

	private void OnEnable()
	{
		if (_inputActions == null)
		{
			_inputActions = new InputActionsRoot();
		}
		_inputActions.Gameplay.Enable();
		_inputActions.Gameplay.Movement.performed += OnMovement;

		_inputActions.Gameplay.Action.performed += OnAction;
	}

	private void OnDisable()
	{
		_inputActions.Gameplay.Movement.performed -= OnMovement;
		_inputActions.Gameplay.Action.performed -= OnAction;
	}

	private void Update()
	{
		_moveDirection = Vector2.Lerp(_moveDirection, _input, _rotationSpeed * Time.deltaTime);
		_moveDirection.Normalize();

		Vector3 pos = transform.position;
		pos += _speed * Time.deltaTime * _moveDirection;
		transform.LookAt(pos);
		transform.position = pos;

	}

	private void OnMovement(InputAction.CallbackContext obj)
	{
		_input =   obj.ReadValue<Vector2>();
		_input.y = Mathf.Floor(_input.y);
		if (_input == Vector2.zero)
		{
			_input = _moveDirection;
		}

	}

	private void OnAction(InputAction.CallbackContext obj)
	{
		// This is for debug. I need to change it
		_onAction.Invoke();

	}




}
