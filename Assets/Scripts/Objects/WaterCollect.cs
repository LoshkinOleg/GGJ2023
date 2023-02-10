using DG.Tweening;
using UnityEngine;

public class WaterCollect : MonoBehaviour, IResetable
{
	[SerializeField]
	private Transform _roots = null;
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
		_roots.DOKill();
		_roots.localScale = Vector3.zero;
	}


	public void PlayAnimation()
	{
		_water.DOKill();
		_water.DOScaleY(0f, _time);

		_roots.DOScale(Vector3.one * 100, _time * .5f).SetEase(Ease.OutCubic);
	}

	private void OnTriggerEnter(Collider other)
	{
		PlayAnimation();
	}
}
