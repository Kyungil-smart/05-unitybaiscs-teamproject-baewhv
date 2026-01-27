using System.Collections;
using UnityEngine;

namespace Game
{
    public class MonsterSpawner : MonoBehaviour
    {
        [Header("Prefab")]
        [SerializeField] private GameObject monsterPrefab;

        [Header("Spawn Settings")]
        [SerializeField] private float spawnInterval = 2.0f;
        [SerializeField] private float spawnRangeX = 8.0f;
        [SerializeField] private float spawnRangeZ = 4.0f;

        [Header("Lifetime")]
        [SerializeField] private float monsterLifetime = 1.5f;

        private Coroutine _spawnRoutine;

        private void OnEnable()
        {
            if (_spawnRoutine == null)
                _spawnRoutine = StartCoroutine(SpawnMonsters());
        }

        private void OnDisable()
        {
            if (_spawnRoutine != null)
            {
                StopCoroutine(_spawnRoutine);
                _spawnRoutine = null;
            }
        }

        private IEnumerator SpawnMonsters()
        {
            // 프리팹이 없으면 스폰하지 않도록 안전장치
            while (true)
            {
                if (monsterPrefab != null)
                    SpawnMonster();

                yield return new WaitForSeconds(spawnInterval);
            }
        }

        private void SpawnMonster()
        {
            Vector3 spawnPosition = new Vector3(
                Random.Range(-spawnRangeX, spawnRangeX),
                0f,
                Random.Range(-spawnRangeZ, spawnRangeZ)
            );

            GameObject monster = Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            Destroy(monster, monsterLifetime); // 코루틴 대신 Destroy 타이머 사용
        }
    }
}
