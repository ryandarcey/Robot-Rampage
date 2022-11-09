using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

using Component = UnityEngine.Component;

public class SquareRoom : MonoBehaviour
{
	// TODO: get size from code?
    Vector3 roomSize = new Vector3(30, 15, 30);	// (x-axis size, y-axis size, z-axis size)
    // down, left, up, right
	ArrayList connectionArr = new();
	ArrayList openConnections = new();
	ArrayList walls = new();

	public SquareRoom()
	{
		
	}

	public void Awake()
	{
		//Debug.Log("SquareRoom : Awake()");
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
				//Debug.Log(component.ToString());
				connectionArr.Insert(0, component);
				// down is never an open connection
			}
			else if (compString.Contains("LeftConnection"))
			{
				connectionArr.Insert(1, component);
				openConnections.Add(component);
			}
			else if (compString.Contains("UpConnection"))
			{
				connectionArr.Insert(2, component);
				openConnections.Add(component);
			}
			else if (compString.Contains("RightConnection"))
			{
				connectionArr.Insert(0, component);
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

		// based on the size of the room, find new position
		// and move this room accordingly
		this.transform.position = FindNewPosition(connection);
		FindNewRotation(connection);
	}

	public void FindNewRotation(Component connection)
	{
		Vector3 connForward = connection.transform.forward;
		Vector3 target = this.transform.position + connForward;
		this.transform.LookAt(target);
		//Debug.Log(connForward);
		/*Component down = (Component)connectionArr[0];
		Vector3 downForward = down.transform.forward;
		Vector3 diff = (-1 * connForward) - downForward;
		Debug.Log(diff);
		Vector3 currentRot = this.transform.rotation.eulerAngles;
		Quaternion newRotQ = Quaternion.Euler(currentRot.x + diff.x, currentRot.y + diff.y, currentRot.z + diff.z);*/

		//return newRotQ;
	}

	public Vector3 FindNewPosition(Component connection)
	{
		//Vector3 upRot = connectionArr[2].transform.forward;
		Vector3 connForward = connection.transform.forward;
		//Debug.Log("Connection's forward: " + connForward);
		Vector3 connPosition = connection.transform.position;

		Vector3 newPos = new Vector3(connPosition.x, connPosition.y, connPosition.z);
		float roomLength = roomSize.z;
		
		// floating point makes these vector components sometimes very small when they "should" be zero
		if(Math.Abs(connForward.x) > 0.01)
		{
			newPos.x += connForward.x * roomLength / 2;
			//Debug.Log("New X = " + newPos.x);
		}
		else if(Math.Abs(connForward.y) > 0.01) {
			newPos.y += connForward.y * roomLength / 2;
			//Debug.Log("New Y = " + newPos.y);
		}
		else if(Math.Abs(connForward.z) > 0.01)
		{
			newPos.z += connForward.z * roomLength / 2;
			//Debug.Log("New Z = " + newPos.z);
		}
		
		return newPos;
	}

	public UnityEngine.Component FindNewConnection()
	{
		int r = UnityEngine.Random.Range(0, openConnections.Count);
		Component c = (Component)openConnections[r];
		string compString = c.ToString();
		openConnections.RemoveAt(r);
		
		
		
		
		
		
		return c;
	}

	// only to be called on starting room --
	//	sets "down" to be a wall instead of doorway
	public void setDownWall()
	{
		Component[] components = this.GetComponentsInChildren(typeof(Component));
		foreach (Component component in components)
		{
			string compString = component.ToString();
			if (compString.Equals("Wall Down"))
			{
				component.gameObject.SetActive(true);
			}

			if (compString.Equals("Door Wall Down"))
			{
				component.gameObject.SetActive(false);
			}
		}
	}
}
