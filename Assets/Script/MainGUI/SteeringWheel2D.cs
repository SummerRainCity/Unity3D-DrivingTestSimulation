using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System.Collections;

/// <summary>
/// Steering wheel.
/// Defines the steerign wheel behaviour.
/// Original source:
/// http://forum.unity3d.com/threads/touchscreen-steering-wheel-rotation-example-mouse-supported.196741/
/// </summary>
public class SteeringWheel2D : MonoBehaviour
{
    [Header("方向盘UI本体")]
    public Graphic UI_Element;
    [Header("方向盘最大旋转角度")]
    public float maximumSteeringAngle = 520f;
    [Header("方向盘旋转复原速度")]
    public float wheelReleasedSpeed = 700f;
    [Header("方向盘当前旋转角度")]
    public float wheelAngle = 0f;
    float wheelPrevAngle = 0f;
    bool wheelBeingHeld = false;
    RectTransform rectT;
    [Header("方向盘旋转中心点")]
    public Vector2 centerPoint;

    /// <summary>
    /// Gets the clamped value.
    /// Returns a value in range [-1,1] similar to GetAxis("Horizontal")
    /// </summary>
    /// <returns>The clamped value.</returns>
    public float GetClampedValue()
    {

        return wheelAngle / maximumSteeringAngle;
    }

    /// <summary>
    /// Gets the angle.
    /// returns the wheel angle itself without clamp operation
    /// </summary>
    /// <returns>The angle.</returns>
    public float GetAngle()
    {
        return wheelAngle;
    }

    /// <summary>
    /// Use this for initialization.
    /// </summary>
    void Start()
    {
        rectT = UI_Element.rectTransform;
        InitEventsSystem();
        UpdateRect();
    }

    /// <summary>
    /// Initializes the events system to catch
    /// the user movements.i.e. MouseDown/MouseDrag/MouseUp.
    /// </summary>
    void InitEventsSystem()
    {
        EventTrigger events = UI_Element.gameObject.GetComponent<EventTrigger>();

        if (events == null)
            events = UI_Element.gameObject.AddComponent<EventTrigger>();

        if (events.triggers == null)
            events.triggers = new System.Collections.Generic.List<EventTrigger.Entry>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        EventTrigger.TriggerEvent callback = new EventTrigger.TriggerEvent();
        UnityAction<BaseEventData> functionCall = new UnityAction<BaseEventData>(PressEvent);
        callback.AddListener(functionCall);
        entry.eventID = EventTriggerType.PointerDown;
        entry.callback = callback;

        events.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        callback = new EventTrigger.TriggerEvent();
        functionCall = new UnityAction<BaseEventData>(DragEvent);
        callback.AddListener(functionCall);
        entry.eventID = EventTriggerType.Drag;
        entry.callback = callback;

        events.triggers.Add(entry);

        entry = new EventTrigger.Entry();
        callback = new EventTrigger.TriggerEvent();
        functionCall = new UnityAction<BaseEventData>(ReleaseEvent);
        callback.AddListener(functionCall);
        entry.eventID = EventTriggerType.PointerUp;
        entry.callback = callback;

        events.triggers.Add(entry);
    }

    /// <summary>
    /// Initializes the center point of seetring wheel transform
    /// rectangle.
    /// 初始化方向盘变换矩形的旋转中心点。
    /// </summary>
    void UpdateRect()
    {
        Vector3[] corners = new Vector3[4];
        rectT.GetWorldCorners(corners);
        for (int i = 0; i < 4; i++)
        {
            corners[i] = RectTransformUtility.WorldToScreenPoint(null, corners[i]);
        }
        Vector3 bottomLeft = corners[0];
        Vector3 topRight = corners[2];
        float width = topRight.x - bottomLeft.x;
        float height = topRight.y - bottomLeft.y;
        Rect _rect = new Rect(bottomLeft.x, topRight.y, width, height);
        //centerPoint = new Vector2(_rect.x + _rect.width * 0.5f, _rect.y - _rect.height * 0.5f);//源代码

        //修正代码：
        //  你需要注意，其实下面的是不必要的，这里是为了防止下面_rect赋值出现偶然问题。
        centerPoint = new Vector2(180, 180);
        centerPoint.x = _rect.x + _rect.width * 0.5f;
        centerPoint.y = _rect.y - _rect.height * 0.5f;
    }


    /// <summary>
    /// Update is called once per frame.
    /// Update the wheel status. If the 
    /// wheel is released, reset the rotation
    /// to initial (zero) rotation by 
    /// wheelReleasedSpeed degrees per second
    /// </summary>
    void Update()
    {
        if (!wheelBeingHeld && !Mathf.Approximately(0f, wheelAngle))
        {
            float deltaAngle = wheelReleasedSpeed * Time.deltaTime;
            if (Mathf.Abs(deltaAngle) > Mathf.Abs(wheelAngle))
                wheelAngle = 0f;
            else if (wheelAngle > 0f)
                wheelAngle -= deltaAngle;
            else
                wheelAngle += deltaAngle;
        }
        rectT.localEulerAngles = Vector3.back * wheelAngle;
    }

    /// <summary>
    /// Presses the event.
    /// Executed when mouse/finger starts touching the steering wheel.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void PressEvent(BaseEventData eventData)
    {
        Vector2 pointerPos = ((PointerEventData)eventData).position;
        wheelBeingHeld = true;
        wheelPrevAngle = Vector2.Angle(Vector2.up, pointerPos - centerPoint);
    }

    /// <summary>
    /// Drags the event.
    /// Executed when mouse/finger is dragged over the steering wheel.
    /// Do nothing if the pointer is too close to the center of the wheel.
    /// Make sure wheel angle never exceeds maximumSteeringAngle.
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void DragEvent(BaseEventData eventData)
    {

        Vector2 pointerPos = ((PointerEventData)eventData).position;

        float wheelNewAngle = Vector2.Angle(Vector2.up, pointerPos - centerPoint);

        if (Vector2.Distance(pointerPos, centerPoint) > 20f)
        {
            if (pointerPos.x > centerPoint.x)
                wheelAngle += wheelNewAngle - wheelPrevAngle;
            else
                wheelAngle -= wheelNewAngle - wheelPrevAngle;
        }

        wheelAngle = Mathf.Clamp(wheelAngle, -maximumSteeringAngle, maximumSteeringAngle);
        wheelPrevAngle = wheelNewAngle;
    }

    /// <summary>
    /// Executed when mouse/finger stops touching the steering wheel
    /// Performs one last DragEvent, just in case
    /// </summary>
    /// <param name="eventData">Event data.</param>
    public void ReleaseEvent(BaseEventData eventData)
    {
        DragEvent(eventData);
        wheelBeingHeld = false;
    }
}
