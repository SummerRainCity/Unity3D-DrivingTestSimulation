using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 时间：2021-1-23-08点32分
/// 功能：汽车转向灯
/// </summary>
public class TurnLight : MonoBehaviour
{
    public enum CarTurnLight
    {
        Off,Left,Right,Emergency
    };

    public Light TurnLight_Front_L;
    public Light TurnLight_Front_R;
    public Light TurnLight_Back_L;
    public Light TurnLight_Back_R;

    [Header("汽车转向灯状态：关闭、左转、右转、应急")]
    public CarTurnLight CTL;
    [Header("汽车转向灯闪动间隔（单位：秒）")]
    public float Interval = 0.45f;

    [Header("转向灯UI元素")]
    public Image Img_R;
    public Image Img_E;
    public Image Img_L;
    private Color colorHide = new Color(1, 1, 1, 0.7f);
    private Color colorShow = new Color(1, 1, 1, 1f);

    [Header("转向灯音频（自动赋值）")]
    [SerializeField]
    private AudioSource _audio;

    private void Start()
    {
        Img_L.color = colorHide;
        Img_R.color = colorHide;
        Img_E.color = colorHide;
        _audio = GetComponent<AudioSource>();
        //默认关闭所有转向灯
        CTL = CarTurnLight.Off;

        TurnLight_Front_L.gameObject.SetActive(false);
        TurnLight_Front_R.gameObject.SetActive(false);
        TurnLight_Back_L.gameObject.SetActive(false);
        TurnLight_Back_R.gameObject.SetActive(false);
    }
    bool FirstFlag = true;
    private void Update()
    {
        if(CTL != CarTurnLight.Off && FirstFlag)
        {
            FirstFlag = false;
            StartWork();
        }
    }

    /// <summary>
    ///回调函数 ：每0.45秒执行一次
    /// </summary>
    private void Fun()
    {
        switch (CTL)
        {
            case CarTurnLight.Off:
                StopWork();
                break;
            case CarTurnLight.Left:
                //先关闭右侧灯
                TurnLight_Front_R.gameObject.SetActive(false);
                TurnLight_Back_R.gameObject.SetActive(false);
                Img_R.color = colorHide;
                Img_E.color = colorHide;
                //开始闪动
                TurnLight_Front_L.gameObject.SetActive(!TurnLight_Front_L.gameObject.activeInHierarchy);
                TurnLight_Back_L.gameObject.SetActive(!TurnLight_Back_L.gameObject.activeInHierarchy);
                //转向灯颜色闪动
                if(Img_L.color.a == 1f)
                {
                    Img_L.color = colorHide;
                }
                else
                {
                    Img_L.color = colorShow;
                }
                break;
            case CarTurnLight.Right:
                //先关闭左侧灯
                TurnLight_Front_L.gameObject.SetActive(false);
                TurnLight_Back_L.gameObject.SetActive(false);
                Img_L.color = colorHide;
                Img_E.color = colorHide;
                //开始闪动
                TurnLight_Front_R.gameObject.SetActive(!TurnLight_Front_R.gameObject.activeInHierarchy);
                TurnLight_Back_R.gameObject.SetActive(!TurnLight_Back_R.gameObject.activeInHierarchy);
                //转向灯颜色闪动
                if (Img_R.color.a == 1f)
                {
                    Img_R.color = colorHide;
                }
                else
                {
                    Img_R.color = colorShow;
                }
                break;
            case CarTurnLight.Emergency:
                //如果两边转向灯[开/关]不对等，那么应先让它们对等再进行闪动
                if (TurnLight_Front_L.gameObject.activeInHierarchy && !TurnLight_Front_R.gameObject.activeInHierarchy ||
                    TurnLight_Front_R.gameObject.activeInHierarchy && !TurnLight_Front_L.gameObject.activeInHierarchy)
                {
                    Img_L.color = colorHide;
                    Img_R.color = colorHide;
                    TurnLight_Front_L.gameObject.SetActive(false);
                    TurnLight_Front_R.gameObject.SetActive(false);
                    TurnLight_Back_L.gameObject.SetActive(false);
                    TurnLight_Back_R.gameObject.SetActive(false);
                }
                //开始闪动
                TurnLight_Front_L.gameObject.SetActive(!TurnLight_Front_L.gameObject.activeInHierarchy);
                TurnLight_Front_R.gameObject.SetActive(!TurnLight_Front_R.gameObject.activeInHierarchy);
                TurnLight_Back_L.gameObject.SetActive(!TurnLight_Back_L.gameObject.activeInHierarchy);
                TurnLight_Back_R.gameObject.SetActive(!TurnLight_Back_R.gameObject.activeInHierarchy);
                //转向灯颜色闪动
                if (Img_L.color.a == 1f)
                {
                    Img_L.color = colorHide;
                    Img_R.color = colorHide;
                    Img_E.color = colorHide;
                }
                else
                {
                    Img_L.color = colorShow;
                    Img_R.color = colorShow;
                    Img_E.color = colorShow;
                }
                break;
        }
    }

