using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomHelperConstruction : MonoBehaviour
{
    public void SetMapCreator(MapCreator mapCreator, RoomData roomAsset)
    {
        _map = mapCreator;
        _roomAsset = roomAsset;
    }

    private MapCreator _map;
    private RoomData _roomAsset;

    public void RotateRoomRight()
    {
        transform.Rotate(Vector3.up * 90f);
    }

    public void RotateRoomLeft()
    {
        transform.Rotate(Vector3.up * -90f);
    }

    public void DestroyThisRoom()
    {
        _map.RemoveParticularAsset(_roomAsset);
        _map.RemoveParticularRoom(gameObject);
        DestroyImmediate(gameObject);
    }

    private void OnDestroy()
    {
        _map.RemoveParticularRoom(gameObject);
    }

    public void SaveThisAssetOnProjectPanel()
    {
        _roomAsset.centerRoomPos = transform.position;
        _roomAsset.parentRotation = transform.rotation;

        _map.SaveThisAsset(_roomAsset);
    }
}
