using UnityEngine;

public class MonsterState: MonoBehaviour
{
        [SerializeField] private int _hp = 10;
        public bool IsDead { get; private set; } = false;
        
        public void TakeDamage(int damage)
        {
                if (IsDead) return;
                _hp -= damage;
                if (_hp <= 0) Die();
        }
        
        private void Die()
        {
                IsDead = true;
                MonsterManager.Unregister();
                Destroy(gameObject);
        }
        
        private void OnDestroy()
        {
                if (!IsDead) MonsterManager.Unregister();
        }
}
