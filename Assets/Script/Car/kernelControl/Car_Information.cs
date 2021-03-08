using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// 【玩家的信息-玩家就是车】
/// 1、汽车是否停止
/// 2、得分
/// </summary>
public class Car_Information : MonoBehaviour
{
    [Header("分数UI-Text组件")]
    public Text textScore;
    [Header("考试分数-初始100分")]
    private static int Score;
    [Header("汽车是否停止超过2秒判断")]
    public float time = 0f;
    public static bool isStop = true;//汽车是否停止
    public float IntervalSecond = 2f;//停止2秒后才判断停止
    private DrivingKernel ObjSpeed;//仅用于速度检测
    public static float Speed = 0;//汽车速度
    public static float SpeedOrigin = 0;//汽车源速度

    private void Start()
    {
        Score = 100;
        ObjSpeed = FindObjectOfType<DrivingKernel>();
    }

    /// <summary>
    /// 扣除指定分数，返回现在的分数
    /// </summary>
    /// <param name="dscore">扣除分数</param>
    public static void DeductScore(int dscore)
    {
        Score -= dscore;
    }

    public static int GetScore()
    {
        return Score;
    }

    private void Update()
    {
        //分数实时更新
        if (textScore != null)
        {
            textScore.text = "分数：" + Score;
        }

        Speed = ObjSpeed.speed;
        SpeedOrigin = ObjSpeed.speedOrigin;
        //判断汽车是否完全停止
        if (ObjSpeed.speed == 0)
        {
            time += Time.deltaTime;
            if (time >= IntervalSecond)
            {
                isStop = true;
                time = 0;
            }
        }
        else
        {
            time = 0;
            isStop = false;
        }
    }
}
