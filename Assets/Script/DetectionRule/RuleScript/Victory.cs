using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 此脚本只在曲线行驶后起作用。
/// 控制逻辑：刚开始游戏“后空气墙”启动，“前空气墙”关闭。
/// 曲线行驶结束时，“前空气墙”启动，“后空气墙”关闭以方便车辆进入停车区域。
/// </summary>
public class Victory : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //告诉游戏管理类，游戏已经结束
            FindObjectOfType<MyGameManager>().GamePlayingFlag = false;
        }
    }
}
