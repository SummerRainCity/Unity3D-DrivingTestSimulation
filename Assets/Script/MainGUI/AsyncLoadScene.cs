using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 游戏场景加载管理类
/// 注意事项：在加载过程中，不能是“游戏暂停”状态，否则会出现加载卡主的现象！例如
/// 你可以把时间缩放改成1：Time.timeScale = 1
/// </summary>
public class AsyncLoadScene : MonoBehaviour
{
    public Text loadingText;
    public Image progressBar;

    private int curProgressValue = 0;//Max=100，到达最大值就加载
    private AsyncOperation operation;

    private void Start()
    {
        //上述注意事项已经说明！
        Time.timeScale = 1;

        if (SceneManager.GetActiveScene().name == "Loading")
        {
            //启动协程
            StartCoroutine(AsyncLoading());

            switch (Globe.loadM)
            {
                case Globe.LoadMode.Default:
                    /*
                     InvokeRepeating函数参数：
                    参数1：函数名
                    参数2：先延时多少秒后执行
                    参数3：重复执行间隔
                     */
                    InvokeRepeating(nameof(ChangeProgressValue), 0, 0.005f);//0.5s
                    break;
                case Globe.LoadMode.Fast:
                    InvokeRepeating(nameof(ChangeProgressValue), 0, 0.001f);//0.1s
                    break;
                case Globe.LoadMode.Slow:
                    InvokeRepeating(nameof(ChangeProgressValue), 0.2f, 0.03f);//3s
                    break;
            }
        }
    }

    IEnumerator AsyncLoading()
    {
        operation = SceneManager.LoadSceneAsync(Globe.nextSceneName);
        //阻止当加载完成自动切换
        operation.allowSceneActivation = false;
        yield return operation;
    }

    private void ChangeProgressValue()
    {
        if (curProgressValue >= 100)
        {
            loadingText.text = "加载完成";//文本显示完成OK  
            CancelInvoke();//取消所有计时
            operation.allowSceneActivation = true;//启用自动加载场景 
            Destroy(gameObject);
        }
        curProgressValue++;
    }

    private void Update()
    {
        loadingText.text = curProgressValue + "%";//实时更新进度百分比的文本显示  
        progressBar.fillAmount = curProgressValue / 100f;//实时更新滑动进度图片的fillAmount值  
    }
}

public class Globe
{
    //加载的场景名
    public static string nextSceneName;
    //加载模式
    public enum LoadMode
    {
        Fast,
        Slow,
        Default
    }
    public static LoadMode loadM = LoadMode.Default;
}