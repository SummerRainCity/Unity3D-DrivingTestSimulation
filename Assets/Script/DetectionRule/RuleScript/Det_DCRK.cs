using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

//倒车入库实时监测脚本
public class Det_DCRK : MonoBehaviour
{
    public float JuLi = 0;
    public float Ab = 0;

    public int ParkingCount = 0;
    [Header("停车点-LEFT点")]
    public Transform point_left;
    public float difference_left = 2f;//差值-停车范围
    [Header("停车点-RIGHT点")]
    public Transform point_right;
    public float difference_right = 2f;//差值-停车范围
    [Header("停车点-车库点")]
    public Transform point_carbarn;
    public float difference_carbarn = 3.2f;//差值-停车范围
    [Tooltip("中途停车扣分时间间隔")]
    private float MidwayParkingTime = 1f;//中途停车检测间隔
    private float mpTime = 0f;//累计计时，如果到达2s则判定中途停车
    public bool _isStop = true;//中途停车判定
    public bool detIsStop = true;//车辆在运行时停止一次仅扣一次中途停车的分
    [Tooltip("考试倒计时")]
    private float timer = 210f;
    public Text textTimer;
    [Header("空气墙")]
    public GameObject AirWall;

    private Transform targetPlayer;

    private void Start()
    {
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }

    enum Step
    {
        Default,Right, Left, Carbarn
    }
    Step step = Step.Right;

    private void Update()
    {
        //Debug.DrawLine(targetPlayer.position, point_carbarn.position, Color.red);
        //JuLi = Vector3.Distance(targetPlayer.position, point_carbarn.position);
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

        switch (step)
        {
            case Step.Left:
                Debug.DrawLine(targetPlayer.position, point_left.position, Color.red);
                JuLi = Vector3.Distance(targetPlayer.position, point_left.position);
                if (Car_Information.isStop && JuLi <= difference_left)
                {
                    targetPlayer.GetComponent<VoicePromptAudio>().Play_KaiShiDaoChe();
                    step = Step.Carbarn;
                }
                else if (JuLi <= difference_left)//在停车范围内，不能判定为中途停车
                {
                    detIsStop = false;
                }
                break;
            case Step.Right:
                Debug.DrawLine(targetPlayer.position, point_right.position, Color.red);
                JuLi = Vector3.Distance(targetPlayer.position, point_right.position);
                if (Car_Information.isStop && JuLi <= difference_right)
                {
                    targetPlayer.GetComponent<VoicePromptAudio>().Play_KaiShiDaoChe();
                    step = Step.Carbarn;
                }
                else if (JuLi <= difference_right)//在停车范围内，不能判定为中途停车
                {
                    detIsStop = false;
                }
                break;
            case Step.Carbarn:
                Debug.DrawLine(targetPlayer.position, point_carbarn.position, Color.red);
                //X轴度数基准向量
                Vector3 carbarnWord_a = point_carbarn.TransformPoint(Vector3.right);
                carbarnWord_a = carbarnWord_a - point_carbarn.position;
                //库点指向车辆
                Vector3 carbarnWord_b = targetPlayer.position - point_carbarn.position;
                //度数
                float Angle = Mathf.Acos(Vector3.Dot(carbarnWord_a.normalized, carbarnWord_b.normalized));
                //车与库点距离
                JuLi = Vector3.Distance(targetPlayer.position, point_carbarn.position);
                //Ab用于判断车辆是否在停车位内
                Ab = Mathf.Cos(Angle) * JuLi;
                if (Car_Information.isStop && Ab <= difference_carbarn)
                {
                    targetPlayer.GetComponent<VoicePromptAudio>().Play_TingCheYiDaoWei();
                    ParkingCount++;
                    step = Step.Left;
                }
                else if (JuLi <= difference_carbarn)//在停车范围内，不能判定为中途停车
                {
                    detIsStop = false;
                }
                break;
            case Step.Default:
                break;
        }

        //中途停车判定（如果车辆是停止的且不在停车范围内）
        if (_isStop && detIsStop)
        {
            targetPlayer.GetComponent<VoicePromptAudio>().Play_ZhongTuTingChe();
            Car_Information.DeductScore(5);
            detIsStop = false;
        }
        //两次倒库已经完成
        if (ParkingCount == 2)
        {
            Destroy(AirWall);//去掉空气墙
            step = Step.Default;
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
