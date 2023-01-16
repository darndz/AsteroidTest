using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
[CreateAssetMenu(fileName = "ObjectsId", menuName = "ScriptableObjects/ObjectsId", order = 1)]
public class ObjectsViewsData : ScriptableObject
{
    [Serializable]
    public struct ObjectView
    {
        public int id;
        public GameObject Prefab;
    }
    [SerializeField] public List<ObjectView> Objects = new List<ObjectView>();
    public bool GetObject(int id, out GameObject obj)
    {
        foreach (ObjectView objectView in Objects)
        {
            if (objectView.id == id)
            {
                obj = objectView.Prefab;
                return true;
            }
        }
        obj = new GameObject();
        return false;
    }
    public void AddNewObject()
    {
        var obj = new ObjectView();
        obj.id = Objects.Count;
        Objects.Add(obj);
    }
}


#if UNITY_EDITOR
[CustomEditor(typeof(ObjectsViewsData))]
public class ObjectsViewsDataEditor : Editor 
{
    public override void OnInspectorGUI () {
        DrawDefaultInspector();
        var list = target as ObjectsViewsData;

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Add new"))
        {
            list.AddNewObject();
        }

        GUILayout.EndHorizontal();
    }
}
#endif