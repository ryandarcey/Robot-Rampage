using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareRoom : MonoBehaviour
{
	// TODO: get size from code?
    Vector3 roomSize = new Vector3(30, 15, 30);	// (x-axis size, y-axis size, z-axis size)
    // down, left, up, right
	ArrayList connectionArr = new();
	ArrayList openConnections = new();

	public SquareRoom()
	{
		
	}

	public void Awake()
	{
		Debug.Log("SquareRoom : Awake()");
		// grabs "connection" components and stores them in an array
		Component[] components = this.GetComponentsInChildren(typeof(Component));
		foreach (Component component in components)
		{
			string compString = component.ToString();
			if (compString.Contains("Connection"))
			{
				//Debug.Log(component.transform.forward);
				//Debug.Log(component.ToString());
			}

			// switch statement for compString wasn't working for some reason
			if (compString.Contains("DownConnection")) {
				Debug.Log(component.ToString());
				connectionArr.Insert(0, component);
				// down is never an open connection
			}
			else if (compString.Contains("LeftConnection"))
			{
				connectionArr.Add(component);
				openConnections.Add(component);
			}
			else if (compString.Contains("UpConnection"))
			{
				connectionArr.Add(component);
				openConnections.Add(component);
			}
			else if (compString.Contains("RightConnection"))
			{
				connectionArr.Add(component);
				openConnections.Add(component);
			}
		}
	}

	// rotate and move this room to connect the 'down' connection point
	//	to the provided connection
	public void ConnectRoomToConnection(Component connection)
	{
		// based on connection, find new rotation (so 'down' so is pointing opposite 'connection')
		// and rotate this room accordingly
		Vector3 rDiff = this.transform.rotation.ToEulerAngles() + FindNewRotation(connection);
		this.transform.rotation.SetEulerAngles(rDiff.x, rDiff.y, rDiff.z);

		// based on the size of the room, find new position
		// and move this room accordingly
		this.transform.SetPositionAndRotation(FindNewPosition(connection), this.transform.rotation);
	}

	public Vector3 FindNewRotation(Component connection)
	{
		Vector3 connForward = connection.transform.forward;
		Component down = (Component)connectionArr[0];
		Vector3 downForward = down.transform.forward;
		Vector3 diff = (-1 * connForward) - downForward;
		Quaternion diffQ = Quaternion.Euler(diff.x, diff.y, diff.z);

		return diff;
	}

	public Vector3 FindNewPosition(Component connection)
	{
		//Vector3 upRot = connectionArr[2].transform.forward;
		Vector3 connForward = connection.transform.forward;
		Vector3 connPosition = connection.transform.position;

		Vector3 newPos = new Vector3(connPosition.x, connPosition.y, connPosition.z);

		if(connForward.x != 0)
		{
			newPos.x += connForward.x * roomSize.z / 2;
		}
		else if(connForward.y != 0) {
			newPos.y += connForward.y * roomSize.z / 2;
		}
		else if(connForward.z != 0)
		{
			newPos.z += connForward.z * roomSize.z / 2;
		}
		
		return newPos;
	}

	public Component FindNewConnection()
	{
		int r = UnityEngine.Random.Range(0, openConnections.Count);
		print(openConnections.Count);   // this is 
		Component c = (Component)openConnections[r];
		openConnections.RemoveAt(r);
		return c;
	}
}
