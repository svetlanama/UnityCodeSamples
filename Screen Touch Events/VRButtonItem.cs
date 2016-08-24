using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;


/*
 * Responsible for controls to be able to
 * handle touch events on iOS/Android screens
 */
public class VRButtonItem : MonoBehaviour, IPointerDownHandler {

	public Sprite normalImage;
	public Sprite selectedImage;
	private Button theButton;

	public ButtonState buttonState {
	    set {
		switch (value) {
		case ButtonState.Normal:
			if (normalImage != null) {
				theButton.image.sprite = normalImage;
			}
			break;

		case ButtonState.Selected:
			if (selectedImage != null) {
				theButton.image.sprite = selectedImage;
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
	}

	/// <summary>
	/// Raises the pointer down event.
	/// </summary>
	/// <param name="eventData">Event data.</param>
	public void OnPointerDown(PointerEventData eventData)
        {
		buttonState = ButtonState.Selected;

		switch (tag) {
		case Constants.btnYes:
			VRSwitchMenuController.Instance.SwitchToVideoUI ();
			VRCounter.Instance.StartCounter ();
			StartCoroutine (LaunchMainScreen());
			break;

		case Constants.btnNo:
			GameManager.Instance.LoadMainLevel (false);
			break;

		default:
			break;
		}
	}

	/// <summary>
	/// Launchs the main screen.
	/// </summary>
	/// <returns>The main screen.</returns>
	public IEnumerator LaunchMainScreen ()
	{
		yield return new WaitForSeconds (Constants.VRModeTimer);

		VRCounter.Instance.StopCounter ();
		GameManager.Instance.LoadMainLevel (true);
	}
}
