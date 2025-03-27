using UnityEngine;

namespace TextureCompare
{
	public static class TextureComparer
	{
		public enum DifficultyLevel
		{
			Easy,
			Medium,
			Hard
		}

		public static float CompareTextures(Texture2D referenceTexture, Texture2D userTexture, DifficultyLevel difficulty)
		{
			Debug.Log("Comparing Textures with " + difficulty);
			
			if (referenceTexture == null || userTexture == null)
			{
				Debug.LogError("One or both textures are null");
				return 0f;
			}
			
			Texture2D processedReferenceTexture = ApplyDifficultyResolution(referenceTexture, difficulty);

			if (processedReferenceTexture.width != userTexture.width || processedReferenceTexture.height != userTexture.height)
			{
				userTexture = ResizeTexture(userTexture, processedReferenceTexture.width, processedReferenceTexture.height);
			}

			Color[] referencePixels = processedReferenceTexture.GetPixels();
			Color[] userPixels = userTexture.GetPixels();

			float totalDifference = 0f;
			int totalPixels = referencePixels.Length;

			for (int i = 0; i < totalPixels; i++)
			{
				float pixelDifference = CalculateColorDifference(referencePixels[i], userPixels[i]);
				totalDifference += pixelDifference;
			}

			float avgDifference = totalDifference / totalPixels;

			float similarityScore = 1f - avgDifference;
			
			if (processedReferenceTexture != referenceTexture)
			{
				Object.Destroy(processedReferenceTexture);
			}

			return similarityScore;
		}

		private static Texture2D ApplyDifficultyResolution(Texture2D originalTexture, DifficultyLevel difficulty)
		{
			switch (difficulty)
			{
				case DifficultyLevel.Easy:
					return ResizeTexture(originalTexture, 2, 2);
				
				case DifficultyLevel.Medium:
					return ResizeTexture(originalTexture, 4, 4);
					
				case DifficultyLevel.Hard:
					return ResizeTexture(originalTexture, 8, 8);;
					
				default:
					return originalTexture;
			}

		}

		private static float CalculateColorDifference(Color color1, Color color2)
		{
			float rDiff = Mathf.Abs(color1.r - color2.r);
			float gDiff = Mathf.Abs(color1.g - color2.g);
			float bDiff = Mathf.Abs(color1.b - color2.b);

			float weightedDiff = (rDiff + gDiff + bDiff) / 3f;

			return weightedDiff;
		}

		private static Texture2D ResizeTexture(Texture2D source, int targetWidth, int targetHeight)
		{
			RenderTexture rt = RenderTexture.GetTemporary(targetWidth, targetHeight);
			RenderTexture.active = rt;

			Graphics.Blit(source, rt);

			Texture2D result = new Texture2D(targetWidth, targetHeight);

			result.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
			result.Apply();

			RenderTexture.active = null;
			RenderTexture.ReleaseTemporary(rt);

			return result;
		}
	}
}