using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    int numRooms = 3;
    public SquareRoom squareRoomPrefab;

    // Start is called before the first frame update
    void Start()
    {
        /*
         * starting with the base room, loop for n number of rooms,
         * making a new room, picking a connection from the previous room,
         * and attaching the new room to the previous room (at the chosen connection)
         */

        // TODO:
        //  - fix rotation
        //  - check if deterministic (always picks same rooms)?
        //  - fix rooms being able to overlap with existing rooms
        //      - NOTE: i'm pretty sure you can't check collision/overlaps in the Start() method (???)

        SquareRoom baseRoom = Instantiate(squareRoomPrefab);
        baseRoom.setDownWall(true);
        
        baseRoom.transform.SetPositionAndRotation(new Vector3(0, 0, 0), baseRoom.transform.rotation);
        Component connection = baseRoom.FindNewConnection();

        for(int i = 0; i < numRooms; i++)
        {
            SquareRoom nextRoom = Instantiate(squareRoomPrefab);
            nextRoom.setDownWall(false);


            nextRoom.ConnectRoomToConnection(connection);
            connection = nextRoom.FindNewConnection();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
