using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

using Component = UnityEngine.Component;

public class SquareRoom : Room
{
	public SquareRoom()
	{
		this.roomSize = new Vector3(30, 15, 30); // (x-axis size, y-axis size, z-axis size)
	}
}
