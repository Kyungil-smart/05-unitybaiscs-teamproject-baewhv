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
            FieldObjectManager.Instance.SetDropObject(transform.position);
            if(ParentPoint)
                ParentPoint.RemoveObject();
        }
        else
        {
            _body.material.SetFloat(Shader.PropertyToID("_FlashAmount"), 1f);
            StartCoroutine(SetDefaultColor());
        }
    }

    private IEnumerator SetDefaultColor()
    {
        yield return YieldContainer.WaitForSeconds(0.1f);
        _body.material.SetFloat(Shader.PropertyToID("_FlashAmount"), 0f);
    }

    public void Heal(int amount)
    {
    }
}
