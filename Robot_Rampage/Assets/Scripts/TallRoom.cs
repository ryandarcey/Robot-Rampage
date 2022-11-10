using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

using Component = UnityEngine.Component;

public class TallRoom : Room
{
	public TallRoom()
	{
		this.roomSize = new Vector3(30, 32, 30); // (x-axis size, y-axis size, z-axis size)
	}
}
