using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

using Component = UnityEngine.Component;

public class TConnection : Room
{
	public TConnection()
	{
		this.roomSize = new Vector3(30, 15, 30); // (x-axis size, y-axis size, z-axis size)
	}
}
