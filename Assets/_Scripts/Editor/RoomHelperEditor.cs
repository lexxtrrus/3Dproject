using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(RoomHelperConstruction))]
public class RoomHelperEditor : Editor
{
    private RoomHelperConstruction rhc;

    private void OnEnable()
    {
        rhc = (RoomHelperConstruction)target;
    }

    public override void OnInspectorGUI()
    {
        if(GUILayout.Button("Rotate Right"))
        {
            rhc.RotateRoomRight();
        }
        if (GUILayout.Button("RotateLeft"))
        {
            rhc.RotateRoomLeft();
        }
        if (GUILayout.Button("Destroy This Room"))
        {
            rhc.DestroyThisRoom();
        }

        if (GUILayout.Button("SaveThisAsset"))
        {
            rhc.SaveThisAssetOnProjectPanel();
        }
    }
}
