using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Button_Rec : MonoBehaviour/*, IPointerDownHandler, IPointerUpHandler*/
{
    /*
     public Replay.ReplayManager replay;//仅用于开始重播
    public DrivingKernel dp;//仅用于停车
    bool conversion = true;

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (conversion)
        {
            //取消物理检测（汽车立刻停止）
            dp.GetComponentInParent<Rigidbody>().isKinematic = true;
            //停止车辆运行
            dp.Resistance();
            //开始回播
            replay.StartReplay();
            //提示RES
            this.GetComponentInChildren<Text>().text = "RES";//restart
            this.GetComponentInChildren<Text>().color = Color.black;
            conversion = false;
            //再次设定false，方便回放可以拖动实时查看车辆状态。
            dp.GetComponentInParent<Rigidbody>().isKinematic = false;
        }
        else
        {
            //重新加载场景
            Scene scene = SceneManager.GetActiveScene();//获取当前场景名
            SceneManager.LoadScene(scene.name);
        }
    }
     */
}