using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

/// <summary>
/// 玩家选择的关卡，默认全部挑战。
/// </summary>
public class SelectPass : MonoBehaviour
{
    private static SelectPass obj = null;
    public enum Sites
    {
        Default,Dcrk,Cftc,Bpqb,Zjzw,Qxxs
    }
    public Sites sites;
    private void Start()
    {
        if(obj == null)
        {
            obj = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}