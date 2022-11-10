using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Room : MonoBehaviour
{
	// TODO: get size from code?
	public Vector3 roomSize;
	ArrayList connectionArr = new();
	ArrayList openConnections = new();
	ArrayList walls = new();

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

				// switch statement for compString wasn't working for some reason
				if (compString.Contains("DownConnection"))
				{
					//Debug.Log(component.ToString());
					connectionArr.Insert(0, component);
					// down is never an open connection
				}
				else if (compString.Contains("LeftConnection"))
				{
					connectionArr.Add(component);
					//connectionArr.Insert(1, component);
					openConnections.Add(component);
				}
				else if (compString.Contains("UpConnection"))
				{
					connectionArr.Add(component);
					//connectionArr.Insert(2, component);
					openConnections.Add(component);
				}
				else if (compString.Contains("RightConnection"))
				{
					connectionArr.Add(component);
					//connectionArr.Insert(0, component);
					openConnections.Add(component);
				}
				else if (compString.Contains("TopConnection"))
				{
					connectionArr.Add(component);
					//connectionArr.Insert(0, component);
					openConnections.Add(component);
				}
			}

			if (compString.Contains("Wall"))
			{
				//Debug.Log(component.transform.forward);
				//Debug.Log(component.ToString());

				if (compString.Contains("Wall Down"))
				{
					// down is always an open entrance
				}
				else if (compString.Contains("Wall Left"))
				{
					walls.Add(component);
				}
				else if (compString.Contains("Wall Up"))
				{
					walls.Add(component);
				}
				else if (compString.Contains("Wall Right"))
				{
					walls.Add(component);
				}
				else if (compString.Contains("Wall Top"))
				{
					walls.Add(component);
				}
			}

		}
	}

	// rotate and move this room to connect the 'down' connection point
	//	to the provided connection
	public Vector3 ConnectRoomToConnection(Component connection)
	{
		// based on connection, find new rotation (so 'down' so is pointing opposite 'connection')
		// and rotate this room accordingly

		// based on the size of the room, find new position
		// and move this room accordingly
		this.transform.position = FindNewPosition(connection);
		FindNewRotation(connection);
		return this.transform.position;
	}

	private void FindNewRotation(Component connection)
	{
		Vector3 connForward = connection.transform.forward;
		Vector3 target = this.transform.position + connForward;
		this.transform.LookAt(target);
	}

	private Vector3 FindNewPosition(Component connection)
	{
		//Vector3 upRot = connectionArr[2].transform.forward;
		Vector3 connForward = connection.transform.forward;
		//Debug.Log("Connection's forward: " + connForward);
		Vector3 connPosition = connection.transform.position;

		Vector3 newPos = new Vector3(connPosition.x, connPosition.y, connPosition.z);
		float roomLength = roomSize.z;

		// floating point makes these vector components sometimes very small when they "should" be zero
		if (Math.Abs(connForward.x) > 0.01)
		{
			newPos.x += connForward.x * roomLength / 2;
			//Debug.Log("New X = " + newPos.x);
		}
		else if (Math.Abs(connForward.y) > 0.01)
		{
			newPos.y += connForward.y * roomLength / 2;
			//Debug.Log("New Y = " + newPos.y);
		}
		else if (Math.Abs(connForward.z) > 0.01)
		{
			newPos.z += connForward.z * roomLength / 2;
			//Debug.Log("New Z = " + newPos.z);
		}

		return newPos;
	}

	// picks random connection from ones that are open
	// returns null if none are available
	public Component FindNewConnection()
	{
		if (openConnections.Count == 0)
		{
			return null;
		}
		int r = UnityEngine.Random.Range(0, openConnections.Count);
		Component c = (Component)openConnections[r];
		string compString = c.ToString();
		//Debug.Log("Connection Name: " + compString);

		// Remove the open connection
		openConnections.RemoveAt(r);

		// Using component string, find which open connection is being used, disable the corresponding wall, and disable all other open connections
		setWalls(compString);

		return c;
	}

	// makes a wall closed/open depending on if there is a room connected to it
	//	(both start as active, then it sets one inactive)
	public void setWalls(string componentName)
	{
		string direction = componentName.Replace("Connection", "");

		//Debug.Log("Direction: " + direction);

		// Strings representing the name of both the wall and entrance given the direction
		string wallName = "Wall " + direction;
		string entranceName = "Entrance " + direction;

		Component[] components = this.GetComponentsInChildren(typeof(Component));
		foreach (Component component in components)
		{
			string compString = component.ToString();

			// If an open entrance is found that isnt the already chosen one the corresponding wall is found, disable it
			if ((compString.Contains("Entrance") && !(compString.Contains(entranceName) || compString.Contains("Entrance Down")))
				|| (compString.Contains(wallName)))
			{
				//Debug.Log("Setting false: " + compString);
				component.gameObject.SetActive(false);
			}
		}
	}

	// Removes down wall unless it is the starting room
	// only to be called on starting room -- sets "down" to be a wall instead of doorway
	public void setDownWall(bool isStartingRoom)
	{
		//Debug.Log("Setting down wall");
		Component[] components = this.GetComponentsInChildren(typeof(Component));
		foreach (Component component in components)
		{
			string compString = component.ToString();

			// Remove the wall if it is not the starting room
			if (compString.Contains("Wall Down") && !isStartingRoom)
			{
				component.gameObject.SetActive(false);
				//Debug.Log("Setting true: " + compString);
			}

			// Remove the down entrance if it is the starting room
			if (compString.Contains("Entrance Down") && isStartingRoom)
			{
				component.gameObject.SetActive(false);
			}
		}
	}
}
