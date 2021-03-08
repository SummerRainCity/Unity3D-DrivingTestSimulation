using UnityEngine;
using System.Collections;

/// <summary>
/// 【汽车核心类】汽车动能类
/// </summary>
public class DrivingKernel : MonoBehaviour
{
    [Header("汽车核心驱动类(PC/Phone)")]
    public WheelCollider flWheelCollider;
    public WheelCollider frWheelCollider;
    public WheelCollider rlWheelCollider;
    public WheelCollider rrWheelCollider;

    public Transform flWheelModel;//内部控制轮子转动（子物体）
    public Transform frWheelModel;//内部控制轮子转动（子物体）
    public Transform rlWheelModel;
    public Transform rrWheelModel;

    public Transform flDiscBrake, frDiscBrake, centerOfMass;//控制转向外部（父物体），centerOfMass是重心

    public float motorTorque = 1600;//最大马达值
    public float steerAngle = 38;//车轮转向角度
    public float brakeTorque = 2000;//刹车力量

    public Transform SteeringWheel;//方向盘-变换组件
    public float CurrentWheelAngle = 0;//当前车轮旋转角度

    //方向盘总角度：550=360+180+10
    //转换比（520/38=13.684210526315789473684210526316）--方向盘总度数520：转向向角38
    public const float ratio = 13.684210526315789473684210526316f;
    [Header("就是汽车实际运行速度(KM/H)")]
    public float speed = 0;
    public float speedOrigin = 0;
    
    [Header("方向盘UI预制件")]
    [Tooltip("自动查找并赋值，无需管理！")]
    public SteeringWheel2D steeringWheelUi;//UI方向盘鱼前车轮同步

    [Header("UI档位类")]
    [Tooltip("自动查找并赋值，无需管理！")]
    [SerializeField]
    private GearsDangWei_Control Gears;
    [SerializeField]
    private Car_HandleBrake Handle;
    private bool Handle_Flag = true;

    private void Start()
    {
        //利用centerOfMass空物体（这个物体在车地盘下）改变车的重心，防止翻车。
        Rigidbody rigidboy = this.GetComponentInParent<Rigidbody>();
        rigidboy.centerOfMass = centerOfMass.localPosition;

        Gears = FindObjectOfType<GearsDangWei_Control>();
        steeringWheelUi = FindObjectOfType<SteeringWheel2D>();
        Handle = FindObjectOfType<Car_HandleBrake>();
    }

    private void Update()
    {
        /**********************************【PC端】*************************************
        //空格-刹车
        if (Input.GetKey(KeyCode.Space))
        {
            rlWheelCollider.motorTorque = 0;
            rrWheelCollider.motorTorque = 0;
            rlWheelCollider.brakeTorque = brakeTorque;
            rrWheelCollider.brakeTorque = brakeTorque;
        }
        //W|S键，开始
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            rlWheelCollider.brakeTorque = 0;
            rrWheelCollider.brakeTorque = 0;
        }
        //动力（车轮碰撞器）
        rlWheelCollider.motorTorque = Input.GetAxis("Vertical") * motorTorque;
        rrWheelCollider.motorTorque = Input.GetAxis("Vertical") * motorTorque;
        //转向（车轮碰撞器）
        flWheelCollider.steerAngle = Input.GetAxis("Horizontal") * steerAngle;
        frWheelCollider.steerAngle = Input.GetAxis("Horizontal") * steerAngle;
        /**********************************PC端-END****************************************/

        /******************【移动端】UI方向盘旋转时控制轮子碰撞器的转动*******************/
        flWheelCollider.steerAngle = steeringWheelUi.wheelAngle / ratio;
        frWheelCollider.steerAngle = steeringWheelUi.wheelAngle / ratio;
        /*********************************************************************************/

        //转向系统
        SteerWheel();
        //转动车轮
        RotateWheel();
        //手刹状态
        if (Handle.Activate)
        {
            Handle_Flag = true;
            rlWheelCollider.brakeTorque = brakeTorque;
            rrWheelCollider.brakeTorque = brakeTorque;
        }
        else if(Handle_Flag)
        {
            Handle_Flag = false;
            rlWheelCollider.brakeTorque = 0;
            rrWheelCollider.brakeTorque = 0;
        }
    }

