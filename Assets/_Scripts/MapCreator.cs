using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public enum DirectionsType
{
    Up, Left, Right, Down
}

[ExecuteInEditMode]
public class MapCreator : MonoBehaviour
{
    private float[] rotations = { 0f, 90f, 180f, 270f};

    [Header("Префабы плиток")]
    [SerializeField] List <GameObject> floorPrefabs;

    [Header("Размер комнаты")]
    [SerializeField, Range(1,6)] private int width;
    [SerializeField, Range(1,6)] private int height;

    [Header("Позиция комнаты (ЦЕНТР)")]
    [SerializeField] private Vector3 roomPos;

    public List<RoomData> roomAssets = new List<RoomData>();
    public List<GameObject> roomsGameObjects = new List<GameObject>();

    private int index = 0;


    public void CreateRoomAsset()
    {
        GameObject room = new GameObject("Room");
        room.transform.position = roomPos;


        RoomData roomAsset = ScriptableObject.CreateInstance("RoomData") as RoomData;
        roomAsset.sizeOfRoom = new Vector2(width, height);
        roomAsset.centerRoomPos = room.transform.position;
        roomAsset.floorGridPrefabs = new List<GameObject>();
        roomAsset.gridLocalPos = new List<Vector3>();
        roomAsset.gridLocalRotation = new List<Quaternion>();

        int xPos = (int)(width * 0.5f);
        int yPos = (int)(height * 0.5f);

        Vector3 firstPos = Vector3.zero;

        // СМЕЩЕНИЕ САМОЙ ПЕРВОЙ ПЛИТКИ ОТНОСИТЕЛЬНО РОДИТЕЛЯ, БЛАГОДАРЯ ЭТОМУ РОДИТЕЛЬ ВСЕГДА БУДЕТ В ЦЕНТРЕ КОМНАТЫ (ширина плитки - 4)
        if (width % 2 != 0)
        {
            firstPos = new Vector3(firstPos.x - xPos * 4f, 0f, 0f);
        }
        else
        {
            firstPos = new Vector3(firstPos.x + 2f- xPos * 4f, 0f, 0f);
        }

        if (height % 2 != 0)
        {
            firstPos = new Vector3(firstPos.x, 0f, firstPos.z - yPos * 4f);
        }
        else
        {
            firstPos = new Vector3(firstPos.x, 0f, firstPos.z + 2f - yPos * 4f);
        }

        // цикл создания комнаты и сохранение её параметров в asset 
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GameObject grid = Instantiate(ReturnRadnomFloorPrefab(out index));
                grid.transform.rotation = ReturnRandomFloorGrig();
                grid.transform.SetParent(room.transform);
                grid.transform.localPosition = new Vector3(firstPos.x + 4f * x, 0f, firstPos.z + 4f * z);

                roomAsset.floorGridPrefabs.Add(floorPrefabs[index]);
                roomAsset.gridLocalPos.Add(grid.transform.localPosition);
                roomAsset.gridLocalRotation.Add(grid.transform.localRotation);
            }
        }

        room.AddComponent<BoxCollider>().size = new Vector3(width * 4f, 0.25f, height * 4f);

        roomAsset = CreateForwardDirection(roomAsset, room.transform);
        roomAssets.Add(roomAsset);

        roomsGameObjects.Add(room);

        room.AddComponent<RoomHelperConstruction>().SetMapCreator(this, roomAsset);

        //AssetDatabase.Refresh();
        //AssetDatabase.CreateAsset(roomAsset, AssetDatabase.GenerateUniqueAssetPath("Assets/RoomDatas/" + typeof(RoomData).ToString() + ".asset"));
    }

    private Quaternion ReturnRandomFloorGrig()
    {
        int rand = UnityEngine.Random.Range(0, 4);
        return Quaternion.Euler(0f, rotations[rand], 0f);
    }

    private GameObject ReturnRadnomFloorPrefab(out int index)
    {
        int rand = UnityEngine.Random.Range(0, 4);
        index = rand;
        return floorPrefabs[rand];
    }

    public void RemoveAllAseets()
    {
        roomAssets.Clear();
    }

    public void RemoveParticularAsset(RoomData roomAsset)
    {
        roomAssets.Remove(roomAsset);
    }

    public void RemoveAllRoomsFromScene()
    {
        foreach (var room in roomsGameObjects)
        {
            DestroyImmediate(room);
        }
        roomsGameObjects.Clear();
    }

    public void RemoveParticularRoom(GameObject room)
    {
        roomsGameObjects.Remove(room);
    }

    public RoomData CreateForwardDirection(RoomData roomAsset, Transform center)
    {
        GameObject forwardGrid = Instantiate(ReturnRadnomFloorPrefab(out index));
        roomAsset.forwardCoridorPrefab = floorPrefabs[index];

        forwardGrid.transform.rotation = ReturnRandomFloorGrig();
        roomAsset.forwardCoridorRotation = forwardGrid.transform.rotation;

        forwardGrid.AddComponent<BoxCollider>().size = new Vector3(4f, 0f, 4f);

        int forwardTranslate = Random.Range(0, 3);
        roomAsset.forwardDirection = (DirectionsType)forwardTranslate;

        //back corridor
        GameObject backGrid = Instantiate(ReturnRadnomFloorPrefab(out index));
        roomAsset.backCoridorPrefab = floorPrefabs[index];

        backGrid.transform.rotation = ReturnRandomFloorGrig();
        roomAsset.backCoridorRotation = forwardGrid.transform.rotation;

        backGrid.AddComponent<BoxCollider>().size = new Vector3(4f, 0f, 4f);

        forwardTranslate = Random.Range(1, 4);
        roomAsset.backDirection = (DirectionsType)forwardTranslate;

        roomAsset = SetForwardCorridor(forwardGrid, backGrid, roomAsset, center);

        return roomAsset;
    }

    public RoomData SetForwardCorridor(GameObject forwardCorridorGrid, GameObject backCorridorGrid ,RoomData roomAsset, Transform center)
    {
        //хер с ним, пусть и задний корридор делает

        forwardCorridorGrid.transform.SetParent(center);
        backCorridorGrid.transform.SetParent(center);
        switch (roomAsset.forwardDirection)
        {
            case DirectionsType.Up:                
                int yPos = (int)(roomAsset.sizeOfRoom.y * 0.5f);
                
                if ((int)roomAsset.sizeOfRoom.y % 2 == 0)
                {
                    yPos = -2 + yPos * 4 + 4;
                }
                else
                {                   
                    yPos = yPos * 4 + 4;
                }

                forwardCorridorGrid.transform.localPosition = new Vector3(0f, 0f, yPos);
                roomAsset.forwardCoridorPos = new Vector3(0f, 0f, yPos);
                break;

            case DirectionsType.Left:

                int xPos = (int)(roomAsset.sizeOfRoom.x * 0.5f);

                if ((int)roomAsset.sizeOfRoom.x % 2 == 0)
                {
                    xPos = 2 - xPos * 4 - 4;
                }
                else
                {
                    xPos = - xPos * 4 - 4;
                }

                forwardCorridorGrid.transform.localPosition = new Vector3(xPos, 0f, 0f);
                roomAsset.forwardCoridorPos = new Vector3(xPos, 0f, 0f); ;
                break;
            
            case DirectionsType.Right:
                 xPos = (int)(roomAsset.sizeOfRoom.x * 0.5f);

                if ((int)roomAsset.sizeOfRoom.x %2 == 0)
                {
                    xPos = -2 + xPos * 4 + 4;
                }
                else
                {
                    xPos = xPos * 4 + 4;
                }

                forwardCorridorGrid.transform.localPosition = new Vector3(xPos, 0f, 0f);
                roomAsset.forwardCoridorPos = new Vector3(xPos, 0f, 0f);
                break;
        }

        if(roomAsset.forwardDirection == roomAsset.backDirection)
        {
            roomAsset.backDirection = DirectionsType.Down;
        }

        switch (roomAsset.backDirection)
        {
            case DirectionsType.Down:
                int yPos = (int)(roomAsset.sizeOfRoom.y * 0.5f);

                if ((int)roomAsset.sizeOfRoom.y % 2 == 0)
                {
                    yPos = 2 - yPos * 4 - 4;
                }
                else
                {
                    yPos = - yPos * 4 - 4;
                }

                backCorridorGrid.transform.localPosition = new Vector3(0f, 0f, yPos);
                roomAsset.backCoridorPos = new Vector3(0f, 0f, yPos);
                break;

            case DirectionsType.Left:

                int xPos = (int)(roomAsset.sizeOfRoom.x * 0.5f);

                if ((int)roomAsset.sizeOfRoom.x % 2 == 0)
                {
                    xPos = 2 - xPos * 4 - 4;
                }
                else
                {
                    xPos = -xPos * 4 - 4;
                }

                backCorridorGrid.transform.localPosition = new Vector3(xPos, 0f, 0f);
                roomAsset.backCoridorPos = new Vector3(xPos, 0f, 0f); ;
                break;

            case DirectionsType.Right:
                xPos = (int)(roomAsset.sizeOfRoom.x * 0.5f);

                if ((int)roomAsset.sizeOfRoom.x % 2 == 0)
                {
                    xPos = -2 + xPos * 4 + 4;
                }
                else
                {
                    xPos = xPos * 4 + 4;
                }

                backCorridorGrid.transform.localPosition = new Vector3(xPos, 0f, 0f);
                roomAsset.backCoridorPos = new Vector3(xPos, 0f, 0f);
                break;
        }

        return roomAsset;
    }

    public void Update()
    {
        transform.position = new Vector3((int)transform.position.x, 0f, (int)transform.position.z);
        roomPos = transform.position;
    }

    public void SaveThisAsset(RoomData roomAsset)
    {
        AssetDatabase.Refresh();
        AssetDatabase.CreateAsset(roomAsset, AssetDatabase.GenerateUniqueAssetPath("Assets/RoomDatas/" + typeof(RoomData).ToString() + ".asset"));
    }
}
