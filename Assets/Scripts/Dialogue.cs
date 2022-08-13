using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
using Sirenix.OdinInspector.Editor;
#endif

namespace _EI
{
    [Serializable]
    public class Dialogue
    {
        [SerializeField, OnCollectionChanged(after: "OnPassagesChanged")]
        private List<Passage> _passages;

        [SerializeField, ValueDropdown("GetPassageList")]
        private string _entryID;

        private Dictionary<string, Passage> _passageDict;
        private Dictionary<string, Passage> passageDict
        {
            get
            {
                if (_passageDict == null)
                    Initialize();
                return _passageDict;
            }
        }

        public List<Passage> passages => _passages;
        public string entryID { get => _entryID; set => _entryID = value; }


        public Passage Entry
        {
            get
            {
                if (!string.IsNullOrEmpty(_entryID))
                    return passageDict[_entryID];
                if (passageDict.Count > 0)
                    return passageDict.Values.First();
                return null;
            }
        }


        public Passage SelectOption(Passage passage, Option option)
        {
            if (passage == null)
                throw new NullReferenceException("Select option when there is no dialogue passage.");

            passage.OnPassageEnd.Invoke();

            string nextPassageID = passage.GetNext(option);
            if (string.IsNullOrEmpty(nextPassageID))
                return null;

            Passage nextPassage = passageDict[nextPassageID];
            nextPassage.OnPassageStart.Invoke();
            return nextPassage;
        }

        public void Initialize()
        {
            _passageDict = new Dictionary<string, Passage>();
            foreach (Passage p in _passages)
            {
                passageDict.Add(p.id, p);
            }
        }

#if UNITY_EDITOR
        public void OnPassagesChanged(CollectionChangeInfo info, object value)
        {
            foreach (Passage psg in _passages)
            {
                psg.dialogue = this;
            }
        }

        public IEnumerable<ValueDropdownItem<string>> GetPassageList()
        {
            var list = passages.Select(p => new ValueDropdownItem<string>($"({p.id.Substring(0, 6)}) \t{p.name}", p.id));
            return list;
        }
#endif
    }

    [System.Serializable]
    public class DialogueEvent : UnityEvent { }

    [Serializable]
    public class Passage
    {
        [SerializeField, HideInInspector]
        private string _id;
        [ShowInInspector, PropertyOrder(-1)]
        public string id
        {
            get
            {
#if UNITY_EDITOR
                if (string.IsNullOrEmpty(_id))
                {
                    _id = GUID.Generate().ToString();
                }
#endif
                return _id;
            }
        }

        [SerializeField, Tooltip("Optional field"), TabGroup("Properties")]
        public string name;

        [SerializeReference, HideInInspector, TabGroup("Properties")]
        public Dialogue dialogue;


        [SerializeField, TabGroup("Properties")]
        private Speaker _speaker;

        [SerializeField, TabGroup("Properties")]
        private RefText _content;

        [SerializeField, ValueDropdown("@dialogue.GetPassageList()"), TabGroup("Properties")]
        private List<string> _nextPassages;

        [SerializeField, TabGroup("Properties")]
        private List<Option> _options;


        [TabGroup("Events")]
        public DialogueEvent OnPassageStart;
        [TabGroup("Events")]
        public DialogueEvent OnPassageEnd;



        public Speaker speaker { get => _speaker; }
        public RefText content { get => _content; }
        public List<Option> nextPassages { get => _options; private set => _options = value; }
        public List<Option> options { get => _options; private set => _options = value; }
        public bool hasOptions => (_options != null) ? (_options.Count > 0) : false;


        public string GetNext(Option option = null)
        {
            if (!hasOptions)
            {
                if (_nextPassages.Count == 1)
                {
                    return _nextPassages[0];
                }
                return null;
            }

            int index = _options.IndexOf(option);
            if (index < 0)
                throw new ArgumentOutOfRangeException("Invalid option");
            return _nextPassages[index];
        }
    }


    [Serializable]
    public class Speaker
    {
        public RefText displayName;
    }

    [Serializable]
    public class Option
    {
        public RefText text;
    }
}
