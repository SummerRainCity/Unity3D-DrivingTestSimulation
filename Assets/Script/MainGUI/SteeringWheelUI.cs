using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SteeringWheelUI : MonoBehaviour
{
    [Header("方向盘UI组件，一般是Image图片")]
    public GameObject SteeringWheel;
    private Canvas CanvasRoot;//画布 
    private RectTransform m_RectTransform;//坐标

    private bool m_IsFirst = true;           //用于记录第一帧按下鼠标时鼠标的位置，便于计算
    private Vector3 m_CurrentPos;            //记录当前帧鼠标所在位置
    private bool m_IsClockwise;              //是否顺时针
    [SerializeField]
    [Header("方向盘复位速度")]
    private float Sleep = 600f;
    [Header("最大旋转角度")]
    public float m_MaxRoundValue = 520f;

    [Header("当前实际旋转角度")]
    public float m_RoundValue = 0;           //记录总的旋转角度 用这个数值来控制一圈半
    private bool m_IsTuringSteeringWheel;    //是否在转方向盘 用这个判断复位

    /*
    ScreenPointToLocalPointInRectangle函数最终给posQuadrant的是四象限，象限中心是[0,0]，鼠标随着
    这个中心滑动来拖动方向盘。需要注意的是，你需要设定offsetWidth与interHeight来修正这个中心。
    */
    public Vector2 posQuadrant;             //获取的值是转换后的四象限
    public InputField offsetWidth, offsetHeight;//Test:对于四象限中心的偏差修正
    public Text textX, textY, textZ;//Test:

    private void Start()
    {
        CanvasRoot = GameObject.Find("GUI_Control").GetComponent<Canvas>();
        m_RectTransform = CanvasRoot.transform as RectTransform;
    }

    private PointerEventData eventDataCurrentPosition;
    private void Update()
    {
        //string offsetstrw = offsetwidth.text;//Test
        //string offsetstrh = offsetheight.text;//Test
        //float offsetw = convert.tosingle(offsetstrw);//Test
        //float offseth = convert.tosingle(offsetstrh);//Test

        //触摸点个数
        int count = Input.touchCount;
        /*--------------------------实时检测用户多触摸情况下是否在转方向盘-----------------------
        ----------------作用：根据一个点击坐标点，获取所有UI同级组件、或获得当前级(包括当前及同辈)下所有子UI组件-----------
        ----------------注意：直接使用多点触控将失效，因为它不知道你到底是要检测那个坐标！-----------
        教程参考链接：https://blog.csdn.net/weixin_41814169/article/details/88682994
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        */
        /****************************************移动端*****************************************/
        for (int i = 0; i < count; i++)
        {
            Vector2 mousePosition = new Vector2();
            mousePosition.x = Input.GetTouch(i).position.x;
            mousePosition.y = Input.GetTouch(i).position.y;
            eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            for (int k = 0; k < results.Count; k++)
            {
                if (results[k].gameObject.tag == "fxp_panel")
                {
                    m_IsTuringSteeringWheel = true;
                    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_RectTransform, mousePosition, CanvasRoot.worldCamera, out posQuadrant))    //获取鼠标点击位置
                    {
                        //posQuadrant.x = posQuadrant.x + (Screen.width / 2) - GetComponent<RectTransform>().position.x - offsetW;//Test
                        //posQuadrant.y = posQuadrant.y + (Screen.height / 2) - GetComponent<RectTransform>().position.y - offsetH;//Test

                        posQuadrant.x = posQuadrant.x + (Screen.width / 2) - GetComponent<RectTransform>().position.x - 560;
                        posQuadrant.y = posQuadrant.y + (Screen.height / 2) - GetComponent<RectTransform>().position.y - 350;

                        Vector3 pos3 = new Vector3(posQuadrant.x, posQuadrant.y, 0);                           //计算后鼠标以方向盘圆心为坐标原点的坐标位置

                        //textX.text = "X:" + ((int)(pos3.x)).ToString();//Test
                        //textY.text = "Y:" + ((int)(pos3.y)).ToString();//Test
                        //textZ.text = "Z:" + ((int)(pos3.z)).ToString();//Test

                        if (m_IsFirst)
                        {
                            m_CurrentPos = pos3;
                            m_IsFirst = false;
                        }

                        Vector3 currentPos = Vector3.Cross(pos3, m_CurrentPos);             //计算当前帧和上一帧手指位置 用于判断旋转方向
                        if (currentPos.z > 0)
                        {
                            m_IsClockwise = true;
                        }
                        else if (currentPos.z < 0)
                        {
                            m_IsClockwise = false;
                        }

                        if (m_CurrentPos != pos3)                                 //范围内让方向盘随着手指转动
                        {
                            if (m_IsClockwise)
                            {
                                if (m_RoundValue <= m_MaxRoundValue)
                                {
                                    m_RoundValue += Vector3.Angle(m_CurrentPos, pos3);

                                    SteeringWheel.transform.Rotate(new Vector3(0, 0, -Vector3.Angle(m_CurrentPos, pos3)));
                                }
                            }
                            else
                            {
                                if (m_RoundValue >= -m_MaxRoundValue)
                                {
                                    m_RoundValue -= Vector3.Angle(m_CurrentPos, pos3);

                                    SteeringWheel.transform.Rotate(new Vector3(0, 0, Vector3.Angle(m_CurrentPos, pos3)));
                                }
                            }
                        }
                        m_CurrentPos = pos3;
                    }
                }
            }
        }
        bool flag = true;
        for (int i = 0; i < count; i++)
        {
            Vector2 mousePosition = new Vector2();
            mousePosition.x = Input.GetTouch(i).position.x;
            mousePosition.y = Input.GetTouch(i).position.y;
            eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            for (int k = 0; k < results.Count; k++)
            {
                if (results[k].gameObject.tag == "fxp_panel")
                {
                    flag = false;
                    break;
                }
            }
        }
        if (flag)
        {
            m_IsFirst = true;
            m_IsTuringSteeringWheel = false;
        }
        /**********************************移动端-END****************************/
        
        /*-----------------------------------PC端------------------------------*
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePosition = Input.mousePosition;
            eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            if (results[0].gameObject.tag == "fxp_panel")
            {
                m_IsTuringSteeringWheel = true;
                Vector2 posQuadrant;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_RectTransform, Input.mousePosition, CanvasRoot.worldCamera, out posQuadrant))    //获取鼠标点击位置
                {
                    posQuadrant.x = posQuadrant.x + (Screen.width / 2) - GetComponent<RectTransform>().position.x;
                    posQuadrant.y = posQuadrant.y + (Screen.height / 2) - GetComponent<RectTransform>().position.y;

                    gx = GetComponent<RectTransform>().position.x;
                    gy = GetComponent<RectTransform>().position.y;

                    Vector3 pos3 = new Vector3(posQuadrant.x, posQuadrant.y, 0);                           //计算后鼠标以方向盘圆心为坐标原点的坐标位置
                    if (m_IsFirst)
                    {
                        m_CurrentPos = pos3;
                        m_IsFirst = false;
                    }

                    Vector3 currentPos = Vector3.Cross(pos3, m_CurrentPos);             //计算当前帧和上一帧手指位置 用于判断旋转方向
                    if (currentPos.z > 0)
                    {
                        m_IsClockwise = true;
                    }
                    else if (currentPos.z < 0)
                    {
                        m_IsClockwise = false;
                    }

                    if (m_CurrentPos != pos3)                                 //范围内让方向盘随着手指转动
                    {
                        if (m_IsClockwise)
                        {
                            if (m_RoundValue <= m_MaxRoundValue)
                            {
                                m_RoundValue += Vector3.Angle(m_CurrentPos, pos3);

                                SteeringWheel.transform.Rotate(new Vector3(0, 0, -Vector3.Angle(m_CurrentPos, pos3)));
                            }
                        }
                        else
                        {
                            if (m_RoundValue >= -m_MaxRoundValue)
                            {
                                m_RoundValue -= Vector3.Angle(m_CurrentPos, pos3);

                                SteeringWheel.transform.Rotate(new Vector3(0, 0, Vector3.Angle(m_CurrentPos, pos3)));
                            }
                        }
                        m_CurrentPos = pos3;
                    }
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            m_IsFirst = true;
            m_IsTuringSteeringWheel = false;
        }
        /*-----------------------------------PC端------------------------------*/

        /*--------------------------方向盘复位（PC与移动通用）-----------------------*/
        if (!m_IsTuringSteeringWheel && m_RoundValue != 0)               //复位
        {
            if (m_RoundValue >= 0)
            {
                m_RoundValue = m_RoundValue - Sleep * Time.deltaTime;               //复位速度
                if (m_RoundValue < 0)
                    m_RoundValue = 0;
                SteeringWheel.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -m_RoundValue));
            }
            else
            {
                m_RoundValue = m_RoundValue + Sleep * Time.deltaTime;
                if (m_RoundValue > 0)
                    m_RoundValue = 0;
                SteeringWheel.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -m_RoundValue));
            }
        }
    }
}