using UnityEngine;
using TextureCompare;
using System.Collections.Generic;

public class TextureComparerTest : MonoBehaviour
{
	[Header("Test Configuration")]
	[Tooltip("Select the difficulty level for the texture comparison")]
	[SerializeField]
	private TextureComparer.DifficultyLevel difficultyLevel = TextureComparer.DifficultyLevel.Medium;

	[Header("Results")]
	[SerializeField]
	private bool logResultsToConsole = true;

	// Reference to display textures in the inspector
	[Header("Test Results")]
	[SerializeField]
	private Texture2D referenceTexture;
	[SerializeField]
	private Texture2D comparedTexture;
	[SerializeField]
	private float similarityScore;

	private void Start()
	{
		RunTestsWithBuiltInTextures();
	}

	[ContextMenu("Run Tests")]
	public void RunTestsWithBuiltInTextures()
	{
		Debug.Log("Starting TextureComparer tests with built-in Unity 2022.3 textures...");

		// These textures are guaranteed to exist in Unity 2022.3
		Texture2D whiteTexture = Texture2D.whiteTexture;
		Texture2D blackTexture = Texture2D.blackTexture;
		Texture2D normalTexture = Texture2D.normalTexture;
		Texture2D redTexture = Texture2D.redTexture;
		Texture2D grayTexture = Texture2D.grayTexture;
		Texture2D linearGrayTexture = Texture2D.linearGrayTexture;

		List<Texture2D> textures = new List<Texture2D>
		{
			whiteTexture,
			blackTexture,
			normalTexture,
			redTexture,
			grayTexture,
			linearGrayTexture
		};

		// Name the textures for better logging
		whiteTexture.name = "White";
		blackTexture.name = "Black";
		normalTexture.name = "Normal";
		redTexture.name = "Red";
		grayTexture.name = "Gray";
		linearGrayTexture.name = "LinearGray";

		// Compare every texture with every other texture
		for (int i = 0; i < textures.Count; i++)
		{
			Texture2D tex1 = textures[i];

			// Print header for this reference texture
			Debug.Log($"===== Comparing with reference texture: {tex1.name} =====");

			for (int j = 0; j < textures.Count; j++)
			{
				Texture2D tex2 = textures[j];

				float score = TextureComparer.CompareTextures(tex1, tex2, difficultyLevel);

				string result =
					$"Comparing {tex1.name} to {tex2.name} (Difficulty: {difficultyLevel}): Similarity = {score:P2}";

				if (logResultsToConsole)
				{
					Debug.Log(result);
				}

				// For the last comparison, store the values for the inspector
				if (i == textures.Count - 2 && j == textures.Count - 1)
				{
					referenceTexture = tex1;
					comparedTexture = tex2;
					similarityScore = score;
				}
			}
		}

		Debug.Log("=== Creating procedural test textures for more advanced comparisons ===");
		CreateAndCompareProceduralTextures();
	}

	private void CreateAndCompareProceduralTextures()
	{
		// Create a procedural checkerboard
		Texture2D checkerTexture = CreateCheckerboardTexture(256, 256, 32);
		checkerTexture.name = "Checker32";

		// Create a procedural checkerboard with different tile size
		Texture2D checker16Texture = CreateCheckerboardTexture(256, 256, 16);
		checker16Texture.name = "Checker16";

		// Create a gradient texture
		Texture2D gradientTexture = CreateGradientTexture(256, 256);
		gradientTexture.name = "Gradient";

		List<Texture2D> customTextures = new List<Texture2D>
		{
			checkerTexture,
			checker16Texture,
			gradientTexture,
			Texture2D.whiteTexture
		};

		// Compare procedural textures
		Debug.Log("=== Comparing procedural textures ===");
		for (int i = 0; i < customTextures.Count - 1; i++)
		{
			Texture2D tex1 = customTextures[i];

			for (int j = i + 1; j < customTextures.Count; j++)
			{
				Texture2D tex2 = customTextures[j];

				float score = TextureComparer.CompareTextures(tex1, tex2, difficultyLevel);

				Debug.Log(
					$"Comparing {tex1.name} to {tex2.name} (Difficulty: {difficultyLevel}): Similarity = {score:P2}");

				// Store the last comparison for the inspector
				referenceTexture = tex1;
				comparedTexture = tex2;
				similarityScore = score;
			}
		}
	}

	private Texture2D CreateCheckerboardTexture(int width, int height, int tileSize)
	{
		Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				bool isEven = ((x / tileSize) + (y / tileSize)) % 2 == 0;
				texture.SetPixel(x, y, isEven ? Color.black : Color.white);
			}
		}

		texture.Apply();
		return texture;
	}

	private Texture2D CreateGradientTexture(int width, int height)
	{
		Texture2D texture = new Texture2D(width, height, TextureFormat.RGBA32, false);

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				float r = (float)x / width;
				float g = (float)y / height;
				texture.SetPixel(x, y, new Color(r, g, 0.5f));
			}
		}

		texture.Apply();
		return texture;
	}
}