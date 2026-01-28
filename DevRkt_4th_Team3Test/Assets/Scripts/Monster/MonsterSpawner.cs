using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("Spawn Settings")] [SerializeField]
    private GameObject _monsterPrefab;

    [SerializeField] private float _spawnInterval = 1.0f;
    [SerializeField] private int _maxPopulation = 50;

    [Header("Spawn Distance")] [SerializeField]
    private float _minSpawnDistance = 15.0f;

    [SerializeField] private float _maxSpawnDistance = 20.0f;

    private float _spawnTimer;
    private Transform _playerTransform;

    private void Start()
    {
        //플레이어 위치 등록
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null) _playerTransform = player.transform;
    }

    private void Update()
    {
        if (_playerTransform == null) return;

        if (MonsterManager.CanSpawn(_maxPopulation))
        {
            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0f)
            {
                SpawnMonsterAroundPlayer();
                _spawnTimer = _spawnInterval;
            }
        }
    }

    private void SpawnMonsterAroundPlayer()
    {
        // 360도 원 안에서 랜덤한 위치 지정
        Vector2 spawnDirection = UnityEngine.Random.insideUnitCircle.normalized;

        // 스폰될 랜덤한 거리 설정
        float spawnDistance = Random.Range(_minSpawnDistance, _maxSpawnDistance);

        // 현재 플레이어 위치에서
        // 위에서 구한 방향과 거리를 통해
        // 최종 위치 계산
        Vector3 spawnPos = _playerTransform.position +
                           new Vector3(spawnDirection.x * spawnDistance, 0.5f, spawnDirection.y * spawnDistance);

        Instantiate(_monsterPrefab, spawnPos, Quaternion.identity);
    }
}