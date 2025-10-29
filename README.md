# Unity Login Sample Project

개요
- 프로젝트명: Unity Login Sample  
- 개발 환경: Unity 2022.2 / C# / VContainer  
- 목적: 환경별 로그인 인증 및 단위 테스트 구조를 갖춘 로그인 래퍼 샘플 구현  

---

주요 기능

1. 환경 전환 시스템 (Environment Switching)
- 개발 / 스테이징 / 운영 서버 전환 지원  
- `IEnvironmentService` 인터페이스로 서비스 의존성 분리  
- UI 드롭다운에서 선택 → 자동으로 API Endpoint 반영  
- PlayerPrefs 기반 환경 저장 기능

2. 로그인 인증
- `AuthService`, `AuthServiceRouter` 를 통한 모듈화된 인증 흐름  
- `LoginController` 와 `LoginWork` 를 Work 패턴 기반으로 분리  
- 실패 시 Toast 메시지, 예외 처리 등 UI 피드백 처리

3. 의존성 주입 (VContainer)
- `IEnvironmentService`, `IAuthService` 등 모든 서비스는 DI로 관리  
- 런타임에서 Mock / Real 환경 교체 가능  
- 테스트, 프로덕션 환경 분리 설계

4. UI 구조
- `UILoginPanel`: ID / PW 입력 및 버튼 이벤트 처리  
- `UIHUDInfo`: 현재 환경 및 서버 URL 표시  
- `ToastMsg`: 사용자 피드백 메시지 표시용 컴포넌트

5. 단위 테스트
- `LoginUniTest` 에서 로그인 검증 흐름 테스트  
- Mock 서비스 주입으로 네트워크 독립 테스트 가능

---
## 주요 클래스 구조
주요 클래스 구조

Scripts/
├── Core/
│   ├── LoginController.cs    // 로그인 흐름 제어
│   └── LoginWork.cs          // 실제 로그인 비즈니스 로직
├── Service/
│   ├── AuthService.cs        // 서버 로그인 처리
│   ├── AuthServiceRouter.cs  // 서버 타입별 라우팅
│   └── EnvironmentService.cs // 서버 환경 관리
├── UI/
│   ├── UILoginPanel.cs       // 로그인 패널 UI
│   ├── UIHUDInfo.cs          // 환경/서버 표시 UI
│   └── ToastMsg.cs           // 토스트 메시지
├── Interface/
│   └── Interface.Service.cs  // 서비스 인터페이스 정의
├── Utills/
│   ├── DebugGUI.cs           // 디버그용 GUI
│   └── Enums.cs              // 열거형
└── Test/
    └── LoginUniTest.cs       // 단위 테스트

## 구현 GIF
**서버 선택**
![Honeycam 2025-10-29 14-28-41](https://github.com/user-attachments/assets/68e25204-4584-48f2-ae02-f04ba4de92d8)

**로그인**
선택한 서버에 주소를 가지고 로그인 시도 합니다.
![Honeycam 2025-10-29 14-31-17](https://github.com/user-attachments/assets/fe8bf8eb-79ac-47b6-a0ff-b0c0e5d4f776)

**로그아웃**
![Honeycam 2025-10-29 14-31-42](https://github.com/user-attachments/assets/07c5c9d3-1346-49e6-ab04-1bba1b6a1f08)

//UI 사용 : https://kenney.nl/
