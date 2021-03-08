using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一个项目结束后，销毁当前项目所有检测信息
/// </summary>
public class End : MonoBehaviour
{
    public GameObject Points;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Destroy(Points);
            Destroy(gameObject);
        }
    }
}
