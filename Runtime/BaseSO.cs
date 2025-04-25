using UnityEngine;

namespace DoppleLittleHelper
{
    public class BaseSO : ScriptableObject
    {
        public virtual void Clear()
        {
            Debug.Log("CLEAR function override");
        }

        public void Save()
        {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }
    }

    public class BaseData
    {
        public virtual void Parse(string[] _data, string[] _keys, string[] _types)
        {
            CustomParse();
        }
        public virtual void CustomParse() { }
    }
}
