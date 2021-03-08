using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 请写简介
/// </summary>
public class UI_CarSpeed : MonoBehaviour
{
    [Header("汽车动力核心类")]
    [SerializeField]
    private DrivingKernel CoreDriving;

    private Text text;

    private void Start()
    {
        this.text = GetComponent<Text>();
        CoreDriving = FindObjectOfType<DrivingKernel>();
    }
	void FixedUpdate() 
	{
        text.text = CoreDriving.speed.ToString() + " KM/H";
	}
}
