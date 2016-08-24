using UnityEngine;
using System.Collections;

/*
 * This class is responsible for Screen fading effect
 * from black to normal 
 */
public class FadeScreen : MonoBehaviour
{

	public Texture2D fadeOutTexture;
	public float fadeSpeed = 0.8f;

	private bool isInitialFading = false;
	private bool isNeedToFade = false;
	private int drawDepth = -1000; // texture's  order in the draw hierarchy: a low number means it renders on top
	private float alpha = 1.0f;
	private float fadeDir = -0.5f; // the direction of fade in = 1 or out = -1

	void OnGUI ()
	{
	    alpha += fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01 (alpha);

		if (isInitialFading) {
			GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, 0.8f);
		} else {
			GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		}
			GUI.depth = drawDepth;
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeOutTexture); 
	}

	/// <summary>
	/// Begins the fade.
	/// </summary>
	/// <returns>The fade.</returns>
	/// <param name="direction">Direction.</param>
	public float BeginFade (float direction, bool isInitialFade)
	{
		alpha = 0.8f;
		isInitialFading = isInitialFade;
		isNeedToFade = true;
		fadeDir = direction;
		return(fadeSpeed);
	}
}
