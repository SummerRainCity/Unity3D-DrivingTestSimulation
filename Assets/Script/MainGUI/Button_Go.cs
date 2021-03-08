using UnityEngine;
using UnityEngine.EventSystems;

public class Button_Go : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("油门踏板")]
    private DrivingKernel driving;

    private void Start()
    {
        driving = FindObjectOfType<DrivingKernel>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        driving.MobileTerminal_AcceleratorPedal(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        driving.MobileTerminal_AcceleratorPedal(false);
    }
}