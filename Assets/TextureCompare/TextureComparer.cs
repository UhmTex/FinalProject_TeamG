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

		public static float CompareTextures(Texture2D referenceTexture, Texture2D userTexture,
			DifficultyLevel difficulty)
		{
			if (referenceTexture == null || userTexture == null)
			{
				Debug.LogError("One or both textures are null");
				return 0f;
			}
		
			if (referenceTexture.width != userTexture.width || referenceTexture.height != userTexture.height)
			{
				Debug.LogWarning("Texture dimensions don't match. Resizing user texture for comparison.");
				userTexture = ResizeTexture(userTexture, referenceTexture.width, referenceTexture.height);
			}
		
			Color[] referencePixels = referenceTexture.GetPixels();
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
		
			return ApplyDifficultyModifier(similarityScore, difficulty);
		}
	
		private static float CalculateColorDifference(Color color1, Color color2)
		{
			float rDiff = Mathf.Abs(color1.r - color2.r);
			float gDiff = Mathf.Abs(color1.g - color2.g);
			float bDiff = Mathf.Abs(color1.b - color2.b);
		
			float weightedDiff = (rDiff + gDiff + bDiff) / 3f;

			return weightedDiff;
		}


		private static float ApplyDifficultyModifier(float score, DifficultyLevel difficulty)
		{
			switch (difficulty)
			{
				case DifficultyLevel.Easy:
					return Mathf.Pow(score, 0.7f);
				case DifficultyLevel.Hard:
					return Mathf.Pow(score, 1.3f);
				case DifficultyLevel.Medium:
					return score;
				default:
					return score;
			}
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