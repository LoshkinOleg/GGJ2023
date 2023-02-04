using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RootMovement : MonoBehaviour
{
	[SerializeField]
	private float _speed = 0f;
	[SerializeField]
	private float _rotationSpeed = 40f;

	public Action OnAction = null;

	private InputActionsRoot _inputActions = null;
	private Vector3 _moveDirection = Vector3.down;
	private Vector2 _input = Vector2.down;
	private bool _active = true;


	public bool Activate { get { return _active; } set { _active = value; } }

	private void OnEnable()
	{
		if (_inputActions == null)
		{
			_inputActions = new InputActionsRoot();
		}
		_inputActions.Gameplay.Enable();
		_inputActions.Gameplay.Movement.performed += OnMovement;

		_inputActions.Gameplay.Action.performed += OnActionPerformed;
	}

	private void OnDisable()
	{
		_inputActions.Gameplay.Movement.performed -= OnMovement;
		_inputActions.Gameplay.Action.performed -= OnActionPerformed;
	}

	private void Update()
	{
		if(!_active)
		{
			return;
		}
		_moveDirection = Vector2.Lerp(_moveDirection, _input, _rotationSpeed * Time.deltaTime);
		_moveDirection.Normalize();

		Vector3 pos = transform.position;
		pos += _speed * Time.deltaTime * _moveDirection;
		transform.LookAt(pos);
		transform.position = pos;

	}

	private void OnMovement(InputAction.CallbackContext obj)
	{
		_input = obj.ReadValue<Vector2>();
		_input.y = Mathf.Floor(_input.y);
		if (_input == Vector2.zero)
		{
			_input = _moveDirection;
		}
	}

	private void OnActionPerformed(InputAction.CallbackContext obj)
	{
		if (OnAction != null)
		{
			OnAction.Invoke();
		}
	}
}
