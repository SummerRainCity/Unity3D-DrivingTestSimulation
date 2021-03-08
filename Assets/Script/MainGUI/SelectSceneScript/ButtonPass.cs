using UnityEngine.EventSystems;
using UnityEngine;

/// <summary>
/// 用户选择的关卡
/// </summary>
public class ButtonPass : MonoBehaviour, IPointerUpHandler
{
    public SelectPass.Sites sites = SelectPass.Sites.Dcrk;
    public void OnPointerUp(PointerEventData eventData)
    {
        SelectPass sp = FindObjectOfType<SelectPass>();
        sp.sites = sites;
        SelectScene ss = FindObjectOfType<SelectScene>();
        ss.Scene_ExamMode();
    }
}
