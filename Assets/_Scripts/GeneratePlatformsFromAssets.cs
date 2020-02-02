using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePlatformsFromAssets : MonoBehaviour
{
    [SerializeField] private List<RoomData> rooms;

    private void Awake()
    {
        foreach (var item in rooms)
        {
            GameObject room = new GameObject("Room");
            room.transform.position = item.centerRoomPos;
            room.transform.rotation = item.parentRotation;

            for (int i = 0; i < item.floorGridPrefabs.Count; i++)
            {
                GameObject grid = Instantiate(item.floorGridPrefabs[i], room.transform);
                grid.transform.localPosition = item.gridLocalPos[i];
                grid.transform.localRotation = item.gridLocalRotation[i];
            }

            room.AddComponent<BoxCollider>().size = new Vector3(item.sizeOfRoom.x * 4f, 0f, item.sizeOfRoom.y * 4f);

            GameObject forwardCoridorGrid = Instantiate(item.forwardCoridorPrefab, room.transform);
            forwardCoridorGrid.transform.localPosition = item.forwardCoridorPos;
            forwardCoridorGrid.transform.localRotation = item.forwardCoridorRotation;

            forwardCoridorGrid.AddComponent<BoxCollider>().size = new Vector3(4f, 0f, 4f);

            GameObject backCoridorGrid = Instantiate(item.backCoridorPrefab, room.transform);
            backCoridorGrid.transform.localPosition = item.backCoridorPos;
            backCoridorGrid.transform.localRotation = item.backCoridorRotation;

            backCoridorGrid.AddComponent<BoxCollider>().size = new Vector3(4f, 0f, 4f);
        }
    }
}
