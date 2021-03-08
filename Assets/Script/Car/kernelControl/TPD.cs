using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 【第三人称】相机注视并跟随角色（放大、缩小、自由旋转）
/// 参考文献：https://blog.csdn.net/ff_0528/article/details/102777237
/// </summary>
public class TPD : MonoBehaviour
{
    private Transform target;
    public float x_Speed = 100;
    public float y_Speed = 100;
    public float mmSpeed = 10;
    public float xMinLimit = 5;
    public float xMaxLimit = 80;
    [Tooltip("初始视角距离")]
    public float distance = 5;
    [Tooltip("最小放大距离")]
    public float minDistance = 3.0f;
    [Tooltip("最大放大距离")]
    public float maxDistance = 10;
    public bool isNeedDamping = true;
    public float damping = 8f;
    public float x_OriginAngle = 30f;
    public float y_OriginAngle = 0f;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;//找到标签为Player的目标物体
        //target = FindObjectOfType<DrivingKernel>().transform;
    }

    private void Update()
    {
        if (target)
        {
            /**********************************PC端*****************************
            y_OriginAngle += Input.GetAxis("Mouse X") * x_Speed * Time.deltaTime;
            x_OriginAngle -= Input.GetAxis("Mouse Y") * y_Speed * Time.deltaTime;
            /********************************************************************/

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
                            y_OriginAngle += Input.GetAxis("Mouse X") * x_Speed * Time.deltaTime;
                            x_OriginAngle -= Input.GetAxis("Mouse Y") * y_Speed * Time.deltaTime;
                        }
                    }
                }
            }
            x_OriginAngle = ClampAngle(x_OriginAngle, xMinLimit, xMaxLimit);
            distance -= Input.GetAxis("Mouse ScrollWheel") * mmSpeed;
            distance = Mathf.Clamp(distance, minDistance, maxDistance);
            Quaternion rotation = Quaternion.Euler(x_OriginAngle, y_OriginAngle, 0.0f);
            Vector3 disVector = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * disVector + target.position;
            if (isNeedDamping)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * damping);
                transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * damping);
            }
            else
            {
                transform.rotation = rotation;
                transform.position = position;
            }
        }
    }

    static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
