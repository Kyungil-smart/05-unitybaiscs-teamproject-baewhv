using UnityEngine;

namespace Game
{
    public static class MonsterPopulationManager
    {
        private static int _count = 0;
        public static int Count => _count;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        public static void ResetCount() => _count = 0;

        public static void Register() => _count++;
        public static void Unregister() => _count = Mathf.Max(0, _count - 1);
        public static bool CanSpawn(int maxPopulation) => _count < maxPopulation;
    }

    [RequireComponent(typeof(Rigidbody))]
    public class Monster : MonoBehaviour
    {
        protected const string PLAYER_TAG = "Player";

        [Header("Movement")]
        [SerializeField] protected float _moveSpeed = 3.0f;

        [Header("Spawn")]
        [SerializeField] protected float _spawnInterval = 2.0f;
        [SerializeField] protected int _maxPopulation = 30;
        [SerializeField] protected float _spawnRadius = 2.0f;

        protected float _spawnTimer;
        protected Transform _target;
        protected SpriteRenderer _spriteRenderer;
        protected Rigidbody _rigidbody;
        protected bool _hasTarget;

        protected virtual void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody = GetComponent<Rigidbody>();
            InitializeSpawnTimer();
            MonsterPopulationManager.Register();
        }

        protected virtual void Start()
        {
            FindTarget();
        }

        protected virtual void FixedUpdate()
        {
            if (!_hasTarget || _target == null)
            {
                _rigidbody.velocity = Vector3.zero;
                return;
            }
            MoveTowardsTarget();
        }

        protected virtual void Update()
        {
            if (_target != null) UpdateSpriteFlip();
            UpdateSpawnTimer();
        }

        protected virtual void OnDestroy()
        {
            MonsterPopulationManager.Unregister();
        }

        protected virtual void OnBecameInvisible() => SetSpriteVisibility(false);
        protected virtual void OnBecameVisible() => SetSpriteVisibility(true);

        private void InitializeSpawnTimer() => _spawnTimer = _spawnInterval;

        protected virtual void FindTarget()
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag(PLAYER_TAG);
            if (playerObj != null)
            {
                _target = playerObj.transform;
                _hasTarget = true;
            }
        }

        // Velocity 기반 이동 (물리 충돌 처리)
         protected virtual void MoveTowardsTarget()
        {
            Vector3 direction = (_target.position - transform.position).normalized;
            _rigidbody.AddForce(direction * _moveSpeed * 10f); 
        }
        

        protected virtual void UpdateSpriteFlip()
        {
            if (_spriteRenderer == null) return;
            _spriteRenderer.flipX = _target.position.x < transform.position.x;
        }

        private void SetSpriteVisibility(bool visible)
        {
            if (_spriteRenderer != null) _spriteRenderer.enabled = visible;
        }

        protected virtual void UpdateSpawnTimer()
        {
            if (!CanSpawn()) return;

            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer <= 0f)
            {
                SpawnClone();
                _spawnTimer = _spawnInterval;
            }
        }

        protected virtual bool CanSpawn()
        {
            return _spawnInterval > 0f && MonsterPopulationManager.CanSpawn(_maxPopulation);
        }

        protected virtual void SpawnClone()
        {
            if (!MonsterPopulationManager.CanSpawn(_maxPopulation)) return;

            Vector3 spawnPos = CalculateSpawnPosition();
            GameObject clone = Instantiate(gameObject, spawnPos, transform.rotation);
            
            // (Clone) 접미사 제거
            int cloneIndex = clone.name.IndexOf("(");
            if (cloneIndex > 0)
                clone.name = clone.name.Substring(0, cloneIndex);
        }

        protected virtual Vector3 CalculateSpawnPosition()
        {
            Vector2 randomOffset = Random.insideUnitCircle * _spawnRadius;
            return transform.position + new Vector3(randomOffset.x, 0f, randomOffset.y);
        }
    }
}