using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LoginSystem.Core;
using LoginSystem.Interface;
using LoginSystem.Service;

using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

namespace LoginSystem.UI
{
    public class LoginPanel : MonoBehaviour
    {
        [SerializeField] private InputField usernameInput;
        [SerializeField] private InputField passwordInput;
        [SerializeField] private Button loginButton;
        
        private void Awake()
        {
            loginButton.onClick.AddListener(OnLoginButtonClick);
            
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.SetResolution(1280, 720, false);
        }

        private void OnDestroy()
        {
            loginButton.onClick.RemoveListener(OnLoginButtonClick);
        }

        private void OnLoginButtonClick()
        {
            onLogin();
        }
        
        LoginWork _loginWork;
        
        [Inject] //mono
        public void Construct(LoginWork loginWork)
        {
            _loginWork = loginWork;

            Debug.Log("Construct");
        }

        private bool _isLoggingIn = false;
        private async UniTask onLogin()
        {
            // 중복 클릭 방지
            if (_isLoggingIn)
            {
                ToastMsg.Show("로그인 처리 중입니다.");
                return;
            }

            string id = usernameInput.text;
            string pw = passwordInput.text;
            
            //기본 검증: 
            //    (규칙은 필요에 맞게 조정해도 됨)
            if (string.IsNullOrWhiteSpace(id))
            {
                ToastMsg.Show("아이디를 입력해 주세요.");   // 큐에 쌓임
                return;
            }
            if (string.IsNullOrWhiteSpace(pw))
            {
                ToastMsg.Show("비밀번호를 입력해 주세요.");
                return;
            }
            _isLoggingIn = true;

            // async 호출 (비동기 실행)
            var loginParam = new LoginParam { UserID = id, Password = pw };
            
            var result = await _loginWork.ExecuteWorkAsync(loginParam);
            if (result.IsSuccess)
            {
                ToastMsg.Show("로그인 성공",10);
                await UniTask.Delay(3000); 
                
                Debug.Log($"login succeed {result.ErrorMessage}");
                LoginController.CacheLoginResult(result);
                await SceneManager.LoadSceneAsync("LogoutScene").ToUniTask();
            }
            else
            {
                _isLoggingIn = false;
                Debug.Log($"result fail : msg => {result.ErrorMessage}");
                //todo 실패 UI
            }
        }
    }
}

