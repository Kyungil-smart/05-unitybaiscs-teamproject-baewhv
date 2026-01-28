
    using UnityEngine;

    public class MonsterMove : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 3.0f;
        private Transform _target;
        private Rigidbody _rigidbody;
        private SpriteRenderer _spriteRenderer;
        
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _rigidbody.freezeRotation = true;
        }
        
        private void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) _target = player.transform;
        }
        
        private void FixedUpdate()
        {
            if (_target == null) return;
                
            Vector3 direction = (_target.position - transform.position);
            direction.y = 0;
            direction = direction.normalized;
            
            _rigidbody.velocity = direction * _moveSpeed;
        }
        
        protected virtual void Update()
        {
            if (_target == null || _spriteRenderer == null) return;

            // 캐릭터 바라보기
            _spriteRenderer.flipX = _target.position.x < transform.position.x;

            // 정면으로 표시
            if (Camera.main != null)
            {
                transform.rotation = Camera.main.transform.rotation;
            }
        }
    }
