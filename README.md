# 그라운드 룰
- 아침 회의 (조회 이후, 10시 20분)
  - 진행사항
  - 오늘의 목표
- 중간 회의 (2시 10분)
  - 오늘의 진척도
  - 이슈 체크
- 빌드 (18시 ~ 19시?)

- 타 작업자와의 협업이 필요할 경우 몇시부터 작업할건지 상의 후 디코에 등록.
- 본인의 작업 완료 후 디코에 간단하게 공유.
  - 이슈, 이후 협업해야할 사항 등
- 디코에 올라온 것 중 중요한 사항은 V 이모지 남기기.
  - 특히 특정 작업물에 대해서 건드리지 말아야 할 때.
- 배분된 역할의 최소 기능은 본인이 최대한 구현한다.

# 깃 룰
- 각자 작업명으로 브랜치 제작하여 해당 브랜치에서 작업.
- 1일 1Pull Request.
  - Pull Request 시 다른 곳에서도 문제가 없도록 마무리 하여 커밋할 것.
- Scene의 메인 배치 작업자는 1명만.
  - 씬에서 테스트가 필요한 경우 다음 절차를 진행한다.
  - 제작된 TestScene
  - .gitignore를 이용해 별도로 분리된 씬에서 작업할것.

### 커밋 메세지
- Summary [*** : 내용]
  - feat : 새로운 기능
  - fix : 버그 수정
  - docs : 문서 수정 (ReadMe 등)
  - refactor : 버그를 수정하거나 기능을 추가하지 않는 코드
  - add : 리소스 추가
  - merge : 병합
  
- Description
  - 추가 설명이 필요할 경우에만 작성.

### Branch
- main : 최종 완성
  - dev : 머지 관리
    - feat/characterMove : 작업단위 브랜치
    - feat/blahblah


# 코드 컨벤션

### 주석
- /// <- Summary 주석으로 간단하게 함수 설명 작성

### 표기법
- 멤버변수 : _camelCase
- 프로퍼티 : PascalCase
- 함수명 : PascalCase
- 매개변수 : camelCase

### 코드 순서
  - 멤버 변수
  - 유니티 이벤트
    - 이벤트 생명주기 순서에 맞게 해주면 더 좋을거같습니다.
  - 내부 함수
  - 외부 함수

```csharp

public class Test : Monobehaviour, IInterface
{
    ///멤버 변수
    [SerializeField] private int _maxHealth;
    public void Properties { get; set; }

    //유니티 이벤트
    private void Update() 
    {

    }

    /// <summary>
    /// 어떤 함수인지 설명
    /// </summary>
    /// <param name="value">매개변수 설명</param>
    /// <returns>반환값 설명</returns>
    public int Function(int value)
    {
        
    }

    /// private 메서드
    private void PrivateFunction(int value)
    {
        
    }
}
```