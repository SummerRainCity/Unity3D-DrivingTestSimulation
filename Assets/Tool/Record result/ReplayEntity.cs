/* IN-GAME REPLAY - @madebyfeno - <feno@ironequal.com>
 * You can use it in commercial projects (and non-commercial project of course), modify it and share it.
 * Do not resell the resources of this project as-is or even modified. 
 * TL;DR: Do what the fuck you want but don't re-sell it
 * 
 * ironequal.com
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.AI;

namespace Replay
{
    #region (3)-记录物体位置、缩放两个信息（类）
    [Serializable]
    public class TimelinedVector3
    {
        public AnimationCurve x;
        public AnimationCurve y;
        public AnimationCurve z;

        public void Add(Vector3 v)
        {
            float time = ReplayManager.Singleton.GetCurrentTime();
            x.AddKey(time, v.x);
            y.AddKey(time, v.y);
            z.AddKey(time, v.z);
        }

        public Vector3 Get(float _time)
        {
            return new Vector3(x.Evaluate(_time), y.Evaluate(_time), z.Evaluate(_time));
        }
    }
    #endregion

    #region (2)-四元组记录物体旋转信息（类）
    [Serializable]
    public class TimelinedQuaternion
    {
        public AnimationCurve x;
        public AnimationCurve y;
        public AnimationCurve z;
        public AnimationCurve w;

        public void Add(Quaternion v)
        {
            float time = ReplayManager.Singleton.GetCurrentTime();
            x.AddKey(time, v.x);
            y.AddKey(time, v.y);
            z.AddKey(time, v.z);
            w.AddKey(time, v.w);
        }

        public Quaternion Get(float _time)
        {
            return new Quaternion(x.Evaluate(_time), y.Evaluate(_time), z.Evaluate(_time), w.Evaluate(_time));
        }
    }
    #endregion

    #region (1)-对物体旋转、缩放、位置等信息进行综合操作（类）
    [Serializable]
    public class RecordData
    {
        //记录位置
        public TimelinedVector3 position;
        //记录旋转角度
        public TimelinedQuaternion rotation;
        //记录缩放
        public TimelinedVector3 scale;

        public void Add(Transform t)
        {
            position.Add(t.position);
            rotation.Add(t.rotation);
            scale.Add(t.localScale);
        }

        public void Set(float _time, Transform _transform)
        {
            _transform.position = position.Get(_time);
            _transform.rotation = rotation.Get(_time);
            _transform.localScale = scale.Get(_time);
        }
    }
    #endregion

    #region (0)-挂载到需要记录的物体上
    public class ReplayEntity : MonoBehaviour
    {
        public RecordData data = new RecordData();

        private Rigidbody _rigidbody;
        private NavMeshAgent agent;
        private Animator animator;

        protected virtual void Start()
        {
            //开始录屏
            StartCoroutine(Recording());

            ReplayManager.Singleton.OnReplayTimeChange += Replay;
            ReplayManager.Singleton.OnReplayStart += OnReplayStart;

            _rigidbody = GetComponent<Rigidbody>();
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
        }

        IEnumerator Recording()
        {
            while (true)
            {
                yield return new WaitForSeconds(1 / ReplayManager.Singleton.recordRate);
                if (ReplayManager.Singleton.isRecording)
                {
                    data.Add(transform);
                }
            }
        }

        public void OnReplayStart()
        {
            if (_rigidbody != null)
                _rigidbody.isKinematic = true;

            if (agent)
                agent.enabled = false;

            if (animator)
                animator.enabled = false;
        }

        public void Replay(float t)
        {
            data.Set(t, transform);
        }
    }
    #endregion
}
