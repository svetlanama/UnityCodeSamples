using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/*
 * Handling PointerEnter/PointerExit of menu items
 * Change apearance depends on menuItem state
 * Change gaze state to appropriate
 */
public class VRItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {

	public enum ButtonState
	{
		Normal,
		Hover,
		Selected
	};

	private IEnumerator clickCoroutine;
	private bool isReadyToClick;
	private GameObject pointer;

	public TextMeshProUGUI textmeshProGUI;
	private Button theButton;
	public ButtonState buttonState {
		set {
			switch (value) {

			case ButtonState.Normal:
				if (theButton.tag == Constants.btnBackFromReport) {
					textmeshProGUI.color = Color.black;
				} else {
					textmeshProGUI.color = Color.white;
				}
				isReadyToClick = false;
				break;

			case ButtonState.Hover:
				if (textmeshProGUI != null) {
					textmeshProGUI.color = new Color (.259f, .197f, .102f, 1f);
				}
				isReadyToClick = true;
				break;

			case ButtonState.Selected:
				if (textmeshProGUI != null) {
					textmeshProGUI.color = Color.yellow;   
				}
				break;

			default:
				break;
			}
		}
	}

	void Awake ()
	{
		theButton = gameObject.GetComponent<Button>();
		buttonState = ButtonState.Normal;
		pointer = GameObject.FindGameObjectsWithTag (Constants.goPointer) [0] as GameObject;
	}

	/// <summary>
	/// Pointers the click.
	/// </summary>
	/// <returns>The click.</returns>
	/// <param name="seconds">Seconds.</param>
	private IEnumerator pointerClick (int seconds)
	{
		yield return new WaitForSeconds (seconds);

		if (isReadyToClick) {
			clickedButton ();
		}
	}

	/// <summary>
	/// The item is clicked.
	/// </summary>
	public void clickedButton ()
	{
		buttonState = ButtonState.Selected;
		StartCoroutine (performAppropriteAction (1, gameObject.tag));
	}

	/// <summary>
	/// Performs the approprite action.
	/// </summary>
	/// <returns>The approprite action.</returns>
	/// <param name="waitTime">Wait time.</param>
	/// <param name="tag">Tag.</param>
	public IEnumerator performAppropriteAction (float waitTime, string tag)
	{
		yield return new WaitForSeconds (waitTime);

		UIActionController.Instance.performAction (tag);
		buttonState = ButtonState.Normal;
	}

	/// <summary>
	/// Raises the pointer enter event.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerEnter (PointerEventData eventData)
	{
		buttonState = ButtonState.Hover;
		GazeController.Instance.changeGazeActive (true);
		clickCoroutine = pointerClick (3);
		StartCoroutine (clickCoroutine);
	}

	/// <summary>
	/// Raises the pointer exit event.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerExit (PointerEventData eventData)
	{
		buttonState = ButtonState.Normal;
		StopCoroutine (clickCoroutine);
		GazeController.Instance.changeGazeActive (false);
	}
}
