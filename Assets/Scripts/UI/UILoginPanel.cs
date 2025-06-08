using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using LoginSystem.Core;
using LoginSystem.Inerface;
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
        }

        private void OnDestroy()
        {
            loginButton.onClick.RemoveListener(OnLoginButtonClick);
        }

        private void OnLoginButtonClick()
        {
            onLogin();
        }
        
        IAuthService _authService;
        IUserService _userService;
        
        [Inject]
        public void Construct(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;

            Debug.Log("Construct");
        }

        private async UniTask onLogin()
        {
            string id = usernameInput.text;
            string pw = passwordInput.text;
            
            // async 호출 (비동기 실행)
            var loginParam = new LoginParam { Username = id, Password = pw };
            var loginExecutor = new WorkExecutor<LoginParam, LoginResult>(new LoginWork(_authService, _userService));
            var result = await loginExecutor.RunAsync(loginParam);

            if (result.IsSuccess)
            {
                Debug.Log($"{result.ErrorMessage}");
                await SceneManager.LoadSceneAsync("MainScene").ToUniTask();
            }
            else
            {
                Debug.Log($"result fail : msg => {result.ErrorMessage}");
                //todo 실패 UI
            }
        }


    }
}

