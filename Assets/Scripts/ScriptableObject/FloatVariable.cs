using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObject/FloatVariable")]
public class FloatVariable : ScriptableObject, ISerializationCallbackReceiver
{
    public float initValue;

    [HideInInspector] public float runTimeValue;
    public void OnAfterDeserialize()
    {
        runTimeValue = initValue;
    }

    public void OnBeforeSerialize(){}
}
