using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public SquareRoom squareRoomPrefab;
    public Hallway hallwayPrefab;
    public TConnection tConnectionPrefab;
    public TallRoom tallRoomPrefab;

    public ArrayList roomPositions = new();

	public int numRooms = 3;
    public float overlapCheckRadius = 10;

	// Start is called before the first frame update
	void Start()
    {
        // TODO:
        //  - choose different types of rooms
        //  - set end room (idk what we want to do for this for now)

        SquareRoom baseRoom = Instantiate(squareRoomPrefab);
        baseRoom.setDownWall(true);
        
        baseRoom.transform.SetPositionAndRotation(Vector3.zero, baseRoom.transform.rotation);
        roomPositions.Add(Vector3.zero);

        Component connection = baseRoom.FindNewConnection();

        for(int i = 0; i < numRooms; i++)
        {
            //Debug.Log("Adding room #" + (i + 1));


            // instantiate newest room being placed
            Room nextRoom;

			int r = UnityEngine.Random.Range(0, 4);
            if (r == 0)
            {
				nextRoom = Instantiate(squareRoomPrefab);
			}
            else if(r == 1)
            {
                nextRoom = Instantiate(hallwayPrefab);
            }
            else if(r == 2)
            {
                nextRoom = Instantiate(tConnectionPrefab);
            }
            else
            {
                nextRoom = Instantiate(tallRoomPrefab);
            }
			
            nextRoom.setDownWall(false);

            // connect it to the chosen connection from previous room
            //  and add it's position to the list
            Vector3 nextRoomPos = nextRoom.ConnectRoomToConnection(connection);
            roomPositions.Add(nextRoomPos);
            
            // choose a connection from newest room that won't result in overlapping with previous rooms
            while(connection != null)
            {
				connection = nextRoom.FindNewConnection();

                // if the connection won't result in the next room overlapping an existing room,
                //  then break out of the while()
                if(connection != null && !WillConnectionOverlap(connection))
                {
                    break;
                }
			}
            if(connection == null)
            {
                break;  // if it gets 'stuck', exit for loop
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
