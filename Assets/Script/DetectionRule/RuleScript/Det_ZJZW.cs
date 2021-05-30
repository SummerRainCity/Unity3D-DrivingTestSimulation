using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//直角转弯实时监测脚本
public class Det_ZJZW : MonoBehaviour
{
    [Tooltip("中途停车扣分时间间隔")]
    private float MidwayParkingTime = 1f;//中途停车检测间隔
    private float mpTime = 1f;//累计计时，如果到达1s则判定中途停车
    public bool _isStop = true;//中途停车判定
    public bool detIsStop = true;//车辆在运行时停止一次仅扣一次中途停车的分
    [Tooltip("考试倒计时")]
    private float timer = 30f;
    public Text textTimer;

    private Transform targetPlayer;
    private void Start()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        //判断汽车是否完全停止
        if (Car_Information.Speed == 0)
        {
            mpTime += Time.deltaTime;
            if (mpTime >= MidwayParkingTime)
            {
                _isStop = true;//汽车已停止
                mpTime = 0;
            }
        }
        else
        {
            mpTime = 0;
            _isStop = false;//汽车已运行
            detIsStop = true;//初步：可以判定中途停车检测
        }
        //中途停车判定（如果车辆是停止的且不在停车范围内）
        if (_isStop && detIsStop)
        {
            print("直角转弯中途不得停车！");
            targetPlayer.GetComponent<VoicePromptAudio>().Play_ZhongTuTingChe();
            Car_Information.DeductScore(100);
            detIsStop = false;
        }

        //项目倒计时
        if (timer <= 0)
        {
            //考试超时
            targetPlayer.GetComponent<VoicePromptAudio>().Play_KaoShiChaoShi();
            Car_Information.DeductScore(100);
            timer = 0f;
            Destroy(gameObject);
        }
        else
        {
            timer -= Time.deltaTime;
            textTimer.text = "时间：" + Mathf.Round(timer) + "s";
        }
    }

    private void OnDestroy()
    {
        textTimer.text = "时间：--";
    }
}
