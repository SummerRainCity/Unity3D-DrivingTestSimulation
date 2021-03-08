using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 【镜头切换】
/// </summary>
public class CameraSwitch : MonoBehaviour
{
    public GameObject[] _camera;
    private int i = 0;

    private void Start()
    {
        _camera[0].SetActive(true);
    }

    private void FixedUpdate()
    {
        /*********************************PC端****************************
        //按下V键-切换视觉
        if (Input.GetKeyDown(KeyCode.V))
        {
            _camera[i % _camera.Length].SetActive(false);
            i++;
            _camera[i % _camera.Length].SetActive(true);
        }
        //按下C键盘，隐藏或显是光标
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (pointView)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = pointView;
                pointView = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = pointView;
                pointView = true;
            }
        }
        /***********************************************************************/
    }

    /// <summary>
    /// 移动端相机切换
    /// </summary>
    public void MobileTerminal_SwitchCamera()
    {
        _camera[i % _camera.Length].SetActive(false);
        i++;
        _camera[i % _camera.Length].SetActive(true);
    }
}