using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static ClientAuthError ConvertFirebaseError(int errorCode)
    {
        switch ((AuthError)errorCode)
        {
            case AuthError.InvalidEmail:
                return ClientAuthError.InvalidEmail;

            case AuthError.MissingPassword:
                return ClientAuthError.MissingPassword;

            case AuthError.UserNotFound:
                return ClientAuthError.UserNotFound;

            case AuthError.EmailAlreadyInUse:
                return ClientAuthError.EmailAlreadyInUse;

            case AuthError.UserDisabled:
                return ClientAuthError.UserDisabled;

            case AuthError.WrongPassword:
                return ClientAuthError.WrongPassword;

            case AuthError.InvalidCredential:
                return ClientAuthError.InvalidCredential;

            case AuthError.WeakPassword:
                return ClientAuthError.WeakPassword;

            case AuthError.NetworkRequestFailed:
                return ClientAuthError.NetworkError;

            case AuthError.TooManyRequests:
                return ClientAuthError.TooManyRequests;

            case AuthError.OperationNotAllowed:
                return ClientAuthError.OperationNotAllowed;

            default:
                return ClientAuthError.Unknown;
        }

    }

    public static string GetAuthErrorMessage(int errorCode)
    {
        switch ((AuthError)errorCode)
        {
            case AuthError.InvalidEmail:            // 5
                return "이메일 형식이 올바르지 않습니다.";

            case AuthError.MissingPassword:         // 4
                return "비밀번호를 입력해주세요.";

            case AuthError.WeakPassword:            // 26
                return "비밀번호가 너무 약합니다. 6자 이상으로 설정해주세요.";

            case AuthError.EmailAlreadyInUse:       // 10
                return "이미 사용 중인 이메일입니다.";

            case AuthError.UserNotFound:            // 11
                return "존재하지 않는 계정입니다.";

            case AuthError.WrongPassword:           // 18
                return "비밀번호가 올바르지 않습니다.";

            case AuthError.UserDisabled:            // 12
                return "비활성화된 계정입니다. 고객센터에 문의해주세요.";

            case AuthError.InvalidCredential:       // 20
                return "인증 정보가 유효하지 않습니다. 다시 로그인해주세요.";

            case AuthError.OperationNotAllowed:     // 15
                return "현재 이 로그인 방식은 사용할 수 없습니다.";

            case AuthError.NetworkRequestFailed:    // 7
                return "네트워크 연결이 불안정합니다. 인터넷 상태를 확인해주세요.";

            case AuthError.TooManyRequests:         // 8
                return "요청이 너무 많습니다. 잠시 후 다시 시도해주세요.";

            default:
                // 디버깅용으로 코드도 같이 노출하고 싶으면 아래처럼:
                // return $"인증 오류가 발생했습니다. (코드: {errorCode})";
                return "인증 오류가 발생했습니다. 잠시 후 다시 시도해주세요.";
        }
    }
}