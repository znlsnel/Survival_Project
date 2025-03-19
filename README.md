# [Unity 7,8기] 게임개발 숙련 프로젝트(팀 - 조장님 군대갔조)

## 스토리

```
우주선을 타고 항해하던 당신은 갑작스러운 사고로 낯선 행성에 불시착합니다.거친 환경과 기괴한 몬스터들이 가득한 이곳에서 
살아남으려면 자원을 수집하고 장비를 제작해야 합니다.
또한 우주선을 수리할 부품과 연료를 찾아야만 탈출할 수 있습니다.
```

## 팀원(역할 분담)

| 진희원 | 정하나 | 김영송 | 이준범 | 천지훈 |
|----|----|----|----|----|
| 생존 관리 | 건축 시스템 | 인벤토리 시스템 | 플레이어 | 레벨 디자인 |
| 날씨 변화 | 자원 트레이딩 | 퀘스트 시스템 | 몬스터 및 전투 | 플랫폼 구현 |

## 프로젝트 구조

```
AAssets/
├── 0. External
├── 1. Scene
├── 2. InputSystem
├── 3. Animation
├── 4. Prefab
├── 5. Data
├── 6. Material
├── 7. Font
├── 8. UI
├── 9. Script
│   ├── Entity # 프로젝트 내 구현하고자 하는 기능 또는 대상 관련
│   │   ├── Player
│   │   ├── Monster
│   │   ├── Drone
│   │   └── ...
│   ├── Handler # 구현 대상의 핸들링을 위한 클래스들 또는 추가적 기능 관련
│   │   ├── BuildingHandler
│   │   └── InventoryHandler
│   └── Manager # 매니저 클래스들 주로 싱글톤 기반 전반 구조 관련
│       ├── GameManager
│       ├── QuestManager
│       └── WeatherManager
└── 10. Source
```

## 주요 기능

### 1. 건축 시스템 (Building System)

건축 모드 전환 시스템
- `PlayerManager`의 State Pattern을 활용하여 `NormalState`와 `BuildState`를 구현
- `InputManager`에서 `PlayerBuilding` 액션 맵을 사용하여 건축 모드 입력 처리
- `EventManager`를 이용해 건축 모드 전환 요청을 처리

건축 입력 처리 (`BuildingInputHandler`)
- `RotateObject (Q, E)`, `PlaceObject (좌클릭)`, `CancelBuild (우클릭)` 등 입력 처리
- `InputManager`에 `BuildingInputHandler`를 컴포넌트로 추가하여 중앙 집중식 입력 관리 구조 구성

건축물 데이터 관리 (`BuildingData`)
- `ScriptableObject`를 활용하여 건축물 데이터 관리

건축 UI 시스템
- 건축 모드 전환 시 UI 활성화 / 비활성화
- 건축 가능/불가능한 영역 표시 (프리뷰 시스템 포함)

건축물 배치
- `BuildingManager`를 싱글톤으로 구현하여 건축물 프리뷰 및 배치 관리
- 프리뷰 모드는 선택된 오브젝트의 머터리얼을 전부 가져와서 색을 바꿔줌
- 건축물이 바닥과 스냅되도록 Raycast 기반 배치 시스템 구현

---

<img src="https://github.com/user-attachments/assets/884d293e-eaab-4657-aa64-f067b0f1cf66" width="400" height="300">

### 2. 교환 시스템 (Storage & Trading System)

박스 인벤토리 시스템 (`BoxInventory`)
- 드론이 채집한 자원을 박스에 저장할 수 있도록 구현
- 박스에 저장된 아이템을 UI에 표시하는 기능 추가

박스 UI (`StorageUI`)
- `BoxInventory`에서 아이템을 가져와 UI에 표시
- `EItemType`별 필터링 가능하도록 Toggle 시스템 적용
- 박스 열 때 ui 업데이트

아이템 스택 시스템 개선
- `ItemDataSO`에서 스택 가능 여부 (`CanStackItems`) 설정
- 같은 아이템일 경우 개수만 증가하도록 Dictionary 활용하여 최적화
- `StorageSlot`을 활용하여 개별 아이템의 툴팁 및 개수 표시 개선

