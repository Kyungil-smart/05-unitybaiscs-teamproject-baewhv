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
        if (_placedObject || !_obj) return;

        switch (_type)
        {
            case FieldObjectSpawnType.Item:
                _placedObject = FieldObjectManager.Instance.SetDropObject(transform.position);
                break;
            case FieldObjectSpawnType.Breakable:
            case FieldObjectSpawnType.Obstacle:
                _placedObject = Instantiate(_obj, transform);
                break;
        }
        _placedObject.ParentPoint = this;

    }

    public void RemoveObject()
    {
        if (!_placedObject) return;
        switch (_type)
        {
            case FieldObjectSpawnType.Item:
                FieldObjectManager.Instance.RemoveDrobObject(_obj);
                break;
            case FieldObjectSpawnType.Breakable:
                Destroy(_placedObject.gameObject);
                _placedObject = null;
                break;
            case FieldObjectSpawnType.Obstacle:
                break;
        }
    }
}