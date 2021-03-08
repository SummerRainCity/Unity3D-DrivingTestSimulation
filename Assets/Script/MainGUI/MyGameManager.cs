using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 【游戏暂停、游戏继续、游戏重开】
/// </summary>
public class MyGameManager : MonoBehaviour
{
    [Tooltip("游戏结束标记，默认情况下，游戏继续。")]
    public bool GamePlayingFlag = true;
    [Tooltip("游戏暂停标记，默认情况下，游戏没有暂停。")]
    public bool GamePauseFlag = false;
    [Tooltip("游戏回访标记，默认用户没有点击回访")]
    [SerializeField]
    private bool RecFlag = false;

    public GameObject playerCarModel;

    #region 游戏状态的改变（继续、重开、退出）
    [Header("需要UI中的GameState游戏对象")]
    public GameObject gameState;
    [Header("游戏结算（画布）-就是游戏结算页面")]
    public GameObject GameSettlement;
    [Header("赢的墙-用于区别是“选择模式”“默认模式”")]
    public Transform WinWalls;
    [Header("在游戏结束时，所有需要隐藏的GUI")]
    public GameObject[] GUI_hide;

    private void Start()
    {
        SelectPass sp = FindObjectOfType<SelectPass>();
        if (WinWalls && sp)
        {
            string eunm_str = sp.sites.ToString();
            if (sp.sites == SelectPass.Sites.Default)//如果是“默认模式”，也就是全图考试
            {
                for (int i = 0; i < WinWalls.childCount; i++)
                {
                    Transform tf = WinWalls.GetChild(i);
                    if (!tf.name.StartsWith("Qxxs"))
                    {
                        tf.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                //如果是“选择模式”，到指定地点进行考试
                //根据所选择项目找到相应的项目起始位置
                GameObject pointObj = GameObject.Find(eunm_str);
                Transform spoint = pointObj.transform;
                playerCarModel.transform.position = spoint.position;
                playerCarModel.transform.rotation = spoint.rotation;
            }
        }
    }

    /// <summary>
    /// 重新开始游戏
    /// </summary>
    public void Restart()
    {
        gameState.SetActive(false);
        Scene scene = SceneManager.GetActiveScene();//获取当前场景名

        //无论什么情况，让游戏继续，方便加载界面显示加载动画，否则会出现动画不动的BUG
        //code...，已经在加载代码中处理了。

        Globe.loadM = Globe.LoadMode.Default;
        Globe.nextSceneName = scene.name;
        SceneManager.LoadScene("Loading");
    }

    /// <summary>
    /// 退出游戏到主界面
    /// </summary>
    public void EscQuit()
    {
        //改变项目点为Default（防止用户在点击“考试模式”“Exam Model”时进入的项目是上一次指定的项目）
        FindObjectOfType<SelectPass>().sites = SelectPass.Sites.Default;

        //无论什么情况，让游戏继续，方便加载界面显示加载动画，否则会出现动画不动的BUG
        GamePlayingFlag = true;
        GameContinue();

        gameState.SetActive(false);
        Destroy(gameObject);

        Globe.loadM = Globe.LoadMode.Fast;
        Globe.nextSceneName = "Main Menu";
        SceneManager.LoadScene("Loading");
    }
    #endregion

    bool first = true;
    /// <summary>
    /// 注意，这里想要在暂停游戏后再按下ESC回到游戏。
    /// 那么你不应该使用FixedUpdate函数，因为此函数会受到timeScale影响。
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //显示、关闭游戏进度管理界面
            if(GamePauseFlag == true)
                gameState.SetActive(true);
            else
                gameState.SetActive(false);
        }

        //如果用户分数低于80分 && 用户还在游戏中...
        if (Car_Information.GetScore() < 80 && GamePlayingFlag)
        {
            GamePause();
            GamePlayingFlag = false;//游戏结束标记

            GameSettlement.SetActive(!GameSettlement.activeSelf);
            GameSettlement.GetComponentInChildren<Text>().text = "考试结束，成绩不合格！";
            GameSettlement.GetComponentInChildren<Text>().color = Color.red;
            FindObjectOfType<VoicePromptAudio>().Play_BuHeGe();
            FindObjectOfType<CarAudioSourceManager>().StopAllAudio();

            //隐藏控制组件
            for (int i = 0; i < GUI_hide.Length; i++)
            {
                if (GUI_hide[i])
                {
                    Destroy(GUI_hide[i]);
                }
            }
        }
        //如果用户分数大于80分 && 游戏已经结束
        else if (Car_Information.GetScore() >= 80 && !GamePlayingFlag && first)
        {
            first = false;
            GamePause();
            GameSettlement.GetComponentInChildren<Text>().text = "考试结束，成绩合格！";
            GameSettlement.GetComponentInChildren<Text>().color = Color.green;
            GameSettlement.SetActive(!GameSettlement.activeSelf);
            FindObjectOfType<VoicePromptAudio>().Play_HeGe();

            //隐藏控制组件
            for (int i = 0; i < GUI_hide.Length; i++)
            {
                if (GUI_hide[i])
                {
                    Destroy(GUI_hide[i]);
                }
            }
        }
    }

    public void SettingGamePause()
    {
        gameState.SetActive(true);
        GamePause();
    }
    public void SettingGameContinue()
    {
        gameState.SetActive(false);
        GameContinue();
    }

    /// <summary>
    /// 游戏暂停（此类使用）
    /// </summary>
    public void GamePause()
    {
        GamePauseFlag = true;
        Time.timeScale = 0;
    }

    /// <summary>
    /// 游戏继续（仅此类使用）
    /// 参数GamePlayingFlag-注意1：游戏结算后，不能继续游戏，这只用于此类中Update函数中的实时判断，以
    /// 来确定游戏是否结束。
    /// 参数RecFlag-注意2：游戏结束后，此类GamePlayingFlag确保Update中的检测只执行一次，但是如果用户需
    /// 要回访游戏，那么RecFlag应为true，默认为false。
    /// </summary>
    public void GameContinue()
    {
        if (GamePlayingFlag || RecFlag)
        {
            GamePauseFlag = false;
            Time.timeScale = 1;
        }
    }

    public void SetRecFlag(bool flag)
    {
        RecFlag = flag;
    }

    /// <summary>
    /// 脚本销毁
    /// </summary>
    private void OnDestroy()
    {
        StopAllCoroutines();//终之所有协程
    }
}



















//在Android上进行数据写入
/*
StreamWriter sw = new StreamWriter("/sdcard/" + "/APP信息.txt");
sw.WriteLine("游戏名：模拟驾考二");
sw.WriteLine("版本：1.0");
sw.WriteLine("作者：夏雨程");
sw.Flush();
sw.Close();
*/