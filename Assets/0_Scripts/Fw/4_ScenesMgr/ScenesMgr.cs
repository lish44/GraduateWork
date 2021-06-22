using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace FW {
    public class ScenesMgr : SingletonBase<ScenesMgr> {

        public void LoadScene (string _sceneName, UnityAction _callback) {
            SceneManager.LoadScene (_sceneName);
            _callback ();
        }

        public void LoadSceneAsyn (string _sceneName, UnityAction _callback) {
            MonoMgr.Instance.StartCoroutine (ReallyLoadSceneAsyn (_sceneName, _callback));
        }

        private IEnumerator ReallyLoadSceneAsyn (string _sceneName, UnityAction _callback) {
            AsyncOperation ao = SceneManager.LoadSceneAsync (_sceneName);
            //更新进度条
            float ProgressValue;
            ao.allowSceneActivation = false;
            FW.Evencenter.Instance.AddEventListener<bool> (EventName.LOADINGFINISH, (b) => {
                ao.allowSceneActivation = b;
            });

            while (!ao.isDone) {
                if (ao.progress < 0.9f) {
                    ProgressValue = ao.progress;
                } else {
                    ProgressValue = 1.0f;
                }
                FW.Evencenter.Instance.EventTrigger<float> (EventName.LOADING, ProgressValue);
                yield return null;
            }
            ProgressValue = 0;
            // Debug.Log("GC清理一次");
            System.GC.Collect ();
            FW.AudioMgr.Instance.ReleaseFreeAudio ();
            _callback ();
        }
    }
}