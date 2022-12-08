using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    private GameObject playerObj = null;
    
    public SquareRoom squareRoomPrefab;
    public Hallway hallwayPrefab;
    public TConnection tConnectionPrefab;
    public TallRoom tallRoomPrefab;
    public DownHallway downHallwayPrefab;

    public GameObject robotEasyPrefab;
    public GameObject robotMediumPrefab;
    public GameObject robotHardPrefab;

    public ArrayList roomPositions = new();

	public int numRooms = 3;
    public float overlapCheckRadius = 20;
    public int maxNumHallwaysInARow = 3;

	// Start is called before the first frame update
	void Start()
    {
        if (playerObj == null)
        {
            playerObj = GameObject.Find("Player");
        }

        GameObject[] robotPrefabs = { robotEasyPrefab, robotMediumPrefab, robotHardPrefab };


        // TODO:
        //  - choose different types of rooms
        //  - set end room (idk what we want to do for this for now)

        SquareRoom baseRoom = Instantiate(squareRoomPrefab);
        baseRoom.setDownWall(true);
        
        baseRoom.transform.SetPositionAndRotation(Vector3.zero, baseRoom.transform.rotation);
        roomPositions.Add(Vector3.zero);

        Component connection = baseRoom.FindNewConnection();
        baseRoom.setWalls(connection);

        int currentNumHallways = 0;
        for(int i = 0; i < numRooms; i++)
        {
            //Debug.Log("Adding room #" + (i + 1));
            bool isSquare = false;

            // instantiate newest room being placed
            Room nextRoom;

            if(i == numRooms-1)
            {
                nextRoom = nextRoom = Instantiate(squareRoomPrefab);
			}
            // simple logic to prevent more than 3 hallways from being placed in a row,
            //  "hallway" currently being a hallway, tConnection, or downHallway
            else if (currentNumHallways < maxNumHallwaysInARow)
            {
                int r = UnityEngine.Random.Range(0, 5);
                if (r == 0)
                {
                    nextRoom = Instantiate(squareRoomPrefab);
                    isSquare = true;
                }
                else if (r == 1)
                {
                    nextRoom = Instantiate(hallwayPrefab);
                    currentNumHallways++;
                }
                else if (r == 2)
                {
                    nextRoom = Instantiate(tConnectionPrefab);
					currentNumHallways++;
				}
                else if (r == 3)
                {
                    nextRoom = Instantiate(tallRoomPrefab);
                }
                else
                {
                    nextRoom = Instantiate(downHallwayPrefab);
					currentNumHallways++;
				}
            }
            else
            {
                currentNumHallways = 0;
                int r = UnityEngine.Random.Range(0, 2);
				if (r == 0)
				{
					nextRoom = Instantiate(squareRoomPrefab);
				}
				else
				{
					nextRoom = Instantiate(tallRoomPrefab);
				}
			}
			
            nextRoom.setDownWall(false);

            // connect it to the chosen connection from previous room
            //  and add it's position to the list
            Vector3 nextRoomPos = nextRoom.ConnectRoomToConnection(connection);
            roomPositions.Add(nextRoomPos);

            if (i != numRooms - 1)
            {
                // choose a connection from newest room that won't result in overlapping with previous rooms
                while (connection != null)
                {
                    connection = nextRoom.FindNewConnection();

                    // if the connection won't result in the next room overlapping an existing room,
                    //  then break out of the while()
                    if (connection != null && !WillConnectionOverlap(connection))
                    {
                        break;
                    }
                }
                if (connection == null)
                {
                    break;  // if it gets 'stuck', exit for loop
                }

				nextRoom.setWalls(connection);
                if (isSquare)
                {
                    int random = UnityEngine.Random.Range(0, 3);
                    nextRoom.SpawnEnemies(robotPrefabs[random]);
                    Debug.Log("SPAWNING ENEMIES");
                }
			}
            else
            {
				nextRoom.setWalls(null);
			}
        }
    }

    private bool WillConnectionOverlap(Component connection)
    {
        //Debug.Log(connection.ToString());
        Vector3 connForward = connection.transform.forward;
        // estimate position of new room by adding OCR in the direction of the connection to the connection's position
        Vector3 newRoomEstimate = connection.transform.position + overlapCheckRadius * connForward;
        //Debug.Log("New Room Estimated Pos:  " + newRoomEstimate.ToString());

        foreach(Vector3 pos in roomPositions)
        {
            Vector3 diff = pos - newRoomEstimate;
            //Debug.Log("diff.magnitude:  " + diff.magnitude);
            if(diff.magnitude < overlapCheckRadius)
            {
                //Debug.Log("LevelGen : WillConnOverlap() -- TRUE");
                return true;
            }
        }

		//Debug.Log("LevelGen : WillConnOverlap() -- FALSE");
		return false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