    /// <summary>
    /// 开始工作
    /// </summary>
    private void StartWork()
    {
        InvokeRepeating("Fun", 0.3f, Interval);
    }

    /// <summary>
    /// 关闭所有转向灯，停止灯光闪动
    /// </summary>
    private void StopWork()
    {
        //关闭UI界面的灯光闪动
        Img_L.color = colorHide;
        Img_R.color = colorHide;
        Img_E.color = colorHide;
        //关闭汽车所有灯光闪动
        TurnLight_Front_L.gameObject.SetActive(false);
        TurnLight_Front_R.gameObject.SetActive(false);
        TurnLight_Back_L.gameObject.SetActive(false);
        TurnLight_Back_R.gameObject.SetActive(false);
        CancelInvoke("Fun");
        FirstFlag = true;
    }

    /// <summary>
    /// 三个BOOL类型，分别检测三种灯状态的第一次与第二次检测。
    /// 其中第一次开，第二次关，三种状态只能有一个是开，所以
    /// 第一次某个状态是开，那么其余应反之。
    /// </summary>
    private bool L = true;//左转弯灯检测
    private bool R = true;//有转弯灯检测
    private bool E = true;//应急灯
    /// <summary>
    /// UI界面调用，用户点击那个灯就闪动那个转弯灯！再次点击
    /// 则关闭此转弯灯
    /// </summary>
    public void SetLeft()
    {
        if (L) //如果是第一次点左转弯灯，未点击状态
        {
            PlayAudio();
            //ER=true意义：防止在右转弯灯、应急灯还没关闭时，点击此左转弯灯然后又点击右转弯灯或者应急灯
            //造成点击右转弯灯或者应急灯时出现二次点击才能正常打开的BUG
            E = true;//应急应为未点击
            R = true;//右转应为未点击

            L = false;//左转弯已点击
            CTL = CarTurnLight.Left;//开启左转弯灯
        }
        else
        {
            StopAudio();
            L = true;//左转弯灯实际为第二次点击，恢复到未点击状态
            CTL = CarTurnLight.Off;//关闭左转弯灯
        }
    }
    public void SetRight()
    {
        if (R)
        {
            PlayAudio();
            L = true;
            E = true;
            R = false;
            CTL = CarTurnLight.Right;
        }
        else
        {
            StopAudio();
            R = true;
            CTL = CarTurnLight.Off;
        }
    }
    public void SetExigency()
    {
        if (E)
        {
            PlayAudio();
            L = true;
            R = true;
            E = false;
            CTL = CarTurnLight.Emergency;
        }
        else
        {
            StopAudio();
            E = true;
            CTL = CarTurnLight.Off;
        }
    }

    private void PlayAudio()
    {
        _audio.Play();
    }
    private void StopAudio()
    {
        _audio.Stop();
    }
}
