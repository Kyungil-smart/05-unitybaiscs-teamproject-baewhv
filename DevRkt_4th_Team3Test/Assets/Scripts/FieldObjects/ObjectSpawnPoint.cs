using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnPoint : MonoBehaviour
{
    [SerializeField] private FieldObjectSpawnType _type;
    [SerializeField] private FieldObject _obj;
    private FieldObject _placedObject;

    private void OnDrawGizmos()
    {
        switch (_type)
        {
            case FieldObjectSpawnType.Breakable:
                Gizmos.color = Color.yellow;
                break;
            case FieldObjectSpawnType.Item:
                Gizmos.color = Color.blue;
                break;
            case FieldObjectSpawnType.Obstacle:
                Gizmos.color = Color.red;
                break;
        }
        
        Gizmos.DrawWireSphere(transform.position, 1.0f);
    }

    public void SpawnObject()
    {
        //if (_placedObject && !_obj) return;
        if (_placedObject || !_obj)
        {
            Debug.Log("스폰 오브젝트가 없습니다.");
            return;
        }
        _placedObject = FieldObjectManager.Instance.SetObject(_obj, transform.position);
        switch (_type)
        {
            case FieldObjectSpawnType.Breakable:
                Debug.Log("파괴 오브젝트 생성");
                break;
            case FieldObjectSpawnType.Item:
                Debug.Log("아이템 오브젝트 생성");
                break;
            case FieldObjectSpawnType.Obstacle:
                Debug.Log("장애물 오브젝트 생성");
                break;
        }
    }

    public void RemoveObject()
    {
        if (!_placedObject) return;
        FieldObjectManager.Instance.SetObject(_obj, transform.position);
    }
}