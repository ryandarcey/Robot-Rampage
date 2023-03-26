using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Room : MonoBehaviour
{
	public EnemyAction enemyPrefab;
	private ArrayList spawnedEnemies = new();

	// TODO: get size from code?
	public Vector3 roomSize;
	public int roomID;
	ArrayList connectionArr = new();
	ArrayList openConnections = new();
	ArrayList doors = new();
	public GameObject downDoor;

	public void Awake()
	{
		Component[] components = this.GetComponentsInChildren(typeof(Component));
		foreach (Component component in components)
		{
			string compString = component.ToString().ToLower();

			if (component.CompareTag("Connection"))
			{
				connectionArr.Add(component);

				if (!compString.Contains("down"))
				{
					// all connections of new room except 'down' are open
					openConnections.Add(component);
				}
			}

			else if (component.CompareTag("Door"))
			{
				doors.Add(component);
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
		/*else if (Math.Abs(connForward.y) > 0.01)
		{
			newPos.y += connForward.y * roomLength / 2;
			//Debug.Log("New Y = " + newPos.y);
		}*/
		else if (Math.Abs(connForward.z) > 0.01)
		{
			newPos.z += connForward.z * roomLength / 2;
			//Debug.Log("New Z = " + newPos.z);
		}

		newPos.x = (float)Math.Round(newPos.x);
		newPos.y = 0f;
		newPos.z = (float)Math.Round(newPos.z);

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
		//setWalls(compString);

		return c;
	}

	// makes a wall closed/open depending on if there is a room connected to it
	//	(both start as active, then it sets one inactive)
	public void SetDoors(Component doorToLeaveOpen, bool isStartingRoom= false)
	{
		string direction;
		if(doorToLeaveOpen != null) 
		{
			direction = doorToLeaveOpen.ToString().Replace("connection_", "");	// TODO: redundant?
			direction.ToLower();
		}
		else
		{
			direction = "NULL";
		}

		//Debug.Log("Direction: " + direction);


		Component[] components = this.GetComponentsInChildren(typeof(Component));
		foreach (Component component in components)
		{
			string compString = component.ToString();

			// leave open provided door + down door, close (set active) other doors
			if (compString.Contains("door") && 
				(compString.Contains(direction) || 
					(compString.Contains("down") && !isStartingRoom)))
			{
				//Debug.Log("Setting false: " + compString);
				component.gameObject.SetActive(false);
			}
			/*else
			{
				component.gameObject.SetActive(true);
			}*/
		}
	}

	// Removes down wall unless it is the starting room
	// only to be called on starting room -- sets "down" to be a wall instead of doorway
	// TODO: redo/fix this method
	/*public void SetDownDoor(bool isStartingRoom=false)
	{
		//Debug.Log("Setting down wall");
		Component[] components = this.GetComponentsInChildren(typeof(Component));
		foreach (Component component in components)
		{
			string compString = component.ToString();

			// Remove the wall if it is not the starting room
			if (compString.Contains("door_down") && isStartingRoom)
			{
				component.gameObject.SetActive(true);
				//Debug.Log("Setting true: " + compString);
			}
		}
	}*/

	public void SetDownDoorActive()
	{
		downDoor.SetActive(true);
	}

	// FOR NOW, SHOULD ONLY BE CALLED ON SQUARE ROOMS
	// TODO: redo this method
	public void SpawnEnemies(GameObject[] robotPrefabs)
	{
		int enemyType = 0;
		Component[] components = this.GetComponentsInChildren(typeof(Component));
		foreach (Component component in components)
		{
			string compString = component.ToString();

			if (compString.Contains("enemy_spawn"))
			{
				Vector3 pos = component.transform.position;

				//int random = UnityEngine.Random.Range(0, 3);
				GameObject enemy = Instantiate(robotPrefabs[enemyType]);
				enemyType++;
				spawnedEnemies.Add(enemy);
				enemy.transform.position = pos;
			}
		}
	}

	public void DespawnEnemies()
	{
		foreach (GameObject enemy in spawnedEnemies)
		{
			Destroy(enemy);
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.CompareTag("Player"))
		{
			FindObjectOfType<LevelGenerator>().RoomEntered(roomID);
		}
	}
}
