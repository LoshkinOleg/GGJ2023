using UnityEngine;

public class ObjectInteractable : MonoBehaviour
{

    [SerializeField]
    private FloatEvent _onAction = null;

    [SerializeField]
    private float _value = 0f;

    [SerializeField]
    private bool _onlyActivation = false;

    private bool _active = true;

    private void OnTriggerEnter(Collider other)
    {
        if (_onlyActivation && !_active)
        {
            return;
        }

        if (_onAction != null)
        {
            _onAction.Raise(_value);
        }

        _active = false;
    }
}
