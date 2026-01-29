using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BreakableObject : FieldObject, IDamagable
{
    [SerializeField][Range(0,5)] private int _heath = 1;

    public void TakeDamage(int amount)
    {
        _heath--;
        if (_heath <= 0)
        {
            FieldObjectManager.Instance.MakeExpObject(EXPType.small, transform.position);
            FieldObjectManager.Instance.RemoveObject(this);
        }
    }

    public void Heal(int amount)
    {
    }
}
