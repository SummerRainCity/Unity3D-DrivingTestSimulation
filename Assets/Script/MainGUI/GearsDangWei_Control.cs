using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 档位控制类
/// </summary>
public class GearsDangWei_Control : MonoBehaviour
{
    [Tooltip("四个档位按钮")]
    public GameObject GearLever;
    [Tooltip("四个档位按钮")]
    public Transform P_point, R_point, N_point, D_point;
    [Tooltip("四个档位字体text")]
    public Text P_text, R_text, N_text, D_text;
    public iTween.EaseType type;
    [Tooltip("档位回档速度")]
    public float Sleep = 1;

    public enum currentGears
    {
        Default,P, R, N, D
    }
    public currentGears gears = currentGears.Default;
    private bool flag = false;//防止重复点第二次

    private void Start()
    {
        P_text.color = Color.green;//默认P档绿色
    }

    public void P()
    {
        if (flag)//如果已经是P档
        {
            return;
        }
        else
        {
            gears = currentGears.P;
            flag = true;
        }
        iTween.MoveTo(GearLever, iTween.Hash(
            "position", P_point.position,
            "time", Sleep,
            "easetype", type,
            "oncomplete", "Fun",//Fun回调函数名字
            "oncompletetarget", gameObject
        ));
    }
    public void R()
    {
        if (flag)
        {
            return;
        }
        else
        {
            gears = currentGears.R;
            flag = true;
        }
        iTween.MoveTo(GearLever, iTween.Hash(
            "position", R_point.position,
            "time", Sleep,
            "easetype", type,
            "oncomplete", "Fun",
            "oncompletetarget", gameObject
        ));
    }
    public void N()
    {
        if (flag)
        {
            return;
        }
        else
        {
            gears = currentGears.N;
            flag = true;
        }
        iTween.MoveTo(GearLever, iTween.Hash(
            "position", N_point.position,
            "time", Sleep,
            "easetype", type,
            "oncomplete", "Fun",
            "oncompletetarget", gameObject
        ));
    }
    public void D()
    {
        if (flag)
        {
            return;
        }
        else
        {
            gears = currentGears.D;
            flag = true;
        }
        iTween.MoveTo(GearLever, iTween.Hash(
            "position", D_point.position,
            "time", Sleep,
            "easetype", type,
            "oncomplete", "Fun",
            "oncompletetarget", gameObject
        ));
    }

    private void Fun()
    {
        SetTextColor();
        flag = false;
    }

    private void SetTextColor()
    {
        P_text.color = Color.white;
        D_text.color = Color.white;
        R_text.color = Color.white;
        N_text.color = Color.white;
        switch (gears)
        {
            case currentGears.P:
                P_text.color = Color.green;
                break;
            case currentGears.D:
                D_text.color = Color.green;
                break;
            case currentGears.R:
                R_text.color = Color.green;
                break;
            case currentGears.N:
                N_text.color = Color.green;
                break;
        }
    }
}
