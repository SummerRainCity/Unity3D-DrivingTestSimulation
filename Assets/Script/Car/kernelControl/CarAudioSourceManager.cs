using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudioSourceManager : MonoBehaviour
{
    private DrivingKernel CoreDriving;

    [Header("档位与音频联系")]
    [SerializeField]
    private GearsDangWei_Control Gears;//档位不同，下面的音效会随之改变
    [Tooltip("启动、运行、停车-Audio_Runing")]
    private AudioSource Audio_Runing;
    [Header("启动、运行、停车-Clips")]
    [SerializeField]
    private AudioClip[] CarClips_Runing;

    private void Start()
    {
        this.CoreDriving = GetComponent<DrivingKernel>();
        this.Audio_Runing = GetComponent<AudioSource>();
        Gears = FindObjectOfType<GearsDangWei_Control>();
    }

    private void FixedUpdate()
    {
        PlayAudio();
    }

    bool AudioInitFlag_Engine = false, AudioInitFlag_Start = false, AudioInitFlag_Stop = false;
    private void PlayAudio()
    {
        if (Gears.gears != GearsDangWei_Control.currentGears.P && Gears.gears != GearsDangWei_Control.currentGears.Default)
        {
            AudioInitFlag_Stop = false;//可以播放熄火声音
            if (!AudioInitFlag_Start)//只能初始化一次
            {
                CoreDriving.CancelResistance();
                AudioInitFlag_Start = true;//标记不能再重复初始化
                StartCoroutine(AudioInit_Start());
            }

            //当前引擎速度
            if (CoreDriving.speed >= 1f)
            {
                if (!AudioInitFlag_Engine)//只能初始化一次
                {
                    AudioInitFlag_Engine = true;
                    StartCoroutine(AudioInit_Enging());
                }
                //音频属性设定
                Audio_Runing.pitch = 0.4f + CoreDriving.speed / 200;//以最大值200km/h为基准进行逐级调整音效。
            }
        }
        else if (Gears.gears == GearsDangWei_Control.currentGears.P)
        {
            if (!AudioInitFlag_Stop)//只能初始化一次
            {
                CoreDriving.Resistance();
                AudioInitFlag_Stop = true;
                StartCoroutine(AudioInit_Stop());
            }
            AudioInitFlag_Start = false;
            AudioInitFlag_Engine = false;
        }
    }

    IEnumerator AudioInit_Start()
    {
        Audio_Runing.clip = CarClips_Runing[0];
        Audio_Runing.volume = 0.3f;//音量
        Audio_Runing.mute = false;//非静音
        Audio_Runing.pitch = 1f;
        Audio_Runing.loop = false;
        Audio_Runing.Play();

        yield return new WaitForSeconds(2.623f);//运行到这，暂停t秒

        AudioInitFlag_Engine = true;
        StartCoroutine(AudioInit_Enging());

        yield return 0;
    }
    IEnumerator AudioInit_Enging()
    {
        Audio_Runing.clip = CarClips_Runing[1];
        Audio_Runing.volume = 1f;//音量
        Audio_Runing.mute = false;//非静音
        Audio_Runing.loop = true;//循环播放
        Audio_Runing.pitch = 0.4f;
        Audio_Runing.Play();//开始播放

        yield return 0;
    }

    IEnumerator AudioInit_Stop()
    {
        Audio_Runing.clip = CarClips_Runing[2];
        Audio_Runing.volume = 1f;//音量
        Audio_Runing.mute = false;//非静音
        Audio_Runing.pitch = 1f;
        Audio_Runing.loop = false;
        Audio_Runing.Play();

        yield return 0;
    }

    public void StopAllAudio()
    {
        Audio_Runing.Stop();
    }
}
