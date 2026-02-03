using UnityEngine;
using TMPro;

public class MonsterSpawner : MonoBehaviour
{

    [Header("Spawn Settings")]
    [SerializeField] private MonsterGroup[] _monsterGroups;
    private int _currentGroupIndex = 0;
    [SerializeField] private int _maxMonsterCount = 50;
    
    [Header("Spawn Time")]
    [SerializeField] private float _baseSpawnTime = 2.0f;
    [SerializeField] private float _minSpawnTime = 0.2f;
    [SerializeField] private float _difficultyLevelTime = 30f; //어려워지는 시간 간격
    [SerializeField] private float _timeDecreasePer = 0.5f; //줄어드는 스폰 시간 값
    [SerializeField] private int _increaseMonsterCount = 10;
    
    [Header("Spawn Distance")]
    [SerializeField] private float _minSpawnDistance = 15.0f;
    [SerializeField] private float _maxSpawnDistance = 20.0f;
    [SerializeField] private TextMeshProUGUI _monsterCountText;
    
    private int _baseMonsterCount;
    private float _spawnTimer;
    private float _elapsedTime;
    private Transform _playerTransform;

    private void Start()
    {
        //플레이어 위치 등록
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) _playerTransform = player.transform;
        
        _baseMonsterCount = _maxMonsterCount;
        _spawnTimer = 0f;
    }

    private void Update()
    {
        if (_playerTransform == null) return;

        _elapsedTime += Time.deltaTime;

        // 시간에 맞춰서 그룹 업데이트 체크
        UpDifficultyLevel();
        UpdateCurrentGroup();
        
        // 난이도가 반영된 간격 계산
        float currentInterval = GetSpawnTime();

        // 현재 선택된 그룹 스폰
        if (MonsterManager.CanSpawn(_maxMonsterCount))
        {
            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0f)
            {
                SpawnMonsterGroup();
                // 현재 스폰 시간 간격 적용
                _spawnTimer = currentInterval;
            }
        }
        ShowMonsterCount();
    }
    
    private void UpDifficultyLevel()
    {
        // 30초가 몇 번 지났는지 계산
        // 0, 30, 60... < 이렇게
        int step = Mathf.FloorToInt(_elapsedTime / _difficultyLevelTime);

        // 30초마다 _increaseMonsterCount 만큼 최대 마릿수 증가
        _maxMonsterCount = _baseMonsterCount + (step * _increaseMonsterCount);
    }
    
    private float GetSpawnTime()
    {
        // 30초마다 _timeDecreasePer 만큼
        // 스폰 간격이 빨라짐 (감소됨)
        int step = Mathf.FloorToInt(_elapsedTime / _difficultyLevelTime);
        float calculatedTime = _baseSpawnTime - (step * _timeDecreasePer);

        // 계산된 시간이 최소 값보다
        // 작아지지 않도록 고정
        return Mathf.Max(calculatedTime, _minSpawnTime);
    }
    

    private void UpdateCurrentGroup()
    {
        // 다음 그룹이 있고
        // 현재 시간이 다음 그룹의 시작 시간보다 크다면
        // 그룹 인덱스 증가
        if (_currentGroupIndex + 1 < _monsterGroups.Length)
        {
            if (_elapsedTime >= _monsterGroups[_currentGroupIndex + 1].startTime)
            {
                _currentGroupIndex++;
                Debug.Log($"몬스터 그룹 변경! 현재 그룹: {_monsterGroups[_currentGroupIndex].groupName}");
            }
        }
    }

    private void SpawnMonsterGroup()
    {
        // 플레이 시간에 맞춰
        // 몬스터 그룹 데이터를 가져옴
        MonsterGroup currentGroup = _monsterGroups[_currentGroupIndex];
        if (currentGroup.prefabs.Length == 0) return;

        // 플레이 시간에 맞춰
        // 한 번에 스폰할 마릿수를 결정
        int spawnCount = SpawnCount();

        // 결정된 마릿수만큼
        // 반복하여 몬스터 스폰
        for (int i = 0; i < spawnCount; i++)
        {
            // 현재 맵에 깔린 총 마릿수가
            // 최대치를 넘었는지 실시간 체크
            if (!MonsterManager.CanSpawn(_maxMonsterCount)) break;

            // 플레이어 기준 거리 범위 랜덤 좌표 계산
            Vector3 spawnPos = GetRandomSpawnPosition();

            // 실제 게임 오브젝트를 생성
            CreateMonster(currentGroup, spawnPos);
        }
    }
    
    // 한 번에 스폰되는 몬스터 수 계산
    private int SpawnCount()
    {
        // elapsedTime: 전체 플레이 시간
        // difficultyLevelTime: 난이도 상승 주기
        // 두개 나눠서 현재 난이도 단계를 구함
        int step = Mathf.FloorToInt(_elapsedTime / _difficultyLevelTime);
        
        //테스트 해가면서 가중치 수치 조절
        return 1 + (step * 2);
    }
    
    /// 랜덤 스폰 위치 구하기
    private Vector3 GetRandomSpawnPosition()
    {
        // Random.insideUnitCircle: 반경 1 이내의 랜덤 좌표를 반환
        // .normalized로 원의 테두리 방향 구하기
        Vector2 spawnDirection = UnityEngine.Random.insideUnitCircle.normalized;
    
        // 최소 거리와 최대 거리 사이
        // 랜덤한 거리값 생성
        float spawnDistance = Random.Range(_minSpawnDistance, _maxSpawnDistance);
    
        // 방향과 거리를 플레이어 위치에 더함
        // Y: 0.5f
        return _playerTransform.position +
               new Vector3(spawnDirection.x * spawnDistance, 0.5f, spawnDirection.y * spawnDistance);
    }
    
    // 몬스터 프리팹 생성
    private void CreateMonster(MonsterGroup group, Vector3 position)
    {
        // 랜덤으로 그룹에 있는 몬스터 1개 선택
        int randomNum = Random.Range(0, group.prefabs.Length);
    
        // 선택된 몬스터를 계산된 위치에 생성
        GameObject obj = Instantiate(group.prefabs[randomNum], position, Quaternion.identity);
    
        // 생성된 몬스터의 MonsterState 세팅
        MonsterState state = obj.GetComponent<MonsterState>();
        if (state != null)
        {
            state.SetExpType(group.expType);
        }
    }

    private void ShowMonsterCount()
    {
        if (_monsterCountText != null)
        {
            _monsterCountText.text = $"몬스터 수: {MonsterManager.MonsterCount} / {_maxMonsterCount}";
        }
    }
}