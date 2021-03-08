using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCar : MonoBehaviour
{
    [Header("相机固定视野追随")]
    [Tooltip("相机跟随车辆的插值")]
    public float deltaValue = 10f;
    private Transform target;
    private Vector3 fixedDistance;
    private Vector3 tempPos;
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;//找到标签为Player的目标物体
        fixedDistance = new Vector3(0, 1.88f, -3.52f);//相机处于赛车背后的初始位置，需要进行测试
    }

    private void Update()
    {
        //transformDirection（new Vector3）可以将Vector3的坐标转换为世界坐标；
        tempPos = target.TransformDirection(fixedDistance) + target.position;
        transform.position = Vector3.Lerp(transform.position, tempPos, Time.fixedDeltaTime * deltaValue);
        transform.LookAt(target);//使相机自身z坐标对齐物体的自身z坐标（注意不是世界坐标）
    }
}
