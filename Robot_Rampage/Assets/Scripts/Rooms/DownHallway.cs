using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

using Component = UnityEngine.Component;

public class DownHallway : Room
{
	public DownHallway()
	{
		this.roomSize = new Vector3(12, 32, 30); // (x-axis size, y-axis size, z-axis size)
	}
}
