using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 【第一人称】主驾驶员视野
/// </summary>
public class FPD : MonoBehaviour
{
    [Tooltip("初始视角距离，默认最大距离")]
    public float distance;
    [Tooltip("缩放速度")]
    public float mmSpeed = 70;
    [Tooltip("最小放大距离")]
    public const float minDistance = 10;
    [Tooltip("最大放大距离")]
    public const float maxDistance = 70;
    private Camera camera;

    private void Start()
    {
        camera = GetComponent<Camera>();
        distance = maxDistance;
    }

    public float X;
    void FixedUpdate()
    {
        /***********************************PC端***************************
        distance -= Input.GetAxis("Mouse ScrollWheel") * mmSpeed;
        if (distance >= maxDistance)
        {
            distance = maxDistance;
        }
        else if (distance <= minDistance)
        {
            distance = minDistance;
        }
        if (distance != camera.fieldOfView) camera.fieldOfView = distance;

        float x = Input.GetAxis("Mouse X");
        float y = Input.GetAxis("Mouse Y");
        if (x != 0 || y != 0)
            RotateView(x, y);
        /************************************************************************/

        /***********************移动端-触摸点个数******************************/
        int count = Input.touchCount;
        for (int i = 0; i < count; i++)
        {
            Vector2 mousePosition = new Vector2();
            mousePosition.x = Input.GetTouch(i).position.x;
            mousePosition.y = Input.GetTouch(i).position.y;
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = mousePosition;
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            for (int k = 0; k < results.Count; k++)
            {
                if (results[k].gameObject.tag == "camera_panel")
                {
                    if (Input.touchCount > 0 && Input.GetTouch(i).phase == TouchPhase.Moved)
                    {
                        float x = Input.GetAxis("Mouse X");
                        float y = Input.GetAxis("Mouse Y");
                        if (x != 0 || y != 0)
                            RotateView(x, y);
                    }
                }
            }
        }
        /***************************移动端-触摸点个数-END**********************/
    }

    /*
     * https://www.bilibili.com/video/BV12s411g7gU?p=154
     */
    public float rotateSleep = 2f;
    private void RotateView(float x, float y)
    {
        x *= rotateSleep;
        y *= rotateSleep;
        //（上下旋转）
        this.X = transform.eulerAngles.x;
        if (this.X >= 180)
        {
            this.X -= 360;
        }
        if (this.X >= 20)
        {
            this.transform.Rotate(Vector3.left);
        }
        else if (this.X <= -20)
        {
            this.transform.Rotate(Vector3.right);
        }
        else
        {
            this.transform.Rotate(-y, 0, 0);
        }
        //（左右旋转）
        this.transform.Rotate(0, x, 0, Space.World);//有问题，在斜坡转摄像机会歪头。但是下面有对此的补救措施。不要删除这句
        Vector3 v3 = this.transform.localRotation.eulerAngles;//重新获取旋转信息，以便下面单独将Z轴固定为0
        v3.z = 0;//重设Z轴为0
        this.transform.localRotation = Quaternion.Euler(v3);//再次重设，此时Z轴为0，不会出现歪头视觉！
    }

    public Camera GetCamera()
    {
        return this.camera;
    }
}

/*
        触摸检测参考链接：https://blog.csdn.net/qq_34444468/article/details/80520215
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
        }
*/