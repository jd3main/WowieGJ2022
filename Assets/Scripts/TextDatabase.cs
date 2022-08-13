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

[ShowOdinSerializedPropertiesInInspector]
[ExecuteAlways]
public class TextDatabase : MonoBehaviour
{
    static readonly string[] ENDL = { "\r\n", "\r", "\n" };
    [ShowInInspector]
    public static TextDatabase instance;

    public char delim = '\t';
    public string filename = "dialogue";
    public int idColumnIndex = 2;
    public int valueColumnIndex = 4;

    [ShowInInspector]
    public static Dictionary<string, TextData> textDataDict = new Dictionary<string, TextData>();


    private void OnEnable()
    {
        Init();
    }

    private void Init()
    {
        instance = this;
        Load();
    }


    public string GetValue(string id)
    {
        if (string.IsNullOrEmpty(id) || !textDataDict.ContainsKey(id))
            return null;
        return textDataDict[id].value;
    }

    [ShowInInspector]
    public void Load()
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
            string[] values = row.Split(delim);
            if (idColumnIndex < values.Length && valueColumnIndex < values.Length)
            {
                string id = values[idColumnIndex];
                string value = values[valueColumnIndex];
                TextData textData = new TextData(id, value);
                textDataDict[textData.id] = textData;
            }
        }
    }

#if UNITY_EDITOR
    [UnityEditor.Callbacks.DidReloadScripts]
    public void OnScriptsReloaded()
    {
        Init();
    }
#endif  // UNITY_EDITOR

}


[Serializable]
public class TextData
{
    public string id;
    public string value;


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
        get => TextDatabase.instance.GetValue(id);
        //set => TextDatabase.instance.SetValue(id, value);
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
