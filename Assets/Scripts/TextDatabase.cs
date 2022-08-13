using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
using Sirenix.OdinInspector.Editor;
#endif

namespace _EI
{
    [ShowOdinSerializedPropertiesInInspector]
    public class TextDatabase : MonoBehaviour
    {
        static readonly string[] ENDL = { "\r\n", "\r", "\n" };

        public static string filename = "TextData/zh-tw";

        [ShowInInspector, OnCollectionChanged(after: "OnTextDataDictChanged")]
        public static Dictionary<string, TextData> textDataDict = new Dictionary<string, TextData>();


        public static string GetValue(string id)
        {
            if (string.IsNullOrEmpty(id) || !textDataDict.ContainsKey(id))
                return null;
            return textDataDict[id].value;
        }

        [ShowInInspector]
        public static void Load()
        {
            Debug.Log($"Load text data from {filename}");

            TextAsset csv = Resources.Load<TextAsset>(filename);

            if (csv == null)
                throw new Exception($"{filename} not found");

            textDataDict = new Dictionary<string, TextData>();

            string[] rows = csv.text.Split(ENDL, StringSplitOptions.RemoveEmptyEntries);

            foreach (string row in rows)
            {
                if (string.IsNullOrEmpty(row))
                    continue;
                TextData textData = new TextData(row.Split(','));
                textDataDict[textData.id] = textData;
            }
        }

#if UNITY_EDITOR
        public static void SetValue(string id, string value)
        {
            if (textDataDict.ContainsKey(id))
                textDataDict[id].value = value;
            else
                textDataDict[id] = new TextData(id, value);
            Save();
        }

        [ShowInInspector]
        public static void Save()
        {
            TextAsset textAsset = Resources.Load<TextAsset>(filename);
            string path = AssetDatabase.GetAssetPath(textAsset);

            Debug.Log($"Save text data to {path}");

            if (textDataDict == null)
                textDataDict = new Dictionary<string, TextData>();

            using (StreamWriter sw = File.CreateText(path))
            {
                foreach (TextData textData in textDataDict.Values)
                {
                    string line = $"{textData.id},{textData.value}";
                    sw.WriteLine(line);
                    Debug.Log(line);
                }
            }
        }

        public void OnTextDataDictChanged(CollectionChangeInfo info, object value)
        {
            Save();
        }

        [UnityEditor.Callbacks.DidReloadScripts]
        public static void OnScriptsReloaded()
        {
            Init();
        }
#endif  // UNITY_EDITOR

        private static void Init()
        {
            Load();
        }

        private void Awake()
        {
            Init();
        }
    }


    [Serializable]
    public class TextData
    {
        public string id;
        public string value;

        public TextData(string[] values)
        {
            id = values[0];
            value = values[1];
        }

        public TextData(string id, string value)
        {
            this.id = id;
            this.value = value;
        }
    }


    [Serializable]
    public class RefText
    {
        [ValueDropdown("GetTextDataPreview")]
        [SerializeField]
        private string _id;
        public string id => _id;

        [ShowInInspector]
        public string Value
        {
            get => TextDatabase.GetValue(id);
            set => TextDatabase.SetValue(id, value);
        }

        public static implicit operator string(RefText refText)
        {
            return refText.Value;
        }

        public static IEnumerable<ValueDropdownItem<string>> GetTextDataPreview()
        {
            return TextDatabase.textDataDict.Values.Select(
                data => new ValueDropdownItem<string>($"{data.id} | {data.value}", data.id)).ToArray();
        }
    }
}
