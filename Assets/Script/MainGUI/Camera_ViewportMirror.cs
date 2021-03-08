using UnityEngine;
using UnityEngine.UI;

public class Camera_ViewportMirror : MonoBehaviour
{
    [SerializeField]
    [Tooltip("所要控制的对象")]
    private GameObject ControlObject;

    [SerializeField]
    [Tooltip("当前获取")]
    private Toggle toggle;

    void Start()
    {
        toggle = GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(delegate (bool isOn)
        {
            ControlObject.SetActive(isOn);
        });
    }

    public void OnToggleClick()
    {
    }
}