드론이 자동으로 채집한 아이템을 박스에 저장
- `DroneAI`가 `BoxInventory`에 아이템을 저장

---

<img src="https://github.com/user-attachments/assets/51326850-9efd-45f8-a580-ce3391be34ac" width="400" height="300">

### 3. 인벤토리 시스템
  - 인벤토리 : 플레이어 인벤토리 구현 및 UI 연결 (아이템 퀵슬롯 관련 ) - 드래그앤 드랍
  - 퀵슬롯 : 사용할 아이템을 키 입력을 통해 선택하는 기능
```
    - 퀵슬롯 구현 → 각 슬롯에 Item Data를 넣어놓는 방식
    - 1 ~ 9번 키를 입력 → 해당 index에 있는 item Data 찾아서 플레이어에 적용
    - 손에 쥘 수 있는 Prefab이 있다면 생성하여 사용
```
---

<img src="https://github.com/user-attachments/assets/504e9da7-d243-408b-af3c-a25efbf96ea8" width="400" height="300">

### 4. 퀘스트 시스템

- Quest Data를 통해 퀘스트를 받고 수행하는 기능 구현
- 퀘스트 완료시 아이템 습득 및 다음 퀘스트 진행 되는 방식
```
  - 퀘스트 시스템 구현 → QuestData에 퀘스트의 Type과 Target을 넣었습니다.
  - ex) 나무 10개 습득 → Type : Pickup, Target : 나무, targetCnt : 10
  - 아이템 습득이 일어나는 곳에서 해당 Type과 Target을 QuestManager에 보고
  - QuestManger에서 퀘스트 진행도 체크
  - 완료 되었다면 보상 지급 및 다음 퀘스트 진행
```

---

<img src="https://github.com/user-attachments/assets/bba0b64c-63ad-4952-a681-35e538a4462b" width="400" height="300">

### 5. 플레이어 컨디션

- **상태 관리:**
    - `UICondition`에서 `health`, `hunger`, `thirsty`, `stamina`, `temperature` 값을 가져와 플레이어 상태를 관리
- Update
  - 배고픔, 갈증, 스태미나를 일정 속도로 감소/증가
  - 특정 조건(배고픔, 갈증, 온도)에 따라 체력이 감소
  - 체력이 0 이하가 되면 `Die()` 메서드를 호출
- **체온 시스템:**
  - 날씨와 낮/밤의 영향을 받아 체온이 변함
    
- **기능 메서드:**
  - `Heal()`, `Eat()`, `Drink()` 체력, 배고픔 등을 회복할 수 있는 메서드
  - `UseStamina()`는 스태미나를 소모, `Rest()`는 체온을 회복
- **사망 처리:**
  - `Die()` 메서드를 통해 플레이어가 죽었을 때 로그를 출력
  - `GetPercentage()`: 현재 값을 0~1 사이 비율로 반환하여 UI 반영.

컨디션
- 체력, 허기, 목마름, 체력 회복 등의 상태를 관리하는 클래스.
- `curValue`: 현재 값, `maxValue`: 최대 값, `startValue`: 초기 값.
- `passiveValue`: 시간이 지남에 따라 변화하는 기본 값.
- `uiBar`: 상태를 시각적으로 표시하는 UI 슬라이더.
- `Add(amount)`: 상태 증가 (최대 값 초과 방지).
- `Subtract(amount)`: 상태 감소 (0 이하 방지).

---

<img src="https://github.com/user-attachments/assets/5f50d5a8-541a-44dc-be41-e69dd640cb63" width="400" height="300">

### 6. 날씨 시스템


UICondition
- 플레이어 상태 관리**
    - `health`, `hunger`, `thirsty`, `stamina`, `temperature`
- UI 초기 설정 (`Start`)
    - `GameManager.Instance.PlayerController`에서 `PlayerCondition`을 가져와 `UICondition`을 연결
    - `temperatureBar`의 `fillRect`에서 `Image` 컴포넌트를 가져옴
    
