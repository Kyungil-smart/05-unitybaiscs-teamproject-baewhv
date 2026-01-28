using UnityEngine;

public class MonsterManager
{
    private static int _monsterCount = 0;
    public static int MonsterCount => _monsterCount;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    public static void ResetCount() => _monsterCount = 0;

    /// <summary>
    /// 스폰 추가
    /// </summary>
    public static void Register() => _monsterCount++;

    /// <summary>
    /// 스폰 취소
    /// </summary>
    public static void Unregister() => _monsterCount = Mathf.Max(0, _monsterCount - 1);

    /// <summary>
    /// 스폰 가능한 상태 확인
    /// </summary>
    public static bool CanSpawn(int maxPopulation) => _monsterCount < maxPopulation;
}