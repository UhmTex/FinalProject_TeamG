using System;
using UnityEngine;

namespace _Main.Scripts
{
	[DefaultExecutionOrder(0)]
	public class Plane : MonoBehaviour
	{
		public Material HeightMapMaterial;
		public Material PaintMaterial;
		
		public RenderTexture HeightMapTexture;
		public RenderTexture PaintTexture;
		
		private void Start()
		{
            GameManager.instance.Plane = this;

            var b = GetComponent<Brush>();
			HeightMapTexture = b.heightMapRenderTexture;
			PaintTexture = b.PaintingRenderTexture;
		}

        
    }
}