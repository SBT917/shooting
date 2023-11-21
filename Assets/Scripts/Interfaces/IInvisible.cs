using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInvisible
{
    void StartInvisible();

    void UpdateInvisible();

    void EndInvisible();

    bool IsInvisible { get; set; }
    
}
