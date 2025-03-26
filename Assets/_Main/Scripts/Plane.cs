using System;
using UnityEngine;

namespace _Main.Scripts
{
	public class Plane : MonoBehaviour
	{
		public Material HeightMapMaterial;
		public Material PaintMaterial;

		
		private void Start()
		{
			
			GameManager.instance.Plane = this;
		}
	}
}