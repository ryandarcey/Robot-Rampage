using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

using Component = UnityEngine.Component;

public class Hallway : Room
{
	public Hallway()
	{
		this.roomSize = new Vector3(20, 12, 32); // (x-axis size, y-axis size, z-axis size)
	}
}
