using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SteeringWheelUI_Backup : MonoBehaviour
{
    [Header("方向盘UI组件")]
    [Tooltip("方向盘组件，一般是Image图片")]
    public GameObject SteeringWheel;
    private Canvas CanvasRoot;//画布 
    private RectTransform m_RectTransform;//坐标

    private bool m_IsFirst = true;           //用于记录第一帧按下鼠标时鼠标的位置，便于计算
    private Vector3 m_CurrentPos;            //记录当前帧鼠标所在位置
    private bool m_IsClockwise;              //是否顺时针
    [Tooltip("方向盘复位速度")]
    public float Sleep = 5f;
    [Tooltip("最大旋转角度")]
    public float m_MaxRoundValue = 550;
    [Tooltip("当前实际旋转角度")]
    public float m_RoundValue = 0;          //记录总的旋转角度 用这个数值来控制一圈半
    private bool m_IsTuringSteeringWheel;    //是否在转方向盘 用这个判断复位

    private void Start()
    {
        CanvasRoot = GameObject.Find("GUICanvas").GetComponent<Canvas>();
        m_RectTransform = CanvasRoot.transform as RectTransform;
    }

    public Text testText;
    private void Update()
    {
        for (var i = 0; i < Input.touchCount; ++i)
        {
            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                Vector2 vpos2 = Input.GetTouch(i).position;
                testText.text = "[" + vpos2.x + "," + vpos2.y + "]";
            }
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButton(0))
            {
                PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
                eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
                //点击的物体是：results[0].gameObject

                if (results[0].gameObject.tag == "fxp_panel")                 //当鼠标点击到方向盘时
                {
                    m_IsTuringSteeringWheel = true;
                    Vector2 pos;
                    if (RectTransformUtility.ScreenPointToLocalPointInRectangle(m_RectTransform, Input.mousePosition, CanvasRoot.worldCamera, out pos))    //获取鼠标点击位置
                    {
                        pos.x = pos.x + (Screen.width / 2) - GetComponent<RectTransform>().position.x;
                        pos.y = pos.y + (Screen.height / 2) - GetComponent<RectTransform>().position.y;

                        Vector3 pos3 = new Vector3(pos.x, pos.y, 0);                           //计算后鼠标以方向盘圆心为坐标原点的坐标位置
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

        if (Input.GetMouseButtonUp(0))
        {
            m_IsFirst = true;
            m_IsTuringSteeringWheel = false;
        }

        if (!m_IsTuringSteeringWheel && m_RoundValue != 0)               //复位
        {
            if (m_RoundValue >= 0)
            {
                m_RoundValue -= Sleep;               //复位速度
                if (m_RoundValue < 0)
                    m_RoundValue = 0;
                SteeringWheel.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -m_RoundValue));
            }
            else
            {
                m_RoundValue += Sleep;
                if (m_RoundValue > 0)
                    m_RoundValue = 0;
                SteeringWheel.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -m_RoundValue));
            }
        }
    }
}