    /*
     * 车轮转动起来，转起来（注意：不是转弯！）
     * 前轮应该是子物体
     */
    private void RotateWheel()
    {
        //前轮（子物体）
        flDiscBrake.Rotate(flWheelCollider.rpm * 6 * Time.deltaTime * Vector3.right);
        frDiscBrake.Rotate(frWheelCollider.rpm * 6 * Time.deltaTime * Vector3.right);
        //后轮
        rrWheelModel.Rotate(rrWheelCollider.rpm * 6 * Time.deltaTime * Vector3.right);
        rlWheelModel.Rotate(rlWheelCollider.rpm * 6 * Time.deltaTime * Vector3.right);

        //汽车速度
        speed = flWheelCollider.rpm * (flWheelCollider.radius * 2 * Mathf.PI) * 60 / 1000;
        //车速-去掉小数点
        speed = Mathf.Round(speed);
        speedOrigin = speed;
        //绝对值
        speed = Mathf.Abs(speed);
    }

    /*
     * 转向
     */
    private void SteerWheel()
    {
        //获取当前轮子转向角度
        Vector3 localEulerAngles = flWheelModel.localEulerAngles;
        localEulerAngles.y = flWheelCollider.steerAngle;

        flWheelModel.localEulerAngles = localEulerAngles;//控制转向外部（flDiscBrake-父物体）
        frWheelModel.localEulerAngles = localEulerAngles;

        CurrentWheelAngle = flWheelCollider.steerAngle;

        //同步方向盘（OK，是延自身旋转）
        Vector3 locaRv3 = SteeringWheel.localRotation.eulerAngles;
        locaRv3.z = -CurrentWheelAngle * ratio;
        SteeringWheel.localRotation = Quaternion.Euler(locaRv3);
    }

    /// <summary>
    /// PUBLIC-油门踏板
    /// </summary>
    /// <param name="press">踏板踩下标记</param>
    public void MobileTerminal_AcceleratorPedal(bool press)
    {
        if(press)
        {
            switch (Gears.gears)
            {
                case GearsDangWei_Control.currentGears.P:
                    break;
                case GearsDangWei_Control.currentGears.R:
                    EnginePower(false);
                    break;
                case GearsDangWei_Control.currentGears.N:
                    CancelEnginePower();
                    break;
                case GearsDangWei_Control.currentGears.D:
                    EnginePower(true);
                    break;
            }
        }
        else
        {
            CancelEnginePower();
            CancelResistance();
        }
    }

    /// <summary>
    /// PUBLIC-刹车踏板
    /// </summary>
    /// <param name="press">踏板踩下标记</param>
    public void MobileTerminal_BrakePedal(bool press)
    {
        switch (press)
        {
            case true:
                Resistance();
                break;
            case false:
                CancelResistance();
                break;
        }
    }

    /// <summary>
    /// 【动力提供】
    /// </summary>
    /// <param name="flag">“真”前进，“假”后退。</param>
    private void EnginePower(bool flag)
    {
        switch (flag)
        {
            case true:
                //前进
                rlWheelCollider.motorTorque = motorTorque;
                rrWheelCollider.motorTorque = motorTorque;
                break;
            case false:
                //后退
                rlWheelCollider.motorTorque = -motorTorque;
                rrWheelCollider.motorTorque = -motorTorque;
                break;
        }
    }

    /// <summary>
    /// 【阻断发动机动力】没有动力
    /// </summary>
    private void CancelEnginePower()
    {
        rlWheelCollider.motorTorque = 0;
        rrWheelCollider.motorTorque = 0;
    }

    /// <summary>
    /// 【刹车】四轮子同时刹车
    /// </summary>
    public void Resistance()
    {
        //四个轮子同时刹车
        flWheelCollider.brakeTorque = brakeTorque;
        frWheelCollider.brakeTorque = brakeTorque;
        rlWheelCollider.brakeTorque = brakeTorque;
        rrWheelCollider.brakeTorque = brakeTorque;
    }

    /// <summary>
    /// 【取消所有轮子阻力】
    /// </summary>
    public void CancelResistance()
    {
        flWheelCollider.brakeTorque = 0;
        frWheelCollider.brakeTorque = 0;
        rlWheelCollider.brakeTorque = 0;
        rrWheelCollider.brakeTorque = 0;
    }
}