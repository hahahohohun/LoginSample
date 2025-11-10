using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIToastMessage : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Text uiText;

    [Header("효과 설정")]
    [SerializeField] private float fadeInTime = 0.15f;
    [SerializeField] private float fadeOutTime = 0.15f;
    [SerializeField] private Vector2 spawnOffset = new Vector2(0f, 40f);
    [SerializeField] private float riseOffset = 20f;

    private RectTransform _rt;

    private void Awake()
    {
        _rt = transform as RectTransform;
        if (canvasGroup == null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void Setup(string message)
    {
        SetText(message);
        if (_rt != null) 
            _rt.anchoredPosition += spawnOffset;
        canvasGroup.alpha = 0f;
        gameObject.SetActive(true);
    }

    public IEnumerator PlayFadeCoroutine(float totalDuration)
    {
        // 등장 페이드
        yield return StartCoroutine(FadeAndRise(0f, 1f, fadeInTime, -riseOffset));

        // 유지 후 서서히 사라짐
        float fadeDuration = Mathf.Max(0f, totalDuration - fadeInTime - fadeOutTime);
        if (fadeDuration > 0f)
            yield return StartCoroutine(Fade(1f, 0f, fadeDuration));
        else
            yield return new WaitForSeconds(totalDuration);

        // 종료 페이드
        yield return StartCoroutine(Fade(0f, 0f, fadeOutTime));
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float t = 0f;
        canvasGroup.alpha = from;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, t / duration);
            yield return null;
        }
        canvasGroup.alpha = to;
    }

    private IEnumerator FadeAndRise(float from, float to, float duration, float yOffset)
    {
        if (_rt == null) 
            yield break;

        float t = 0f;
        Vector2 start = _rt.anchoredPosition + new Vector2(0f, yOffset);
        Vector2 end = _rt.anchoredPosition;

        canvasGroup.alpha = from;
        _rt.anchoredPosition = start;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float p = Mathf.Clamp01(t / duration);
            float ease = 1f - Mathf.Pow(1f - p, 3f);
            canvasGroup.alpha = Mathf.Lerp(from, to, ease);
            _rt.anchoredPosition = Vector2.Lerp(start, end, ease);
            yield return null;
        }

        canvasGroup.alpha = to;
        _rt.anchoredPosition = end;
    }

    private void SetText(string message)
    {
        if (uiText != null)
            uiText.text = message;
    }
}
