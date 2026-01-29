using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonsterState : MonoBehaviour
{
    [Header("HP Settings")]
    [SerializeField] private float _maxHp = 10f;
    [SerializeField] private float _currentHp;
    [SerializeField] private Slider _hpSlider;
    
    [Header("Attack Settings")]
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _attackCooltime = 0.5f;
    [SerializeField] private GameObject _damageText;
    private float _lastAttackTime;
    
    public static System.Action OnMonsterDie;
    
    protected virtual void Awake()
    {
        _currentHp = _maxHp;
        if (_hpSlider != null)
        {
            _hpSlider.minValue = 0f;
            _hpSlider.maxValue = _maxHp;
            _hpSlider.value = _currentHp;
        }

        MonsterManager.Register();
    }
    
    //TODO: 테스트용, 나중에 삭제
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            TakeDamage(3);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        // 플레이어에 부딪히면 공격
        if (other.CompareTag("Player"))
        {
            Attack(other.gameObject);
        }
    }

    /// <summary>
    /// 몬스터에게 데미지 주는 로직
    /// </summary>
    public void TakeDamage(float damage)
    {
        _currentHp -= damage;
        if (_hpSlider != null) _hpSlider.value = _currentHp;
        
        //데미지 텍스트 표시
        Canvas myCanvas = GetComponentInChildren<Canvas>();
        if (myCanvas != null)
        {
            MonsterDamageText.ShowDamageText(_damageText, myCanvas.transform, Mathf.RoundToInt(damage));
        }
        
        if (_currentHp <= 0) Die();
    }

    public void Attack(GameObject target)
    {
        //쿨타임
        if (Time.time - _lastAttackTime < _attackCooltime) return;
        PlayerStats stats = target.GetComponent<PlayerStats>();
        if (stats != null)
        {
            stats.TakeDamage(_damage);
            _lastAttackTime = Time.time;
        }else
        {
            Debug.LogWarning("ERROR: 플레이어에게 PlayerStats 컴포넌트가 없습니다!");
        }
    }

    private void Die()
    {
        OnMonsterDie?.Invoke();
        MonsterManager.Unregister();
        Destroy(gameObject);
    }

    protected virtual void OnDestroy()
    {
        //static 데이터 때문에 한번 더 정확하게 제거
        if (gameObject.scene.isLoaded) MonsterManager.Unregister();
    }
}