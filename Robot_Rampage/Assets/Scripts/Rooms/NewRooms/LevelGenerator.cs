using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.MemoryProfiler;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Hallway hallwayPrefab;
	public SquareRoom squareRoom1Prefab;
	public SquareRoom squareRoom2Prefab;
	public SquareRoom squareRoom3Prefab;

	private const int overlapCheckRadius = 92;

	//private SquareRoom baseRoom;
	//private Component previousRoomConnection = null;

	public GameObject robotEasyPrefab;
    public GameObject robotMediumPrefab;
    public GameObject robotHardPrefab;

	private ArrayList placedRooms = new();
	private int nextRoomID = 0;

	public int numRoomsToGenerateAtATime = 20;
    public int maxNumHallwaysInARow = 1;    // TODO: get rid of this and hardcode max 1 hallway in a row

	public static LevelGenerator instance;

	void Awake()
	{
		// singleton SettingsManager
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			// already have SettingsManager in scene, don't need another one
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);  // SettingsManager persists between scenes
	}

	// Start is called before the first frame update
	void Start()
	{
		SquareRoom baseRoom = Instantiate(squareRoom1Prefab);
		//baseRoom.SetDownDoor(true); // TODO

		baseRoom.transform.SetPositionAndRotation(Vector3.zero, baseRoom.transform.rotation);

		placedRooms.Add(baseRoom);
		baseRoom.roomID = nextRoomID;
		nextRoomID++;

		GenerateRooms();

		/*Component previousRoomConnection = baseRoom.FindNewConnection();
		baseRoom.SetDoors(previousRoomConnection);*/
	}

    private bool WillConnectionOverlap(Component connection, Room room)
    {
		if (connection == null)
		{
			return true;
		}
		// TODO: more robust overlap checking (essentially full hitbox checking)

		//Debug.Log(connection.ToString());
		Vector3 connForward = connection.transform.forward;
		// estimate position of new room by adding Z/2 in the direction of the connection to the connection's position
		Vector3 newRoomEstimate = connection.transform.position + room.roomSize.z/2 * connForward;
		Vector3 newRoomSize = room.roomSize;
		//Debug.Log("New Room Estimated Pos:  " + newRoomEstimate.ToString());

		foreach (Room placedRoom in placedRooms)
		{
			Vector3 placedPos = placedRoom.transform.position;
			Vector3 placedSize = placedRoom.roomSize;
			Vector3 diff = placedPos - newRoomEstimate;

			/*Debug.Log("");
			Debug.Log("PLACED ROOM POSITION: " + placedPos);
			Debug.Log("NEW ROOM POSITION:    " + newRoomEstimate);
			Debug.Log("");*/

			if (true/*diff.magnitude < overlapCheckRadius*/)
			{
				float placedRoomLength;
				if (placedSize.x > placedSize.z)
					placedRoomLength = placedSize.x;
				else
					placedRoomLength = placedSize.z;

				float newRoomLength;
				if (newRoomSize.x > newRoomSize.z)
					newRoomLength = newRoomSize.x;
				else
					newRoomLength = newRoomSize.z;

				float placedRoomMinX = placedPos.x - placedRoomLength/2;
				float placedRoomMaxX = placedPos.x - placedRoomLength/2;
				float placedRoomMinZ = placedPos.z - placedRoomLength/2;
				float placedRoomMaxZ = placedPos.z - placedRoomLength/2;

				float newRoomMinX = newRoomEstimate.x - newRoomLength/2;
				float newRoomMaxX = newRoomEstimate.x - newRoomLength/2;
				float newRoomMinZ = newRoomEstimate.z - newRoomLength/2;
				float newRoomMaxZ = newRoomEstimate.z - newRoomLength/2;

				// check if rooms overlap
				if ((placedRoomMaxX >= newRoomMinX && newRoomMaxX >= placedRoomMinX)
					&& (placedRoomMaxZ >= newRoomMinZ && newRoomMaxZ >= placedRoomMinZ))
				{
					return true;// !DeleteRoom(placedRoom);
				}
			}
		}

		//Debug.Log("LevelGen : WillConnOverlap() -- FALSE");
		return false;
    }

	// returns true if succesfully deleted,
	//	false if not deleted
	bool DeleteRoom(Room room)
	{
		return false;
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RoomEntered(int roomID)
    {
		if (roomID % (numRoomsToGenerateAtATime) == numRoomsToGenerateAtATime / 2)
		{
			Debug.Log("~~~~ player entered room " + roomID + " -- GENERATING more rooms");
			GenerateRooms();
		}
		else if (roomID % (numRoomsToGenerateAtATime) == 0 && roomID >= numRoomsToGenerateAtATime)
		{
			Debug.Log("~~~~ player entered room " + roomID + " -- DESTROYING rooms");
			DestroyRooms(roomID);
		}
		else
			Debug.Log("~~ player entered room " + roomID);

	}

    void GenerateRooms()
    {
		GameObject[] robotPrefabs = { robotEasyPrefab, robotMediumPrefab, robotHardPrefab };
		SquareRoom[] squareRoomPrefabs = { squareRoom1Prefab, squareRoom2Prefab, squareRoom3Prefab };

		int currentNumHallways = 0;
		for (int i = 0; i < numRoomsToGenerateAtATime; i++)
		{
			////////////////////////////////////////
			// pick next room to be placed

			bool isSquare = false;

			// newest room being placed
			Room nextRoom;

			// only 1 hallway should be placed in a row
			if (currentNumHallways < maxNumHallwaysInARow)
			{
				int r = UnityEngine.Random.Range(0, 5);
				if (r < 3)
				{
					// r = 0, 1, or 2
					nextRoom = Instantiate(squareRoomPrefabs[r]);
					isSquare = true;
				}
				else
				{
					// r = 3 or 4
					nextRoom = Instantiate(hallwayPrefab);
					currentNumHallways++;
				}

			}
			else
			{
				int r = UnityEngine.Random.Range(0, 3);
				nextRoom = Instantiate(squareRoomPrefabs[r]);
				isSquare = true;

				currentNumHallways = 0;
			}

			//Debug.Log("NEXT ROOM: " + nextRoom); //////////////////
			//nextRoom.SetDownDoor();

			////////////////////////////////////////
			// connect the room just picked to the previous room (most recently placed room/room last in list)

			Room previousRoom = (Room)placedRooms[^1];
			Component previousRoomConnection = new();

			for (int j=0; j<4; j++)
			{
				previousRoomConnection = previousRoom.FindNewConnection();

				// if the connection won't result in the next room overlapping an existing room,
				//  then break out of the while()
				bool willConOverlap = WillConnectionOverlap(previousRoomConnection, nextRoom);
				Debug.Log("WILL CONNECTION OVERLAP: " + willConOverlap);
				
				if (previousRoomConnection != null && !willConOverlap)
				{
					break;
				}
			}

			if (previousRoomConnection == null)
			{
				Destroy(nextRoom);
				Debug.Log("~~~~ PREVIOUS ROOM CONNECTION NULL, TERMINATING LEVEL GEN ~~~~");
				return;
			}

			//Debug.Log("PREVIOUS CONNECTION: " + previousRoomConnection);	///////////////////
			Vector3 nextRoomPos = nextRoom.ConnectRoomToConnection(previousRoomConnection);
			
			previousRoom.SetDoors(previousRoomConnection, placedRooms.Count == 1);
			//previousRoom.SetDownDoor(placedRooms.Count == 1);	// based room, want to keep down door closed

			placedRooms.Add(nextRoom);
			nextRoom.roomID = nextRoomID;
			nextRoomID++;

			// if not a hallway, spawn enemies in the room
			if (isSquare)
			{
				int random = UnityEngine.Random.Range(0, 3);
				nextRoom.SpawnEnemies(robotPrefabs[random]);
				Debug.Log("SPAWNING ENEMIES");
			}
		}
	}

    void DestroyRooms(int roomID)
    {
		int previouslyDestroyedID = -1;
		while(previouslyDestroyedID < (roomID - numRoomsToGenerateAtATime/2))
		{
			Room previouslyDestroyed = (Room)placedRooms[0];
			placedRooms.RemoveAt(0);
			previouslyDestroyedID = previouslyDestroyed.roomID;
			Destroy(previouslyDestroyed.gameObject);
		}
	}

}

