using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FieldObject : MonoBehaviour
{
    [SerializeField]private SpriteRenderer _body;
    private void Awake()
    {
        if(_body)
        {
            _body.sortingOrder = 1;
        }
    }
}


public enum FieldObjectSpawnType
{
    Item,
    Breakable,
    Obstacle,
}