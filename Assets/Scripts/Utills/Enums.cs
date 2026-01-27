

public enum LoginErrorCode
{
    None,
    InvalidToken,
    InvalidUserData,
    EmptyInput,
    AuthFailed,
    NetworkError,
    Unknown
}

//계정 상태
public enum AuthState
{
    Create, //가입
    Login,
    Logout,
    UserData,
}


public enum ServerEnv
{
    DEV,
    QA,
    LIVE
}

public enum AuthError
{
    None = 0,
    InvalidEmail = 5,
    UserDisabled = 12,
    UserNotFound = 11,
    WrongPassword = 18,
    EmailAlreadyInUse = 10,
    OperationNotAllowed = 15,
    WeakPassword = 26,
    MissingPassword = 4,
    InvalidCredential = 20,
    NetworkRequestFailed = 7,
    TooManyRequests = 8,
}

public enum ClientAuthError
{
    None = 0,

    // 입력값
    InvalidEmail,
    MissingPassword,

    // 계정
    UserNotFound,
    EmailAlreadyInUse,
    UserDisabled,

    // 인증
    WrongPassword,
    InvalidCredential,
    WeakPassword,

    // 네트워크 / 제한
    NetworkError,
    TooManyRequests,
    OperationNotAllowed,

    // 기타
    Unknown
}
