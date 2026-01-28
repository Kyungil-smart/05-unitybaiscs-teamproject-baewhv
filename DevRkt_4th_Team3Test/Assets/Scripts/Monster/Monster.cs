using UnityEngine;

namespace Game
{
    public class Monster : MonoBehaviour
    {
        // --- 멤버 변수 ---

        [Header("Movement Settings")]
        [SerializeField] private float _moveSpeed = 3.0f;

        [Header("Spawn Settings")]
        [SerializeField] private float _spawnInterval = 5.0f;
        [SerializeField] private int _maxPopulation = 30; // 최대 개체수 제한

        // 전체 몬스터 개체수 공유 (Static)
        private static int _currentMonsterCount = 0;

        private float _spawnTimer;
        private Transform _target;
        private SpriteRenderer _spriteRenderer;

        // --- 유니티 이벤트 ---

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _spawnTimer = _spawnInterval;
            
            // 생성 시 카운트 증가
            _currentMonsterCount++;
        }

        private void Start()
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                _target = playerObj.transform;
            }
        }

        private void Update()
        {
            FollowTarget();
            CheckFlip();
            HandleSelfSpawn();
        }

        private void OnDestroy()
        {
            // 파괴 시 카운트 감소
            _currentMonsterCount--;
            if (_currentMonsterCount < 0) _currentMonsterCount = 0;
        }

        // 화면 밖 렌더링 비활성화 (최적화)
        private void OnBecameInvisible()
        {
            if (_spriteRenderer != null) _spriteRenderer.enabled = false;
        }

        // 화면 진입 시 렌더링 활성화
        private void OnBecameVisible()
        {
            if (_spriteRenderer != null) _spriteRenderer.enabled = true;
        }

        // --- 내부 함수 ---

        private void FollowTarget()
        {
            if (_target == null) return;

            // Y축 고정, X/Z축 추적
            Vector3 targetPos = new Vector3(_target.position.x, transform.position.y, _target.position.z);

            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPos,
                _moveSpeed * Time.deltaTime
            );
        }

        private void CheckFlip()
        {
            if (_target == null || _spriteRenderer == null) return;
            _spriteRenderer.flipX = _target.position.x < transform.position.x;
        }

        private void HandleSelfSpawn()
        {
            if (_spawnInterval <= 0f) return;
            if (_currentMonsterCount >= _maxPopulation) return; // 제한 초과 시 중단

            _spawnTimer -= Time.deltaTime;

            if (_spawnTimer <= 0f)
            {
                SpawnClone();
                _spawnTimer = _spawnInterval;
            }
        }

        private void SpawnClone()
        {
            if (_currentMonsterCount >= _maxPopulation) return;

            // X/Z 평면 랜덤 위치 계산
            Vector2 randomCircle = Random.insideUnitCircle * 1.5f;
            Vector3 randomOffset = new Vector3(randomCircle.x, 0f, randomCircle.y);
            Vector3 spawnPosition = transform.position + randomOffset;

            // 자가 복제
            GameObject clone = Instantiate(gameObject, spawnPosition,transform.rotation);
            clone.name = gameObject.name.Replace("(Clone)", "");
        }
    }
}