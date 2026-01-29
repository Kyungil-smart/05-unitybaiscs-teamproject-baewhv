using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : FieldObject, IInteractable
{
    public virtual void Interact(PlayerStats pc)
    {
        FieldObjectManager.Instance.RemoveObject(this);
    }
}
