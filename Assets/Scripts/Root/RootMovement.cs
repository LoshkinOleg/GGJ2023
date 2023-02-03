using UnityEngine;
using UnityEngine.InputSystem;

public class RootMovement : MonoBehaviour
{
	[SerializeField]
	private float _speed = 0f;
	[SerializeField]
	private float _rotationSpeed = 40f;

	private InputActionsRoot _inputActions = null;
	private Vector3 _moveDirection = Vector3.zero;
	private Vector2 _input = Vector2.zero;

	private void OnEnable()
	{
		if (_inputActions == null)
		{
			_inputActions = new InputActionsRoot();
		}
		_inputActions.Gameplay.Enable();
		_inputActions.Gameplay.Movement.performed += OnMovement;
	}

	private void OnDisable()
	{
		_inputActions.Gameplay.Movement.performed -= OnMovement;
	}

	private void Update()
	{
		_moveDirection = Vector2.Lerp(_moveDirection, _input, _rotationSpeed * Time.deltaTime);

		Vector3 pos = transform.position;
		pos += _speed * Time.deltaTime * _moveDirection;
		transform.LookAt(pos);
		transform.position = pos;

	}

	private void OnMovement(InputAction.CallbackContext obj)
	{
		_input = obj.ReadValue<Vector2>();
		if (_input == Vector2.zero)
		{
			_input = _moveDirection;
		}
	}




}
