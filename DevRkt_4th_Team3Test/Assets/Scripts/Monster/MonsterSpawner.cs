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
                SpawnMonsterFromCurrentGroup();
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

    private void SpawnMonsterFromCurrentGroup()
    {
        // 현재 그룹 데이터 가져오기
        MonsterGroup currentGroup = _monsterGroups[_currentGroupIndex];
        if (currentGroup.prefabs.Length == 0) return;

        // 스폰 방향 결정
        Vector2 spawnDirection = UnityEngine.Random.insideUnitCircle.normalized;
        
        //스폰 거리 결정
        float spawnDistance = Random.Range(_minSpawnDistance, _maxSpawnDistance);
        
        //최종 월드 좌표 계산
        Vector3 spawnPos = _playerTransform.position +
                           new Vector3(spawnDirection.x * spawnDistance, 0.5f, spawnDirection.y * spawnDistance);
        
        // 현재 그룹의 프리팹 중에서 랜덤 스폰
        int randomNum = Random.Range(0, currentGroup.prefabs.Length);
        GameObject obj = Instantiate(currentGroup.prefabs[randomNum], spawnPos, Quaternion.identity);
        
        // 생성된 몬스터에게 그룹의 EXP 타입 추가
        MonsterState state = obj.GetComponent<MonsterState>();
        if (state != null)
        {
            state.SetExpType(currentGroup.expType);
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