using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 【车身压线检测】
 * 扣100分
 */
public class Wall : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        //如果玩家的汽车碰到墙体
        if(collision.gameObject.tag == "Player")
        {
            Car_Information.DeductScore(100);
            Destroy(gameObject);
        }
    }
}
