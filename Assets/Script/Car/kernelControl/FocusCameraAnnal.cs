using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 【录像摄像机】
/// </summary>
public class FocusCameraAnnal : MonoBehaviour
{
    public GameObject target;
    public int hight = 6;

    void Update()
    {
        Vector3 pos = this.transform.position;
        pos.x = target.transform.position.x;
        pos.z = target.transform.position.z;
        pos.y = hight + target.transform.position.y;
        this.transform.position = pos;
    }
}
