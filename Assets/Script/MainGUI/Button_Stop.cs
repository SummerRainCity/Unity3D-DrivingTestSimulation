using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 刹车踏板按下时
/// </summary>
public class Button_Stop : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private DrivingKernel driving;

    private void Start()
    {
        driving = FindObjectOfType<DrivingKernel>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        driving.MobileTerminal_BrakePedal(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        driving.MobileTerminal_BrakePedal(false);
    }
}