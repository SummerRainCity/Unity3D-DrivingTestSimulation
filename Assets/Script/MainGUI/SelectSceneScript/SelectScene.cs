using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

public class SelectScene : MonoBehaviour
{
    /// <summary>
    /// 主界面
    /// </summary>
    public void Scene_Main()
    {
        //SceneManager.LoadScene("UGUI_Start");
        Globe.loadM = Globe.LoadMode.Fast;
        Globe.nextSceneName = "Main Menu";
        SceneManager.LoadScene("Loading");
    }

    /// <summary>
    /// 考试模式
    /// </summary>
    public void Scene_ExamMode()
    {
        Globe.loadM = Globe.LoadMode.Slow;
        Globe.nextSceneName = "Exam Mode";
        SceneManager.LoadScene("Loading");
    }

    /// <summary>
    /// 免费模式
    /// </summary>
    public void Scene_FreeMode()
    {
        //SceneManager.LoadScene("Free Mode");
        Globe.loadM = Globe.LoadMode.Slow;
        Globe.nextSceneName = "Free Mode";
        SceneManager.LoadScene("Loading");
    }

    /// <summary>
    /// 标准场地模式
    /// </summary>
    public void Scene_StandardMode()
    {
        Globe.loadM = Globe.LoadMode.Slow;
        Globe.nextSceneName = "Standard Mode";
        SceneManager.LoadScene("Loading");
    }

    /// <summary>
    /// 教程
    /// </summary>
    public void Scene_Tutorial()
    {
        Globe.loadM = Globe.LoadMode.Fast;
        Globe.nextSceneName = "Tutorial";
        SceneManager.LoadScene("Loading");
    }

    /// <summary>
    /// 移动端-退出游戏
    /// </summary>
    public void Exit_Game()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
