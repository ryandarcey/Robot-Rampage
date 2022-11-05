using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRoom
{
	// Properties a room needs:
	//	roomSize --> size of model in Unity units (width x length x height from the perspective of the down wall?)
	Vector3 roomSize { get; set; }
	//	doorways/connections (objects/structs that store position, orientation)

	//	connectionsArr --> stores booleans (whether/not a connection has another room connected)
	bool[] roomList { get; set; }

	// Methods a room needs:
	//	getConnectionByIndex(int i)
	//	rotateRoom(quarternion?)
	//	moveRoom(Vector3 position)
	//	
}
