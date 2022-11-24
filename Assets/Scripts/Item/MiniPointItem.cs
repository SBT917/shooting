using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniPointItem : Item
{
    protected override void Awake()
    {
        point = 1;
        base.Awake();
    }
}
