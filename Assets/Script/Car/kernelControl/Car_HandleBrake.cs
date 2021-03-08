using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 手刹系统
/// 时间：2021-2-18-17点31分
/// </summary>
public class Car_HandleBrake : MonoBehaviour
{
    [Header("手刹拉起-标志")]
    public bool Activate;

    [Header("手刹拉起-图片(自动赋值)")]
    [SerializeField]
    private Sprite image_on;
    [Header("手刹放下-图片(自动赋值)")]
    [SerializeField]
    private Sprite image_off;
    [Header("音频相关")]
    [SerializeField]
    private AudioSource _audio;
    [SerializeField]
    private AudioClip audio_on, audio_off;

    private Image ImageMain;

    private void Start()
    {
        ImageMain = GetComponent<Image>();

        image_on = Resources.Load("Image/HandleBrakeOn", typeof(Sprite)) as Sprite;
        image_off = Resources.Load("Image/HandleBrakeOff", typeof(Sprite)) as Sprite;

        if (ImageMain != null)
        {
            ImageMain.sprite = image_on;
            Activate = true;
        }

        _audio = GetComponent<AudioSource>();
    }

    public void ChangeState()
    {
        if (Activate)
        {
            _audio.clip = audio_off;
            _audio.Play();
            Activate = false;
            ImageMain.sprite = image_off;
        }
        else
        {
            _audio.clip = audio_on;
            _audio.Play();
            Activate = true;
            ImageMain.sprite = image_on;
        }
    }
}
