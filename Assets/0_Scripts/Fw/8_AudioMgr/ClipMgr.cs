using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FW {
    public class ClipMgr {
        string[] m_clipName; // 所有音效的名字
        List<SingleClip> m_allSingleClip = new List<SingleClip> ();

        public ClipMgr () {
            // ReadConfig ();
            // LoadClips ();
            InitData ();
        }

        //---------------------------------根据名字找音源片段----------------------------
        /// <summary>
        /// 更具名字找到SingleClip
        /// </summary>
        /// <param name="_clipName">音效名字</param>
        /// <returns>返回SingleClip</returns>
        public SingleClip FindClipByName (string _clipName) {
            // int tempIndex = -1;
            for (int i = 0, j = m_clipName.Length; i < j; ++i) {
                if (m_clipName[i].Equals (_clipName)) //判断两个字符串相等
                    return m_allSingleClip[i];
            }
            return null;
        }
        //=============================================================================

        //----------------------------配置文件读取 加载音效到内存-------------------------
        void InitData () {
            var dataList = FW.DataMgr.Instance.GetAudioData();
            var count = dataList.Count;
            m_clipName = new string[count];
            int cnt = 0;
            foreach (var item in dataList) {
                m_clipName[cnt++] = item.Value.Name;
                var clip = FW.ResMgr.Instance.Load<AudioClip> (item.Value.Name);
                SingleClip sc = new SingleClip (clip);
                m_allSingleClip.Add (sc);
            }
        }
        //=============================================================================
    }
}