using DG.Tweening;
using System;
using UnityEngine;

public class PanelFadeInOut : MonoBehaviour
{
	public Action OnFadeIn;
	public Action OnFadeOut;

	[SerializeField]
	private CanvasGroup _canvasGroup = null;
	[SerializeField]
	private float _durationIn = 1f;
	[SerializeField]
	private float _durationOut = 1f;

	private void OnDisable()
	{
		_canvasGroup.DOKill();
	}

	public void FadeIn()
	{
		_canvasGroup.DOKill();
		_canvasGroup.alpha = 0f;
		_canvasGroup.interactable = false;
		_canvasGroup.DOFade(1f, _durationIn).OnComplete(() =>
		{
			OnFadeIn?.Invoke();
			_canvasGroup.interactable = true;
		});
	}

	public void FadeOut()
	{
		_canvasGroup.DOKill();
		_canvasGroup.interactable = false;
		_canvasGroup.DOFade(0f, _durationIn).OnComplete(() =>
		{
			OnFadeOut?.Invoke();
			_canvasGroup.interactable = true;
		});
	}
}
