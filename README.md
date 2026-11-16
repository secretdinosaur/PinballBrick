# PinballBricks

[![Google Play](https://img.shields.io/badge/Google_Play-Store-414141?logo=googleplay)](<Google Play 스토어 링크 필요>)
[![Unity](https://img.shields.io/badge/Unity-6000.0.60f1-black?logo=unity)](https://unity.com/)

<대표 이미지(스토어 그래픽 또는 타이틀 화면)에 대한 사진 필요>

**[English](#english)** | **[한국어](#korean)**

| Gameplay | Augment selection | Boss wave |
| :---: | :---: | :---: |
| <기본 플레이(플리퍼로 공을 튕겨 블록을 깨는 장면)에 대한 사진 필요> | <레벨업 증강 선택 화면에 대한 사진 필요> | <보스 웨이브와 제한 시간 타이머에 대한 사진 필요> |

<br>

<a id="english"></a>

## English

> Development period: January 2025 – July 2025 (about 7 months, 2-person team)

A mobile survivors-like that combines pinball and brick breaker. You control the flippers at the bottom, bounce balls into bricks, and build your run from level-up stats and 130+ augments. It is integrated with Google Play Games for a global leaderboard, achievements, and cloud saves.

### Team

| Member | Main areas |
| --- | --- |
| 송우진 | Augment system, brick/wave system, ball and stats, level-up flow, Google Play Games integration (leaderboard, achievements, cloud save) |
| 이주호 | Shader Graph effects, sound, items (gold, EXP, dice), UI design |

<br>

### Gameplay

- **Core loop**: Hold the left/right flipper buttons to hit the balls into bricks. If no ball is left on the field for 3 seconds, the game is over.
- **Waves**: Each wave announces its number on screen. Every 10th wave is a boss wave; the next wave does not start until the boss is destroyed, and failing to kill it within 60 seconds ends the game.
- **Bricks**: A basic brick plus 9 ability bricks (Heal, Hard, Slime, Shield, Teleport, Ignore, Absorb, Twin, Reborn), and 3 rare special bricks (Gold, EXP, Ball) that reward gold, experience, or extra balls.
- **Growth**: Level 1 and every 5th level offer 3 augments; all other levels offer 3 random stats out of 8 (Attack Damage, Critical Damage, Critical Chance, Effect Damage, Effect Duration, Gold Gain, EXP Gain, Summon Balls).
- **Fever**: Destroying bricks in quick succession raises a score multiplier (up to x16) that is paid out when the gauge runs out.

| Level-up stats | Special bricks | Fever |
| :---: | :---: | :---: |
| <레벨업 스탯 선택 화면에 대한 사진 필요> | <골드·경험치·공 스페셜 블록에 대한 사진 필요> | <피버 배수가 올라가는 장면에 대한 사진 필요> |

<br>

### Technical Implementation

#### 1. Hook-Based Augment System (133 Augments)

- Every augment implements `IAugment`, which exposes hooks with empty default bodies: `BeforeAttackEffect`, `AfterMainAttackEffect`, `CriticalAttackEffect`, `AfterAttackEffect`, `AfterBrickDieEffect`, `AfterBirthEffect` / `AfterDeathEffect` (ball), and `AfterLevelUpEffect`.
- `DamageManager` and the ball/brick/level managers simply iterate over the active augment list and call the matching hook. An augment overrides only the hooks it needs, so adding one takes a single class and one line in the registration list.
- Augments are split into three base classes: `StatAugment` (flat stat changes), `FieldAugment` (field-wide effects with a shared cooldown), and `SpecialAugment` (fire, poison, decay, lightning, curse, bomb, and other on-hit effects).
- Long-running effects are registered by key in `AugmentRoutine`, a small coroutine registry, so an augment's coroutine can be replaced or stopped when the augment is removed.

#### 2. Rarity Rolls and Follow-Up Augments

- Rarity runs from grade 5 down to grade 0. A grade is rolled first from a weight table (grade 0: 0.5%, grade 1: 4.5%, grade 2: 10%, grade 3: 25%, grade 4: 40%, grade 5: 20%), then one augment of that grade is picked uniformly.
- Augments flagged `isFollowUpAugment` start in a locked list and never appear. When a base augment is applied it calls `UnlockAugmentByType`, e.g. taking **FireBall** unlocks every `"Fire"` type augment, so the fire-duration and fire-explosion augments start appearing in later choices. Removing the base augment locks them again.
- An augment that has been picked is locked, and the three options shown at once are tracked so they never duplicate.

<화염공 획득 후 연계 증강이 등장하는 장면에 대한 사진 필요>

#### 3. Wave Generation and Balancing

Brick composition and strength are derived from the wave number by formulas, with coefficients tuned in an Excel sheet.

- **Brick type weights**: each type gets a Gaussian weight centered 20 waves apart (σ = 55), so the mix shifts gradually toward harder bricks instead of switching abruptly. Each type is also gated behind an unlock wave (every 10 waves); the weight of still-locked types is redistributed to the basic and heal bricks.
- **Count and HP**: a wave spawns `10–15 + min(wave / 10, 10)` bricks. Normal brick HP scales with `(wave + 5·tier + 1)^1.1`, boss HP with the exponent 1.2 and roughly a 5× multiplier, each with a small random spread.
- **Boss type**: decided by the wave band (Hard for waves 11–20, Slime for 21–30, and so on), so each boss introduces the mechanic of that stretch.
- **Special bricks**: from wave 5 the spawn chance accumulates with every brick and resets to 0 once one spawns, which keeps them rare but prevents long droughts.
- **Rapid waves**: after wave 10 a wave can be a Rapid wave with a much shorter spawn interval. Its chance grows by 9% per normal wave and resets when it triggers.

#### 4. Grid Placement and Reflection-Based Pooling

- The field is a 6 × 10 grid. Bricks take a random free cell, and the cell is returned when the brick dies. When no cell is free, spawning pauses.
- Brick types are listed by name in a `BrickInfo` ScriptableObject. The spawner resolves the type with `Type.GetType` and calls the generic `GetFromPool<T>` / `TakeToPool<T>` of the pool manager through `MakeGenericMethod`, so a new brick type needs no change in the spawner.

#### 5. Google Play Games Integration and Saving

- **Leaderboard**: the score is reported on game over. The ranking tab loads the top 100 in pages of 25 with `LoadScores` / `LoadMoreScores`, plus the player-centered rank. Without a connection it falls back to the local high score.
- **Achievements**: 21 Google Play Games achievements, each wrapped in a small class that reports progress from the run statistics.
- **Cloud save**: shop upgrades, gold, and lifetime statistics are serialized to JSON and stored with Saved Games. If the player is not signed in or the request fails, the same JSON is written to a local file instead.
- **Daily missions**: 3 missions are drawn from a mission database and reset at 00:00 UTC (9 AM KST), with a countdown shown in the UI. Achievements accumulate statistics across runs and pay gold each time a tier is passed.

<br>

### Out-of-Game Screens

A bottom menu switches between Shop, Missions & Achievements, Main, Ranking, and Settings.

| Screen | Details |
| --- | --- |
| Shop | Spend gold on starting stats: Attack Damage, Effect Damage, Bonus EXP, Critical Chance, Critical Damage, and the number of starting balls. Upgrades can be reset for a full refund. |
| Missions & Achievements | Daily missions and cumulative achievements, both rewarding gold |
| Main | Start a run. Paying 1,000 gold grants one random augment to start with. |
| Ranking | Global leaderboard, or the local record when offline |
| Settings | BGM and SFX volume |

| Shop | Missions & Achievements | Ranking |
| :---: | :---: | :---: |
| <상점 화면에 대한 사진 필요> | <미션과 업적 화면에 대한 사진 필요> | <랭킹 화면에 대한 사진 필요> |

<br>

### Troubleshooting

| Problem | Fix |
| --- | --- |
| A run could stall forever on a boss the build cannot kill | A 60-second timer starts when a boss spawns and ends the run when it expires |
| Losing the last ball ended the run even when an augment was about to respawn one | Game over only triggers after the field has had no ball for 3 seconds |
| Twin bricks need two cells and broke when only one was free | The brick roll is repeated when fewer than two cells are available |
| Waves piled up when the player could not keep up | The next wave waits up to 5 seconds while more than 5 bricks remain |

<br>

### Project Structure

```
Assets/
├─ Augment/       Augment manager, IAugment, Stat/Field/Special augment sets, special attack objects
├─ Ball/          Ball manager, ball types (Bomb, Ghost, Glass), persistent/temporary stats
├─ Brick/         Brick manager, damage manager, 10 brick types, special bricks
├─ Item/          Gold, EXP beads, dice
├─ Pin/           Flippers
├─ Shader/        Shader Graph effects, visual effect manager
├─ Sound/         Sound manager, BGM, SFX
├─ UI/            Level-up, achievements and missions, leaderboard, fever bar, damage text
├─ UpgradeShop/   Shop upgrades
├─ Modules/       Generic object pool module
└─ Scenes/        Title, Main, Game
```

<br>

### Tech Stack & How to Play

**Unity 6 (6000.0.60f1)** · C# · URP 2D · Shader Graph · Input System · DOTween · Redcode.Pools · Google Play Games Plugin for Unity

- **Play**: <Google Play 스토어 링크 필요>
- **Controls**: hold the left / right button to raise each flipper

<br>

---

<a id="korean"></a>

## 한국어

> 개발 기간: 2025년 1월 ~ 2025년 7월 (약 7개월, 2인 개발)

핀볼과 벽돌깨기를 합친 뱀서라이크 모바일 게임입니다. 아래쪽 플리퍼를 조작해 공을 튕겨 블록을 깨고, 레벨업 스탯과 130종 이상의 증강으로 자신만의 빌드를 만들어 갑니다. Google Play Games와 연동해 글로벌 랭킹, 업적, 클라우드 저장을 지원합니다.

### 팀 구성

| 팀원 | 주요 담당 |
| --- | --- |
| 송우진 | 증강 시스템, 블록·웨이브 시스템, 공과 스탯, 레벨업 흐름, Google Play Games 연동(랭킹·업적·클라우드 저장) |
| 이주호 | Shader Graph 이펙트, 사운드, 아이템(골드·경험치·주사위), UI 디자인 |

<br>

### 게임 플레이

- **코어 루프**: 좌우 플리퍼 버튼을 눌러 공을 쳐 올려 블록을 깹니다. 필드에 공이 하나도 없는 상태가 3초간 이어지면 게임 오버입니다.
- **웨이브**: 웨이브가 넘어갈 때마다 현재 웨이브를 화면에 표시합니다. 10의 배수 웨이브는 보스 웨이브로, 보스를 깨기 전에는 다음 웨이브가 시작되지 않고 60초 안에 깨지 못하면 게임 오버입니다.
- **블록**: 기본 블록과 능력 블록 9종(Heal, Hard, Slime, Shield, Teleport, Ignore, Absorb, Twin, Reborn)이 있고, 가끔 등장하는 스페셜 블록 3종(골드, 경험치, 공)은 각각 골드, 경험치, 추가 공을 줍니다.
- **성장**: 1레벨과 5레벨마다 증강 3개 중 하나를 고르고, 나머지 레벨에서는 스탯 8종(공격력, 치명타 공격력, 치명타 확률, 이펙트 공격력, 이펙트 지속시간, 골드 획득량, 경험치 획득량, 공 소환) 중 무작위 3개에서 하나를 고릅니다.
- **피버**: 블록을 연속으로 깨면 점수 배수가 최대 16배까지 오르고, 게이지가 다 닳을 때 누적 점수에 곱해 정산합니다.

| 레벨업 스탯 | 스페셜 블록 | 피버 |
| :---: | :---: | :---: |
| <레벨업 스탯 선택 화면에 대한 사진 필요> | <골드·경험치·공 스페셜 블록에 대한 사진 필요> | <피버 배수가 올라가는 장면에 대한 사진 필요> |

<br>

### 기술 구현

#### 1. 훅 기반 증강 시스템 (증강 133종)

- 모든 증강은 `IAugment`를 구현합니다. 인터페이스에는 `BeforeAttackEffect`, `AfterMainAttackEffect`, `CriticalAttackEffect`, `AfterAttackEffect`, `AfterBrickDieEffect`, 공의 `AfterBirthEffect` / `AfterDeathEffect`, `AfterLevelUpEffect` 훅이 빈 기본 구현으로 들어 있습니다.
- `DamageManager`와 공·블록·레벨 매니저는 활성 증강 목록을 순회하며 해당 시점의 훅만 호출합니다. 증강은 필요한 훅만 오버라이드하므로, 새 증강은 클래스 하나를 만들고 등록 목록에 한 줄을 추가하면 됩니다.
- 증강은 세 가지 추상 클래스로 나뉩니다. `StatAugment`는 스탯을 올리고, `FieldAugment`는 공용 쿨타임을 가지고 필드 전체에 영향을 주며, `SpecialAugment`는 화염, 독, 부식, 번개, 저주, 폭탄 같은 타격 효과를 담당합니다.
- 지속 효과는 코루틴 레지스트리인 `AugmentRoutine`에 키로 등록해, 증강이 제거되면 해당 코루틴을 교체하거나 중지할 수 있게 했습니다.

#### 2. 등급 추첨과 연계 증강

- 등급은 5등급부터 0등급까지이며, 가중치 표(0등급 0.5%, 1등급 4.5%, 2등급 10%, 3등급 25%, 4등급 40%, 5등급 20%)로 등급을 먼저 뽑은 뒤 그 등급 안에서 증강 하나를 균등하게 고릅니다.
- `isFollowUpAugment`로 표시한 증강은 처음에 잠금 목록에 들어가 등장하지 않습니다. 기반 증강이 적용될 때 `UnlockAugmentByType`을 호출해 같은 타입을 풀어 줍니다. 예를 들어 **FireBall**을 얻으면 `"Fire"` 타입 증강이 모두 풀려, 이후 선택지에 화염 지속시간이나 화염 폭발 증강이 등장합니다. 기반 증강이 제거되면 다시 잠깁니다.
- 이미 고른 증강은 잠그고, 한 번에 제시하는 3개는 따로 추적해 서로 겹치지 않게 했습니다.

<화염공 획득 후 연계 증강이 등장하는 장면에 대한 사진 필요>

#### 3. 웨이브 생성과 밸런싱

블록 구성과 강도는 웨이브 번호에서 수식으로 계산하고, 계수는 엑셀 시트에서 조정했습니다.

- **블록 종류 가중치**: 종류마다 20웨이브 간격으로 중심을 둔 가우시안 가중치(σ = 55)를 줘서, 블록 구성이 갑자기 바뀌지 않고 점차 어려운 블록 쪽으로 옮겨 갑니다. 종류별 해금 웨이브(10웨이브 단위)도 따로 두어, 아직 해금되지 않은 종류의 가중치는 기본 블록과 힐 블록에 나눠 줍니다.
- **개수와 체력**: 한 웨이브에 `10~15 + min(웨이브 / 10, 10)`개가 나옵니다. 일반 블록 체력은 `(웨이브 + 5·구간 + 1)^1.1`에 비례하고, 보스는 지수 1.2에 약 5배를 곱하며, 각각 약간의 무작위 편차를 둡니다.
- **보스 종류**: 웨이브 구간에 따라 정해집니다(11~20웨이브는 Hard, 21~30웨이브는 Slime 등). 보스가 그 구간에 새로 나오는 블록의 특성을 먼저 보여 주는 역할을 합니다.
- **스페셜 블록**: 5웨이브부터 블록이 하나 나올 때마다 등장 확률이 누적되고, 한 번 등장하면 0으로 초기화됩니다. 희귀하지만 너무 오래 안 나오는 일은 없게 했습니다.
- **Rapid 웨이브**: 10웨이브 이후에는 생성 간격이 훨씬 짧은 Rapid 웨이브가 나올 수 있습니다. 확률은 일반 웨이브마다 9%씩 오르고 발동하면 초기화됩니다.

#### 4. 그리드 배치와 리플렉션 기반 풀링

- 필드는 6 × 10 그리드입니다. 블록은 빈 칸 중 무작위 위치에 배치되고, 파괴되면 칸을 반납합니다. 빈 칸이 없으면 생성을 멈춥니다.
- 블록 종류는 `BrickInfo` ScriptableObject에 이름으로 나열합니다. 스포너는 `Type.GetType`으로 타입을 찾고 `MakeGenericMethod`로 풀 매니저의 제네릭 `GetFromPool<T>` / `TakeToPool<T>`를 호출하므로, 블록 종류를 추가해도 스포너 코드는 바뀌지 않습니다.

#### 5. Google Play Games 연동과 저장

- **랭킹**: 게임 오버 시 점수를 등록합니다. 랭킹 탭은 `LoadScores` / `LoadMoreScores`로 상위 100명을 25명씩 페이지 단위로 불러오고, 내 순위는 따로 조회합니다. 연결이 없으면 로컬 최고 기록을 보여 줍니다.
- **업적**: Google Play Games 업적 21개를 각각 작은 클래스로 감싸, 플레이 통계로 진행도를 보고합니다.
- **클라우드 저장**: 상점 강화, 골드, 누적 통계를 JSON으로 직렬화해 Saved Games에 저장합니다. 로그인이 안 되어 있거나 요청이 실패하면 같은 JSON을 로컬 파일에 저장합니다.
- **데일리 미션**: 미션 데이터베이스에서 3개를 뽑고, UTC 00:00(한국 시간 오전 9시)에 초기화하며 남은 시간을 UI에 표시합니다. 업적은 플레이 통계를 누적해 구간을 넘을 때마다 골드를 지급합니다.

<br>

### 게임 외 화면

하단 메뉴로 상점, 미션과 업적, 메인, 랭킹, 설정을 오갑니다.

| 화면 | 내용 |
| --- | --- |
| 상점 | 골드로 시작 스탯을 강화합니다. 공격력, 이펙트 공격력, 추가 경험치, 치명타 확률, 치명타 공격력, 시작 공 개수가 있고, 강화를 초기화하면 골드를 전부 돌려받습니다. |
| 미션과 업적 | 데일리 미션과 누적 업적. 둘 다 골드를 보상으로 줍니다. |
| 메인 | 게임을 시작합니다. 1,000 골드를 내면 무작위 증강 하나를 들고 시작할 수 있습니다. |
| 랭킹 | 글로벌 랭킹. 오프라인이면 로컬 기록을 보여 줍니다. |
| 설정 | BGM, SFX 음량 |

| 상점 | 미션과 업적 | 랭킹 |
| :---: | :---: | :---: |
| <상점 화면에 대한 사진 필요> | <미션과 업적 화면에 대한 사진 필요> | <랭킹 화면에 대한 사진 필요> |

<br>

### 트러블슈팅

| 문제 | 보정 |
| --- | --- |
| 현재 빌드로 깰 수 없는 보스를 만나면 게임이 끝없이 늘어짐 | 보스가 등장하면 60초 타이머를 시작하고, 시간이 다 되면 게임 오버 처리 |
| 증강으로 공이 곧 다시 생기는데도 마지막 공이 사라지는 순간 게임이 끝남 | 필드에 공이 없는 상태가 3초간 이어져야 게임 오버 처리 |
| 트윈 블록은 두 칸이 필요한데 빈 칸이 하나뿐이면 문제가 생김 | 빈 칸이 두 칸 미만이면 블록 종류를 다시 추첨 |
| 플레이어가 블록을 못 따라가면 웨이브가 계속 쌓임 | 블록이 5개 넘게 남아 있으면 다음 웨이브를 최대 5초까지 대기 |

<br>

### 시스템 구조

```
Assets/
├─ Augment/       증강 매니저, IAugment, Stat/Field/Special 증강 세트, 특수 공격 오브젝트
├─ Ball/          공 매니저, 공 종류(Bomb, Ghost, Glass), 영구·임시 스탯
├─ Brick/         블록 매니저, 데미지 매니저, 블록 10종, 스페셜 블록
├─ Item/          골드, 경험치 구슬, 주사위
├─ Pin/           플리퍼
├─ Shader/        Shader Graph 이펙트, 비주얼 이펙트 매니저
├─ Sound/         사운드 매니저, BGM, SFX
├─ UI/            레벨업, 업적과 미션, 리더보드, 피버 바, 데미지 텍스트
├─ UpgradeShop/   상점 강화
├─ Modules/       제네릭 오브젝트 풀 모듈
└─ Scenes/        Title, Main, Game
```

<br>

### 기술 스택 & 실행

**Unity 6 (6000.0.60f1)** · C# · URP 2D · Shader Graph · Input System · DOTween · Redcode.Pools · Google Play Games Plugin for Unity

- **플레이**: <Google Play 스토어 링크 필요>
- **조작**: 좌 / 우 버튼을 누르고 있으면 해당 플리퍼가 올라갑니다
