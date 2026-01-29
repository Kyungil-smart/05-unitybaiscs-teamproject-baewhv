using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileData : MonoBehaviour
{
    [SerializeField] private List<ObjectSpawnPoint> _spawnPoints;
    [SerializeField] private GameObject _tileGroup;
    [SerializeField] private bool _showTiles = false;

    private void OnValidate()
    {
        if (!_tileGroup) return;

        foreach (Transform child in _tileGroup.transform)
        {
            child.gameObject.hideFlags = _showTiles ? HideFlags.None : HideFlags.HideInHierarchy;
        }
    }

    /// <summary>
    /// 스폰포인트에 설정된 오브젝트 생성 활성화
    /// </summary>
    public void EnableSpawn()
    {
        foreach (ObjectSpawnPoint sp in _spawnPoints)
        {
            sp.SpawnObject();
        }
    }

    public void DisableSpawn()
    {
        foreach (ObjectSpawnPoint sp in _spawnPoints)
        {
            sp.RemoveObject();
        }
    }
}