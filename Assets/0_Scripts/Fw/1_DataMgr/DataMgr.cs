using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using FW;
using LitJson;
using UnityEngine;
using UnityEngine.Events;

namespace FW {
    public class DataMgr : SingletonBase<DataMgr> {

        // 所有配置文件的名字
        List<string> m_allConfigFileName = new List<string> ();

        private Dictionary<string, PrefabDataType> m_prefabConfigDic;
        private Dictionary<string, NamePathDataType> m_audioConfigDic;
        private Dictionary<string, NamePathDataType> m_spriteConfigDic;
        private Dictionary<string, Dictionary<int, RoleBattleType>> m_RoleBaseInfoDic;

        // public SettingData settingData;

        public override void Init () {

            // 所有表名加载到m_allConfigFileName 里面 
            Utility.FileOperation.ReaderConfigFileToLine ("AllConfigrationFile.txt", (line) => { m_allConfigFileName.Add (line); });
            InitData ();
        }

        void InitData () {
            for (int i = 0, j = m_allConfigFileName.Count; i < j; ++i) {
                JsonData jd = Utility.FileOperation.GetJsonData (m_allConfigFileName[i]);
                if (jd == null) continue;
                Association (m_allConfigFileName[i], jd);
            }
            // if (ArchiveMgr.Instance.DefaultArchive == null) {
            //     Debug.Log ("kkkkkkkkkkkkk");
            //     settingData = LoadJsonData<SettingData> ("Setting.json");
            // } else
            // settingData = ArchiveMgr.Instance.DefaultArchive.settingData;
        }

        // 每个表与对应的数据结构关联
        void Association (string _configFileName, JsonData _jd) {
            switch (_configFileName) {
                case "AudioCfg.txt":
                    AudioConfigAss (_jd);
                    break;
                case "PrefabCfg.txt":
                    PrefabConfigAss (_jd);
                    break;
                case "SpriteCfg.txt":
                    SpriteConfigAss (_jd);
                    break;
            }
        }

        // 获取prefab路径
        public string GetPrefabPath (string _prefabName) {
            if (m_prefabConfigDic.ContainsKey (_prefabName))
                return m_prefabConfigDic[_prefabName].Path;
            // else
            //     Debug.Log (_prefabName + " : 不在prefab字典里");
            return string.Empty;
        }

        public Dictionary<string, NamePathDataType> GetAudioData () {
            return m_audioConfigDic;
        }

        public string GetAudioPath (string _audioName) {
            if (m_audioConfigDic.ContainsKey (_audioName))
                return m_audioConfigDic[_audioName].Path;
            return string.Empty;
        }

        public string GetSpritePath (string _spriteName) {
            if (m_spriteConfigDic.ContainsKey (_spriteName)) {
                return m_spriteConfigDic[_spriteName].Path;
            }
            return string.Empty;
        }

        public T LoadJsonData<T> (string _configFileName) where T : class {
            string content = Utility.FileOperation.GetConfigFileAllContent (_configFileName);
            JsonReader jr = new JsonReader (content);
            T data = JsonMapper.ToObject<T> (jr);
            return data??data;
        }

        public void SaveJsonData (string _configFileName, object obj) {
            string path = Application.streamingAssetsPath + "/" + _configFileName + ".json";
            var json = JsonMapper.ToJson (obj);
            File.WriteAllText (path, json);
        }

        //-------------------配置表关联-----------------------
        public void PrefabConfigAss (JsonData _jd) {
            m_prefabConfigDic = new Dictionary<string, PrefabDataType> ();
            foreach (JsonData elem in _jd) {
                PrefabDataType table = new PrefabDataType ();
                table.Id = elem["Id"].ToString ();
                table.Name = elem["Name"].ToString ();
                table.Path = elem["Path"].ToString ();
                m_prefabConfigDic[table.Name] = table;
            }
        }

        public void AudioConfigAss (JsonData _jd) {
            m_audioConfigDic = new Dictionary<string, NamePathDataType> ();
            foreach (JsonData elem in _jd) {
                NamePathDataType table = new NamePathDataType ();
                table.Name = elem["Name"].ToString ();
                table.Path = elem["Path"].ToString ();
                m_audioConfigDic[table.Name] = table;
            }
        }

        public void SpriteConfigAss (JsonData _jd) {
            m_spriteConfigDic = new Dictionary<string, NamePathDataType> ();
            foreach (JsonData elem in _jd) {
                NamePathDataType table = new NamePathDataType ();
                table.Name = elem["Name"].ToString ();
                table.Path = elem["Path"].ToString ();
                m_spriteConfigDic[table.Name] = table;
            }
        }
    }
}