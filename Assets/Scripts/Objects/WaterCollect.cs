using DG.Tweening;
using UnityEngine;

public class WaterCollect : MonoBehaviour, IResetable
{
    [SerializeField]
    private Transform _water = null;
    [SerializeField]
    private float _time = 1f;

    private Vector3 _localScale;

    private void Start()
    {
        _localScale = _water.localScale;
    }


    public void ResetObject()
    {
        _water.DOKill();
        _water.localScale = _localScale;
    }


    public void PlayAnimation()
    {
        _water.DOKill();
        _water.DOScaleY(0f, _time);
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayAnimation();
    }
}
