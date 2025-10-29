using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToastMsg : MonoBehaviour
{
    public static ToastMsg Instance { get; private set; }

    [Header("Toast Prefab (UIToastMessage)")]
    [SerializeField] private UIToastMessage toastPrefab;

    [Header("Parent for Toasts")]
    [SerializeField] private RectTransform parent;

    [Header("Defaults")]
    [SerializeField] private float defaultFadeDuration = 1f;

    [Tooltip("한 번에 표시 가능한 최대 개수")]
    [SerializeField] private int maxActiveToasts = 3;

    [Tooltip("메시지 간 세로 간격 (px)")]
    [SerializeField] private float verticalSpacing = 8f;

    private readonly Queue<(string msg, float fade)> _queue = new();
    private readonly List<UIToastMessage> _activeToasts = new();

    private bool _isProcessing;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (parent == null)
        {
            var canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                parent = canvas.transform as RectTransform;
                canvas.overrideSorting = true;    // 부모 Canvas 영향 무시
                canvas.sortingOrder = 9999;       // 다른 모든 UI보다 위로
            }
        }
    }

    public static void Show(string message)
    {
        if (Instance == null)
        {
            Debug.LogWarning("[ToastMsg] Instance is null. 씬에 ToastMsg 오브젝트가 필요합니다.");
            return;
        }
        Instance.Enqueue(message, Instance.defaultFadeDuration);
    }

    public static void Show(string message, float fade)
    {
        if (Instance == null)
        {
            Debug.LogWarning("[ToastMsg] Instance is null. 씬에 ToastMsg 오브젝트가 필요합니다.");
            return;
        }
        Instance.Enqueue(message, fade);
    }

    private void Enqueue(string message, float fade)
    {
        _queue.Enqueue((message, fade));
        if (!_isProcessing)
            StartCoroutine(ProcessQueue());
    }

    private IEnumerator ProcessQueue()
    {
        _isProcessing = true;

        while (_queue.Count > 0 || _activeToasts.Count > 0)
        {
            // 동시에 최대 maxActiveToasts까지 표시
            while (_queue.Count > 0 && _activeToasts.Count < maxActiveToasts)
            {
                var (msg, fade) = _queue.Dequeue();
                StartCoroutine(ShowToastRoutine(msg, fade));
            }

            yield return null;
        }

        _isProcessing = false;
    }

    private IEnumerator ShowToastRoutine(string msg, float fade)
    {
        if (toastPrefab == null)
        {
            Debug.LogError("[ToastMsg] toastPrefab is null");
            yield break;
        }

        var toast = Instantiate(toastPrefab, parent);
        _activeToasts.Add(toast);

        UpdateToastPositions();

        toast.Setup(msg);
        yield return StartCoroutine(toast.PlayFadeCoroutine(fade));

        _activeToasts.Remove(toast);
        Destroy(toast.gameObject);

        UpdateToastPositions();
    }

    private void UpdateToastPositions()
    {
        // 화면 중앙 기준 정렬
        // 첫 번째 메시지를 중앙에 두고, 나머지는 위로 차곡차곡 쌓이게
        float totalHeight = (_activeToasts.Count - 1) * verticalSpacing;
        float startY = -totalHeight * 0.5f; // 중앙 정렬

        for (int i = 0; i < _activeToasts.Count; i++)
        {
            if (_activeToasts[i] == null) continue;
            var rt = _activeToasts[i].transform as RectTransform;
            if (rt != null)
            {
                rt.anchorMin = new Vector2(0.5f, 0.5f);
                rt.anchorMax = new Vector2(0.5f, 0.5f);
                rt.pivot = new Vector2(0.5f, 0.5f);
                rt.anchoredPosition = new Vector2(0f, startY + i * verticalSpacing);
            }
        }
    }
}
