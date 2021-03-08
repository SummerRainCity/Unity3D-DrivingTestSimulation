using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 汽车碰撞音频类
/// </summary>
public class VoicePromptAudio : MonoBehaviour
{
    private void Start()
    {
        audio_ = GetComponent<AudioSource>();

        //考试开始准备语音提示
        StartCoroutine(Setout());
    }

    #region 碰撞相关音频
    AudioSource audio_;
    [Header("碰撞音频")]
    public AudioClip[] audioCollClip;

    /*
     * 车辆只要撞到什么东西就播放碰撞音效，需要注意的是需要排除“碰撞检测”
     */
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "detection")
        {
            audio_.clip = audioCollClip[4];
            audio_.loop = false;
            audio_.volume = 1.0f;
            audio_.Play();
        }
        else
        {
            int index = Random.Range(0, audioCollClip.Length - 1);//参数传入(0,5)，返回范围(0,4)
            audio_.clip = audioCollClip[index];
            audio_.loop = false;
            audio_.volume = 1.0f;
            audio_.Play();
        }
    }
    #endregion

    #region 游戏提示相关音频
    [Header("游戏提示相关音频")]
    public AudioClip HeGe;
    public AudioClip BuHeGe;
    public AudioClip KaiShiDaoChe;
    public AudioClip KaoShiChaoShi;
    public AudioClip KaoShiKaiShi;
    public AudioClip QingQiBu;
    public AudioClip TingCheYiDaoWei;
    public AudioClip YaXian_10;
    public AudioClip YaXian_100;
    public AudioClip ZhongTuTingChe;
    public void Play_KaiShiDaoChe()
    {
        audio_.clip = KaiShiDaoChe;
        audio_.loop = false;
        audio_.volume = 1.0f;
        audio_.Play();
    }
    public void Play_KaoShiChaoShi()
    {
        audio_.clip = KaoShiChaoShi;
        audio_.loop = false;
        audio_.volume = 1.0f;
        audio_.Play();
    }
    public void Play_TingCheYiDaoWei()
    {
        audio_.clip = TingCheYiDaoWei;
        audio_.loop = false;
        audio_.volume = 1.0f;
        audio_.Play();
    }
    public void Play_ZhongTuTingChe()
    {
        audio_.clip = ZhongTuTingChe;
        audio_.loop = false;
        audio_.volume = 1.0f;
        audio_.Play();
    }
    public void Play_HeGe()
    {
        audio_.clip = HeGe;
        audio_.loop = false;
        audio_.volume = 1.0f;
        audio_.Play();
    }

    public void Play_BuHeGe()
    {
        audio_.clip = BuHeGe;
        audio_.loop = false;
        audio_.volume = 1.0f;
        audio_.Play();
    }
    public void Play_YaXian_100()
    {
        audio_.clip = YaXian_100;
        audio_.loop = false;
        audio_.volume = 1.0f;
        audio_.Play();
    }

    private void Play_KaoShiKaiShi()
    {   audio_.clip = KaoShiKaiShi;
        audio_.loop = false;
        audio_.volume = 1.0f;
        audio_.Play();
    }

    private void Play_QingQiBu()
    {
        audio_.clip = QingQiBu;
        audio_.loop = false;
        audio_.volume = 1.0f;
        audio_.Play();
    }

    #endregion

    IEnumerator Setout()
    {
        yield return new WaitForSeconds(0.1f);//运行到这，暂停0.1秒
        Play_KaoShiKaiShi();
        yield return new WaitForSeconds(1.64f);
        Play_QingQiBu();
        yield return new WaitForSeconds(1.0f);
        StopAllCoroutines();
    }
}
