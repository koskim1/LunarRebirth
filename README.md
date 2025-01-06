# <div align=center>  Lunar Rebirth: Dungeon of Eternal Return </div>

<div align=center><img src="https://github.com/user-attachments/assets/121345ff-2a70-476b-a5c4-124a49ed4ebd" width="600" height="300" /> </div>

<div align=center> 스팀페이지 : https://store.steampowered.com/app/3405580/Lunar_Rebirth_Dungeon_of_Eternal_Return/</div>
<hr>
루나리버스는 던전 탐험과 보스 전투를 중심으로 한 1인으로 개발한 액션 RPG 게임입니다.

절차적으로 생성되는 던전과 다단계 보스 전투를 통해 플레이어에게 새로운 도전을 제공합니다.  
|제목|내용|
|------|---|
|타이틀 씬|<div align=center><img src="https://github.com/user-attachments/assets/1227fe2f-32e0-480e-87ed-ddd900b54048" width="600" height="300" /> </div>
|카드 시스템(로그라이크)|<div align=center><img src="https://github.com/user-attachments/assets/3d46be5c-f3be-41df-bdf9-9c5cb0eefcab" width="600" height="300" /> </div>|
|절차적 던전 생성|<div align=center><img src="https://github.com/user-attachments/assets/daba4cb7-9efb-4be7-9c3f-9623b32331b1" width="600" height="200" /> </div>|
## 🛠️ 기술 스택
- **Engine**: Unity 2022.3.15  
- **Language**: C#  
- **Libraries**: DOTween, NavMesh, etc.  

## 🎮 주요 특징
- **DFS 기반 랜덤 던전 생성**: 플레이할 때마다 새로운 경험.
- **보스 전투 시스템**: AI 및 페이즈 기반의 다양한 공격 패턴.
- **몰입감 있는 스토리**: Unity Timeline으로 구현된 컷씬.
- **커스터마이징 가능한 성장 시스템 / 로그라이크 요소**.
- **NPC상점과 플레이어에게 친절한 튜토리얼**
- **게임 내 지원 언어 : 영어 / 한글**

## 👜 폴더 소개
- *Assets/_Scripts 내부 폴더*
- **0_Title 및 Managers** : 게임의 전반적인 흐름과 데이터를 관리하는 핵심 스크립트를 포함합니다. (씬 전환, 싱글톤 관리, UI 상태 제어 등)
- **1_Loading** : 비동기식 로딩 및 fake 로딩 기법사용.
- **2_UI** : 적들 및 본인의 HealthBar와, 타격 시 ui데미지효과, 레벨 업 및 플레이어와의 인터페이스를 구현하는 요소들이 포함된 폴더입니다.
- **_Player** : 기본움직임 및 애니메이션, 능력, LockOn기능, 독백, 게임 내 MLP관리.
- **Boss** : 보스전 관련 다단계 페이즈와, NavMeshAgent를 이용한 추적 및 공격 AI구현.
- **Camera** : 기본적인 collider detection과 미니맵관련 스크립트.
- **Card** : 게임의 주 성장요소인 레벨업 시 고를 수 있는 카드들을 관리하는 스크립트들 입니다. ( 확률기반 카드생성, SO연동 )
- **Dialogue** : NPC혹은 플레이어 독백 시 사용되는 스크립트 입니다.
- **Enemies** : 적 관련 AI행동 및 애니메이션
- **Interface** : LockOn구현시 사용했던 인터페이스 스크립트
- **MapGeneration** : DFS로 구현된 절차적 던전이 구현되어 있으며, 각 방의 상태에 따라 던전이 알맞게 바뀌도록 구현했습니다.
- **NPC** : ShopNpc와 Tuto_Npc관련 스크립트
- **Shop** : ShopNpc와 연동해서 Shop의 SO관련 물품들을 연동해 두었습니다.

주요 스크립트들은 위 폴더에 모두 있고, 연동되고 있는 ScriptableCards, ShopItem는   
*Assets/ScriptableCards*   
*Assets/ShopItem* 에 있습니다.

<br>
<hr>

![Image Sequence_008_0003](https://github.com/user-attachments/assets/a13c3475-de7f-4b6b-9071-8742c50ae913)
더욱 자세한 게임의 정보는 스팀페이지에서 확인하실 수 있습니다.
