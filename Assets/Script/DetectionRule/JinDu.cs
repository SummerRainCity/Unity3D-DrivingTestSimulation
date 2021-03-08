using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 汽车到达一个项目时播放该项目的名称音频
/// </summary>
public class JinDu : MonoBehaviour
{
    [Header("播放下一项目的项目名称")]
    public AudioClip clip;

    [Header("下一个项目对象，在当前项目结束后自动激活")]
    public GameObject NextExamProjectObject;

    private void Start()
    {
        //默认下一项目是关闭的
        if(NextExamProjectObject != null)
            NextExamProjectObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (NextExamProjectObject != null)
                NextExamProjectObject.SetActive(true);

            AudioSource audio = other.GetComponent<AudioSource>();
            audio.clip = clip;
            audio.Play();
        }
    }
}
