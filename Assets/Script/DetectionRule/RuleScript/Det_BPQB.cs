using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//半坡起步实时监测脚本
public class Det_BPQB : MonoBehaviour
{
    [Header("前后保险杠检测")]
    public float Ab = 0;
    private const float Ab_Max = 2.9f;
    private const float Ab_Min = 1.7f;

    [Header("边距30公分")]
    public float Bc = 0;
    private const float Bc_Min = 2.4f;
    private const float Bc_Max = 2.7f;

    [Header("停车点")]
    public Transform point_carbarn;

    [Header("空气墙")]
    public GameObject AirWall;

    [Tooltip("考试倒计时")]
    private float timer = 30f;
    public Text textTimer;

    public bool detIsStop = true;//车辆在运行时停止一次仅扣一次中途停车的分
    private Transform targetPlayer;

    private void Start()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }

    enum Step
    {
        Default, Carbarn
    }
    Step step = Step.Carbarn;
    private void Update()
    {
        //X轴度数基准向量
        Vector3 carbarnWord_a = point_carbarn.TransformPoint(Vector3.forward);
        carbarnWord_a = carbarnWord_a - point_carbarn.position;
        //库点指向车辆
        Vector3 carbarnWord_b = targetPlayer.position - point_carbarn.position;
        //度数
        float Angle = Mathf.Acos(Vector3.Dot(carbarnWord_a.normalized, carbarnWord_b.normalized));
        //车与库点距离
        float JuLi = Vector3.Distance(targetPlayer.position, point_carbarn.position);
        //Ab用于判断车辆是否在停车位内
        Ab = Mathf.Sin(Angle) * JuLi;
        Bc = Mathf.Cos(Angle) * JuLi;

        Debug.DrawLine(targetPlayer.position, point_carbarn.position, Color.red);

        switch (step)
        {
            case Step.Carbarn:
                if (Car_Information.isStop
                    && Ab <= Ab_Max && Ab >= Ab_Min
                    && Bc <= Bc_Max && Bc >= Bc_Min)
                {
                    targetPlayer.GetComponent<VoicePromptAudio>().Play_TingCheYiDaoWei();
                    step = Step.Default;
                }else if(Ab <= Ab_Max && Ab >= Ab_Min && Bc <= Bc_Max && Bc >= Bc_Min)
                {
                    detIsStop = false;
                }
                break;
            case Step.Default:
                Destroy(AirWall);
                break;
        }

        if(Car_Information.Speed == 0 && detIsStop)
        {
            print("【半坡起步】中途停车，扣100分");
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