- 체온 UI 업데이트 (`Update`)
    - `temperature.curValue` 값을 0~100 범위로 정규화하여 체온 바(`Slider`)에 적용
    - 체온 값에 따라 색상을 **파랑 → 초록 → 빨강**으로 변화시켜 시각적으로 온도 변화를 표현

Weather
- 날씨 상태를 `Sunny`, `Rainy`, `Hot`, `Snow` 네 가지로 설정
- `SetRandomWeather()`: 1~100 사이 난수를 생성해 25% 확률로 각 날씨를 결정
- 게임 시작 시 `Start()`에서 날씨를 랜덤하게 설정

UIWeather
- 날씨 UI 관리
    - `sunny`, `rainy`, `hot`, `snow` 게임 오브젝트를 통해 현재 날씨 상태를 UI로 표시
- 날씨 효과 적용 (`Update`)
    - `Weather` 클래스에서 `currentWeather` 값을 가져와 현재 날씨를 확인
    - 모든 날씨 UI 요소를 초기화한 후 현재 날씨에 맞는 UI를 활성화
    - `rainParticle` 및 `snowParticle`이 존재하면 해당 날씨에서 파티클 효과 재생
- 파티클 위치 조정 (`LateUpdate`)
    - 카메라의 위치를 기준으로 `rainParticle`과 `snowParticle`의 위치를 조정하여 플레이어 이동 시에도 파티클이 따라오도록 설정

---

<img src="https://github.com/user-attachments/assets/351311c9-6ad8-4cdb-90f3-95a890ca1df7" width="400" height="300">

### 7. 전투 시스템

액터(플레이어, 몬스터) 관련 클래스 구현
- 구조(컨트롤러 → 각 핸들러)
- 플레이어와 같은 구조(컨트롤러 → 각 핸들러)
- 모델 휴머노이드 기본 모델을 기준으로 공통 애니메이터와 애니메이션으로 디폴트 설정
(장점: 휴머노이드 모델과 기존 핸들러 로직의 확장만으로 새로운 몬스터 구현 가능)

플레이어 구현
- subStateMachine을 통해 콤보 공격 구현
- 애니메이션 블렌딩과 StateMachineBehaviour 를 통해 자연스러운 모션 구현

몬스터 구현
- 오버라이드 애니메이터를 통해 애니메이션 확장, 기본 클래스 확장하여 행동 패턴 로직 추가
- 애니메이션 이벤트와 타격 유효 판정 클래스를 통해 공격 유효 판정 체크

몬스터 종류
- 일반병 : 기본 형태의 몬스터(플레이 감지 및 범위 내 공격)
- 마법사 : 범위 내 플레이어 존재 여부를 판정으로 마법 공격
- 투척병 : 타격 판정 클래스를 컴포넌트로 가진 투사체를 발사하여 공격

---

<img src="https://github.com/user-attachments/assets/cca60faa-551d-4049-977d-573daef59472" width="400" height="300">

### 8. 레벨 디자인

나침반 기능: 맵내에서 방향을 알려주는 나침반
- 현재 플레이어의 y축 회전 값 가져오기
- 이전 프레임과 비교하여 회전 변화량(Δ각도) 계산
- 변화량에 따라 나침반을 좌우로 이동

컷씬
- `Cinemachine`을 활용하여 드롭십 착륙 컷씬 & 엔딩 컷씬 구현
- 페이드 인/아웃 (`CanvasGroup`을 활용한 페이드 효과)
  
드롭십 착륙 컷씬
- `CutScene`에서 드롭십이 목표 지점까지 이동 후 폭발하는 연출 추가
- `CameraShake`를 `CinemachineTransposer`를 활용하여 구현
- `Time.timeScale`을 활용한 슬로우 모션 효과 추가
  
엔딩 컷씬 (비행기 이륙 연출)
- `Cinemachine` 카메라가 목표 지점까지 따라간 후 멈추도록 설정

