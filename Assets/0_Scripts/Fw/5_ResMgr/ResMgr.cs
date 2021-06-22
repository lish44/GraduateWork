using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FW {
    public class ResMgr : SingletonBase<ResMgr> {

        string GetPath (string _name) {
            var path = string.Empty;
            // 先找prefab表
            if (path == string.Empty)
                path = FW.DataMgr.Instance.GetPrefabPath (_name);
            // 再找audio表
            if (path == string.Empty)
                path = FW.DataMgr.Instance.GetAudioPath (_name);
            if (path == string.Empty)
                path = FW.DataMgr.Instance.GetSpritePath (_name);

            return path;
        }

        //同步
        public T Load<T> (string _name) where T : UnityEngine.Object {
            var path = GetPath (_name);
            T res = Resources.Load<T> (path);
            if (res is GameObject) {
                var go = GameObject.Instantiate (res);
                go.name = go.name.Replace ("(Clone)", "");
                return go;
            }
            return res;
        }

        public T[] LoadAll<T> (string _name) where T : UnityEngine.Object {
            var path = GetPath (_name);
            T[] res = Resources.LoadAll<T> (path);
            if (res == null) return res;
            for (int i = 0; i < res.Length; ++i) {
                if (res[i] is GameObject) {
                    res[i] = GameObject.Instantiate (res[i]);
                    res[i].name = res[i].name.Replace ("(Clone)", "");
                } else {
                    return res;
                }
            }
            return res;
        }

        //异步
        public void LoadAsync<T> (string _name, UnityAction<T> _callback) where T : UnityEngine.Object {
            FW.MonoMgr.Instance.StartCoroutine (ReallyLoadAsync (_name, _callback));
        }

        private IEnumerator ReallyLoadAsync<T> (string _name, UnityAction<T> _callback) where T : UnityEngine.Object {
            var path = GetPath (_name);
            ResourceRequest r = Resources.LoadAsync<T> (path);
            yield return r;

            if (r.asset is GameObject) {
                var go = GameObject.Instantiate (r.asset) as T;
                go.name = go.name.Replace ("(Clone)", "");
                _callback (go);
            } else
                _callback (r.asset as T);

        }
    }
}