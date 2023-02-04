using UnityEngine;
using UnityEngine.EventSystems;

public class UISelect : MonoBehaviour
{
    [SerializeField]
    private GameObject _selectedGameObject;

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(_selectedGameObject);
    }
}
