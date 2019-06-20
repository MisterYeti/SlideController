using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class FadableCanvasGroup : MonoBehaviour
{
    [Header("Will be used if controller isn't set to Global")]
    [SerializeField] private float _fadeDuration = 0.5f;

    private CanvasGroup _canvasGroup;


    [HideInInspector] public UnityEvent OnFadeIn;
    [HideInInspector] public UnityEvent OnFadeOut;

    public float FadeDuration { get => _fadeDuration; }

    private void Init()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        gameObject.SetActive(true);
        
    }

    public void FadeIn()
    {
        Init();
        StopAllCoroutines();
        _canvasGroup.blocksRaycasts = false;
        StartCoroutine(FadeCoroutine(true));
    }

    public void FadeIn(float duration)
    {
        _fadeDuration = duration;
        FadeIn();
    }

    public void FadeOut()
    {
        Init();
        StopAllCoroutines();
        _canvasGroup.blocksRaycasts = false;
        StartCoroutine(FadeCoroutine(false));
    }

    public void FadeOut(float duration)
    {
        _fadeDuration = duration;
        FadeOut();
    }


    private IEnumerator FadeCoroutine(bool fadeIn)
    {
        var currentTime = 0f;
        var startValue = _canvasGroup.alpha;
        var endValue = fadeIn ? 1f : 0f;
        var duration = fadeIn ? (1f - startValue) * FadeDuration : startValue * FadeDuration;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            _canvasGroup.alpha = Mathf.Lerp(startValue, endValue, currentTime / duration);
            yield return null;
        }

        if (fadeIn)
        {
            OnFadeIn?.Invoke();
            _canvasGroup.blocksRaycasts = true;
        }
        else
        {
            OnFadeOut?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
