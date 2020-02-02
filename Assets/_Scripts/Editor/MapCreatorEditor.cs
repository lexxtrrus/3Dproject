using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MapCreator))]
public class MapCreatorEditor : Editor
{
    private MapCreator mapCreator;
    private void OnEnable()
    {
        mapCreator = (MapCreator)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if(GUILayout.Button("Create Room", GUILayout.Width(250f), GUILayout.Height(30f)))
        {
            mapCreator.CreateRoomAsset();
        }      

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Clear List of Assets", GUILayout.Width(125f)))
        {
            mapCreator.RemoveAllAseets();
        }

        if (GUILayout.Button("DESTROY ROOMS", GUILayout.Width(125f)))
        {
            mapCreator.RemoveAllRoomsFromScene();
        }
        GUILayout.EndHorizontal();

        
    }
}
