using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Absolver : ItemObject
{
    public override void Interact(PlayerStats pc)
    {
        FieldObjectManager.Instance.AbsolvingAllExpObject();
        base.Interact(pc);
    }
}
