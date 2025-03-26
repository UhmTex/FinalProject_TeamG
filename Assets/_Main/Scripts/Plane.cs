using System;
using UnityEngine;

namespace _Main.Scripts
{
	public class Plane : MonoBehaviour
	{
		public Material HeightMapMaterial;

		private void Start()
		{
			HeightMapMaterial = GetComponent<Brush>().heightMapMaterial;
			
			GameManager.instance.Plane = this;
		}
	}
}