# Unity Login Sample Project

개요
- 프로젝트명: Unity Login Sample  
- 개발 환경: Unity 2022.2 / C# / VContainer  
- 목적: 
1. DI구조를 통해 구현에 의존성을 낮추고 환경(DEV/QA/LIVE)에 변경이 유연하게 가능하도록 구현
2. 인터페이스를 활용해 의존성과 결합도를 낮추어 유지보수성을 살리는 작업 패턴 

- ## Features
- **ToastMsg**: 코루틴 기반 토스트(최대 3개 동시, 큐 처리, 중앙 정렬)
- **Environment Switch**: `UIHUDInfo` 드롭다운으로 DEV/QA/LIVE 실시간 전환
- **VContainer**: `IAuthService` 라우터로 Mock/Real 전환
- **UniTask**: `LoginWork.ExecuteWorkAsync` 비동기 로그인
---

주요 기능

1. 환경 전환 시스템 (Environment Switching)
- 개발 / 스테이징 / 운영 서버 전환 지원  
- `IEnvironmentService` 인터페이스로 서비스 의존성 분리  
- UI 드롭다운에서 선택 → 자동으로 API Endpoint 반영  
- PlayerPrefs 기반 환경 저장 기능 (편의)

2. 로그인 인증
- `AuthService`, `AuthServiceRouter` 를 통한 모듈화된 인증 흐름  
- `LoginWork` 를 Work 패턴 기반으로 분리  
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

```
## 주요 클래스 구조

Scripts/
├── Core/
│   ├── LoginController.cs    // 로그인 결과 캐싱용(아직 별다른 기능없음)
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

```
## 단위 테스트 (Unit Tests)

**EditMode**와 **PlayMode** 환경에서 각각 독립적인 단위 테스트를 수행합니다.  
테스트 프레임워크는 `NUnit + Unity Test Framework + UniTask` 기반으로 구성되어 있습니다.

---

### EditMode Tests  
> **목적:** 런타임 의존성이 없는 순수 로직 및 DI 구성을 검증
> **테스트 스크린샷:**  

<p align="left">
  <img width="702" height="298" alt="EditModeTests" src="https://github.com/user-attachments/assets/f7121139-3966-4823-903a-026fc9b170dc" />
</p>

| 테스트명 | 설명 |
|-----------|------|
| **Container_Resolves_Router_Only** | DI 컨테이너 구성 검증 — `IAuthService`가 오직 `AuthServiceRouter`로 Resolve되는지 확인 |
| **DefaultEnv_Is_DEV** | `EnvironmentService`의 초기 환경값이 `DEV`로 올바르게 설정되는지 검증 |
| **Router_Env_Change** | 환경 변경 시 `AuthServiceRouter`가 `MockService` → `AuthService`로 정상 전환되는지 확인 |

> EditMode 테스트는 실제 실행(Play)용

---

### PlayMode Tests  
> **목적:** 실제 런타임 환경에서 비동기 로그인 로직 및 실패 케이스를 검증
> **테스트 스크린샷:**  

<p align="left">
  <img width="702" height="233" alt="PlayModeTests" src="https://github.com/user-attachments/assets/0340e69d-1a78-488d-8993-ce24f2a3d244" />
</p>

| 테스트명 | 설명 |
|-----------|------|
| **Login_Succeed_Test** | 정상 입력(`ID`, `PW`) 시 `MockService`를 통해 로그인 성공 결과 반환 확인 |
| **Login_Failed_ID_Empty** | 아이디가 비어 있을 경우 로그인 실패 반환 검증 |
| **Login_Failed_PW_Empty** | 비밀번호가 비어 있을 경우 로그인 실패 반환 검증 |
| **Login_Failed_IDPW_Empty** | 아이디·비밀번호 모두 비었을 때 실패 반환 검증 |

> PlayMode 테스트는 `UniTask` 기반 비동기 흐름을 `UnityTest`로 감싸 실제 런타임 시퀀스를 그대로 재현하며,  
> UI·네트워크 모의(Mock) 기반 로직이 올바르게 작동하는지 보장

---

### 테스트 환경 요약
- **Framework:** `NUnit`, `UnityEngine.TestTools`, `UniTask`
- **DI 컨테이너:** `VContainer`
- **테스트 분류:**  
  - `EditMode`: 순수 서비스 로직 / DI 구성  
  - `PlayMode`: 비동기 로그인 / Mock 라우팅 / 실패 케이스

> 모든 테스트는 `Test Runner` 창에서 **Run All** 실행으로 전체 테스트 케이스 한번에 체크 가능

## 구현 GIF
**서버 선택**
![Honeycam 2025-10-29 14-28-41](https://github.com/user-attachments/assets/68e25204-4584-48f2-ae02-f04ba4de92d8)

**로그인**
선택한 서버에 주소를 가지고 로그인 시도 합니다.
![Honeycam 2025-10-29 14-31-17](https://github.com/user-attachments/assets/fe8bf8eb-79ac-47b6-a0ff-b0c0e5d4f776)

**로그아웃**
![Honeycam 2025-10-29 14-31-42](https://github.com/user-attachments/assets/07c5c9d3-1346-49e6-ab04-1bba1b6a1f08)

프로젝트 다운로드
[https://drive.google.com/file/d/1lueWrALwM0zkHWwO_FYcZvB-ZyYBtVQw/view?usp=sharing](https://drive.google.com/file/d/1BucKwFBM9PxQlmjhHx8j5CMJJdyk8lA5/view?usp=sharing)

//UI 사용 : https://kenney.nl/

