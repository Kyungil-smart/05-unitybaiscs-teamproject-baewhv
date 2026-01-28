using UnityEngine;
using UnityEngine.UI;

public class MonsterState : MonoBehaviour
{
    [SerializeField] private float _maxHp = 10f;
    [SerializeField] private float _currentHp;
    [SerializeField] private Slider _hpSlider;

    protected virtual void Awake()
    {
        _currentHp = _maxHp;
        if (_hpSlider != null)
        {
            _hpSlider.maxValue = _maxHp;
            _hpSlider.value = _currentHp;
        }

        MonsterManager.Register();
    }

    /// <summary>
    /// 몬스터에게 데미지 주는 로직
    /// </summary>
    public void TakeDamage(float damage)
    {
        _currentHp -= damage;

        if (_hpSlider != null)
        {
            _hpSlider.value = _currentHp;
        }

        if (_currentHp <= 0) Die();
    }

    private void Die()
    {
        MonsterManager.Unregister();
        Destroy(gameObject);
    }

    protected virtual void OnDestroy()
    {
        //static 데이터 때문에 한번 더 정확하게 제거
        if (gameObject.scene.isLoaded) MonsterManager.Unregister();
    }
}