using UnityEngine;
using System.Collections;

/*
 * This class is responsible for gradually UI appearance 
 * while user performs switching between UI
 */
public class FadeUI : MonoBehaviour {

	public float fadeSpeed = 0.8f;

	/// <summary>
	/// Fades the user interface canvas.
	/// </summary>
	/// <param name="direction">Direction.</param>
	public void FadeUICanvas(int direction) 
	{
		if (direction == -1) {
			StartCoroutine (DoFadeOut ());
		} else {
			StartCoroutine (DoFadeIn ());
		}
	}

	/// <summary>
	/// Perform Fade Out.
	/// </summary>
	/// <returns>The fade out.</returns>
	public IEnumerator DoFadeOut() {
		CanvasGroup canvasGroup = GetComponent<CanvasGroup> ();
		while (canvasGroup.alpha > 0) {
			canvasGroup.alpha -= Time.deltaTime / 2 * fadeSpeed;
			yield return null;
		}
		canvasGroup.blocksRaycasts = false;
		yield return null;
	}

	/// <summary>
	/// Perform Fade In.
	/// </summary>
	/// <returns>The fade in.</returns>
	public IEnumerator DoFadeIn() 
	{
		CanvasGroup canvasGroup = GetComponent<CanvasGroup> ();
		while (canvasGroup.alpha < 1) {
			canvasGroup.alpha += Time.deltaTime / 2 * fadeSpeed;
			yield return null;
		}
		canvasGroup.blocksRaycasts = true;
		yield return null;
	}

	/// <summary>
	/// Do the quick FadeIn/FadeOut.
	/// </summary>
	/// <param name="isVisible">If set to <c>true</c> is visible.</param>
	public void DoQuickFadeInOrOut(bool isVisible)
	{
		CanvasGroup canvasGroup = GetComponent<CanvasGroup> ();
		canvasGroup.alpha = isVisible ? 1 : 0;
		canvasGroup.blocksRaycasts = isVisible;
	}
		
}
