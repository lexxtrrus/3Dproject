using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Room", menuName = "MadEditor/CreateRoom", order = 0)]
public class RoomData : ScriptableObject
{
    public Vector2 sizeOfRoom; // x/y

    public Vector3 centerRoomPos;
    public Quaternion parentRotation;

    public List<GameObject> floorGridPrefabs;
    public List<Vector3> gridLocalPos;
    public List<Quaternion> gridLocalRotation;

    public DirectionsType forwardDirection;
    public GameObject forwardCoridorPrefab;
    public Vector3 forwardCoridorPos;
    public Quaternion forwardCoridorRotation;


    public DirectionsType backDirection;
    public GameObject backCoridorPrefab;
    public Vector3 backCoridorPos;
    public Quaternion backCoridorRotation;
}
