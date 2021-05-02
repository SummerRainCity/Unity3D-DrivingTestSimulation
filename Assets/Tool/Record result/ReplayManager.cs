/* IN-GAME REPLAY - @madebyfeno - <feno@ironequal.com>
 * You can use it in commercial projects (and non-commercial project of course), modify it and share it.
 * Do not resell the resources of this project as-is or even modified. 
 * TL;DR: Do what the fuck you want but don't re-sell it
 * 
 * ironequal.com
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace Replay
{
    public class ReplayManager : MonoBehaviour
    {
        public int recordRate = 30;//源代码数值：120
        public bool isRecording = false;
        public bool isPlaying = false;
        [Tooltip("当前实例对象（自动赋值）")]
        public static ReplayManager Singleton;
        public Action<float> OnReplayTimeChange;
        public Action OnReplayStart;

        private bool wasPlaying = true;

        private bool replayReplayAvailable = false;

        #region UI

        public Slider _slide;
        public Image _play;
        public Image _replay;
        public Image _pause;
        public Text _timestamp;
        [Tooltip("回放的界面")]
        public GameObject _replayCanvas;
        [Tooltip("汽车操控的界面")]
        public CanvasGroup ControlsGroup;
        [Tooltip("仅用于紧急停车")]
        private DrivingKernel dp;
        [Tooltip("REC按钮只能按下一次")]
        public Button buttonRec;
        [Header("在回放时，所有需要隐藏的GUI")]
        public GameObject[] GUI_hide;

        #endregion

        #region Time

        private float _startTime;
        private float _endTime;

        #endregion

        void Awake()
        {
            if (ReplayManager.Singleton == null)
            {
                ReplayManager.Singleton = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // Use this for initialization
        void Start()
        {
            // This line call the replay to start after 3 seconds. You can remove this line and call StartReplay when you want.
            //在5秒后，调用函数StartReplay，需要注意，程序一运行就开始记录，5f表示记录5秒。
            //Invoke("StartReplay", 5f);//这个StartReplay函数执行意味着不在记录，应当由用户自己控制。

            dp = FindObjectOfType<DrivingKernel>();
            ControlsGroup.alpha = 0;//记录时隐藏进度条
            _slide.interactable = false;//录屏时这个滚动条不能拖动
            isRecording = true;
            _startTime = Time.time;

            _slide = _replayCanvas.GetComponentInChildren<Slider>();

            _play.GetComponent<Button>().onClick.AddListener(() => Play());
            _pause.GetComponent<Button>().onClick.AddListener(() => Pause());
            _replay.GetComponent<Button>().onClick.AddListener(() => ReplayReplay());
            _slide.GetComponent<Slider>().onValueChanged.AddListener((Single v) => SetCursor(v));

            EventTrigger trigger = _slide.GetComponent<EventTrigger>();
            {
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerDown;
                entry.callback.AddListener((eventData) =>
                {
                    wasPlaying = isPlaying;
                    Pause();
                });
                trigger.triggers.Add(entry);
            }
            {
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerUp;
                entry.callback.AddListener((eventData) =>
                {
                    if (wasPlaying)
                        Play();
                });
                trigger.triggers.Add(entry);
            }

            trigger = _slide.transform.parent.GetComponent<EventTrigger>();
            {
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerExit;
                entry.callback.AddListener((eventData) =>
                {
                    _slide.handleRect.transform.localScale = Vector3.zero;
                });
                trigger.triggers.Add(entry);
            }
            {
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerEnter;
                entry.callback.AddListener((eventData) =>
                {
                    _slide.handleRect.transform.localScale = Vector3.one;
                });
                trigger.triggers.Add(entry);
            }
        }

        public float GetCurrentTime()
        {
            return Time.time - _startTime;
        }

        /// <summary>
        /// 开始回放（用户调用）
        /// </summary>
		public void StartReplay()
        {
            buttonRec.interactable = false;
            //无论何种原因，取消游戏暂停
            Time.timeScale = 1;
            //停止车辆运行
            dp.Resistance();
            //取消物理检测（汽车立刻停止）
            dp.GetComponentInParent<Rigidbody>().isKinematic = true;
            //隐藏结算界面相关组件
            for (int i = 0; i < GUI_hide.Length; i++)
            {
                if (GUI_hide[i])
                {
                    GUI_hide[i].SetActive(false);
                }
            }

            ControlsGroup.alpha = 1;//开始播放的时候显示进度条
            _slide.interactable = true;//播放时滚动条能拖动
            _endTime = Time.time;
            _replayCanvas.SetActive(true);
            isPlaying = false;
            _replayCanvas.GetComponent<CanvasGroup>().alpha = 1;
            _slide.maxValue = _endTime - _startTime;
            OnReplayTimeChange(0);
            RefreshTimer();

            if (OnReplayStart != null)
            {
                // You can remove this log if you don't care
#if UNITY_EDITOR
                Debug.Log("There's " + OnReplayStart.GetInvocationList().Length + " objects affected by the replay.");
#endif
                OnReplayStart();
            }
        }
       
        // Update is called once per frame
        void Update()
        {
            if (isPlaying)
            {
                _slide.value += Time.deltaTime * Time.timeScale;

                OnReplayTimeChange(_slide.value);
            }
            // You can remove/modify this if you use Space for something else
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isPlaying)
                {
                    Pause();
                }
                else
                {
                    Play();
                }
            }
            // ------
        }

        public void Play()
        {
            _slide.Select();
            if (!isPlaying && _slide.value != _endTime - _startTime)
            {
                isPlaying = true;

                Swap(_play.gameObject, _pause.gameObject);

                if (_play.transform.GetSiblingIndex() > _pause.transform.GetSiblingIndex())
                {
                    _play.transform.SetSiblingIndex(_pause.transform.GetSiblingIndex());
                }
            }
        }

        void Swap(GameObject _out, GameObject _in = null, float delay = 0f)
        {

            if (_in != null)
            {
                _in.SetActive(true);
            }

            _out.SetActive(false);
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            _slide.Select();
            if (isPlaying)
            {
                isPlaying = false;

                Swap(_pause.gameObject, _play.gameObject);

                if (_pause.transform.GetSiblingIndex() > _play.transform.GetSiblingIndex())
                {
                    _pause.transform.SetSiblingIndex(_play.transform.GetSiblingIndex());
                }
            }
        }

        /// <summary>
        /// 开始回放
        /// </summary>
        public void ReplayReplay()
        {
            _slide.value = 0;
            replayReplayAvailable = false;
            Swap(_replay.gameObject);
            Play();
        }

        public void SetCursor(Single value)
        {
            RefreshTimer();

            if (replayReplayAvailable)
            {
                replayReplayAvailable = false;
                Swap(_replay.gameObject, _play.gameObject);
            }

            if (_slide.value == _endTime - _startTime)
            {
                Pause();

                replayReplayAvailable = true;
                Swap(_play.gameObject, _replay.gameObject, .2f);
            }

            if (OnReplayTimeChange != null)
            {
                OnReplayTimeChange(value + _startTime);
            }
        }

        void RefreshTimer()
        {
            float current = _slide.value;
            float total = (_endTime - _startTime);

            string currentMinutes = Mathf.Floor(current / 60).ToString("00");
            string currentSeconds = (current % 60).ToString("00");

            string totalMinutes = Mathf.Floor(total / 60).ToString("00");
            string totalSeconds = (total % 60).ToString("00");

            _timestamp.text = currentMinutes + ":" + currentSeconds + " / " + totalMinutes + ":" + totalSeconds;
        }

#if UNITY_EDITOR
        void OnDestroy()
        {
            Debug.LogWarning(gameObject.name + " destroyed.");
        }
#endif
    }
}
