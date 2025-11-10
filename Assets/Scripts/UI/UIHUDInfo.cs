using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class UIHUDInfo : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Text _envLabel;                 // 현재 환경 표시용 라벨
    [SerializeField] private TMP_Dropdown _envDropdown;      // 환경 선택 드롭다운 (TMP)
    [SerializeField] private Text _serverURL;          

    [Header("Options")]
    [SerializeField] private bool _rememberSelection = true; // PlayerPrefs로 기억할지 여부

    private IEnvironmentService _envService;

    private static readonly string PrefsKey = "UIHUDInfo.ServerEnv";

    [Inject]
    public void Construct(IEnvironmentService envService)
    {
        _envService = envService;
    }

    private void Awake()
    {
        if (_envDropdown == null)
        {
            return;
        }

        // 드롭다운 구성
        SetupDropdownOptions();

        // PlayerPrefs 저장값 복원 (선택사항)
        if (_rememberSelection && PlayerPrefs.HasKey(PrefsKey))
        {
            if (Enum.TryParse(PlayerPrefs.GetString(PrefsKey), out ServerEnv saved))
                _envService.Set(saved);
        }

        // 초기 UI 갱신
        SetDropdownValue(_envService.Environment);
        RefreshLabelAndColor(_envService.Environment);

        // 이벤트 연결
        _envDropdown.onValueChanged.AddListener(OnDropdownChanged);
        _envService.OnChanged += OnEnvChanged;
    }

    private void OnDestroy()
    {
        if (_envDropdown != null)
            _envDropdown.onValueChanged.RemoveListener(OnDropdownChanged);

        if (_envService != null)
            _envService.OnChanged -= OnEnvChanged;
    }

    private void SetupDropdownOptions()
    {
        _envDropdown.ClearOptions();
        var names = Enum.GetNames(typeof(ServerEnv));
        _envDropdown.AddOptions(new List<string>(names));
    }

    private void SetDropdownValue(ServerEnv env)
    {
        int idx = (int)env;
        if (_envDropdown.options.Count > idx)
            _envDropdown.SetValueWithoutNotify(idx);
    }

    private void OnDropdownChanged(int index)
    {
        var newEnv = (ServerEnv)index;

        _envService.Set(newEnv);  //
        OnEnvChanged(newEnv);
        if (_rememberSelection)
        {
            PlayerPrefs.SetString(PrefsKey, newEnv.ToString());
            PlayerPrefs.Save();
        }
    }

    private void OnEnvChanged(ServerEnv env)
    {
        RefreshLabelAndColor(env);
    }

    private void RefreshLabelAndColor(ServerEnv env)
    {
        if (_envLabel == null) return;

        _envLabel.text = $"ENV:{env}";
        _envLabel.color = EnvToColor(env);

        _serverURL.text = _envService.GetBaseUrl();
    }

    private Color EnvToColor(ServerEnv env)
    {
        return Color.grey;
        switch (env)
        {
            //case ServerEnv.DEV:  return new Color(1f, 0.85f, 0.2f);   // 노랑
            //case ServerEnv.QA:   return new Color(0.2f, 0.8f, 1f);    // 늘색
            //case ServerEnv.LIVE: return new Color(0.2f, 1f, 0.4f);    초록
            //default:              return Color.;
        }
    }

    public void OnApplicationQuit()
    {
        if (_rememberSelection && _envDropdown != null)
        {
            var currentEnv = (ServerEnv)_envDropdown.value;
            PlayerPrefs.SetString(PrefsKey, currentEnv.ToString());
            PlayerPrefs.Save();
        }

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 플레이모드 중지
#else
    Application.Quit(); // 실제 앱 종료
#endif
    }
}
