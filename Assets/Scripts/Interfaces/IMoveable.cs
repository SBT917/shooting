using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable
{
    /**  
    <summary>
    移動処理
    </summary>
    */
    void Move(Vector3 direction);

    /**  
    <summary>
    移動する方向
    </summary>
    */
    Vector3 Direction { get; set; }

    /**  
    <summary>
    移動速度
    </summary>
    */
    float Speed { get; set; }
}
