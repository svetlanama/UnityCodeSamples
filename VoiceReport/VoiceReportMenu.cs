using UnityEngine;
using System.Collections;
using TMPro;

public enum VoiceReportAction
{
	Recording,
	Sending,
	Finished,
	None
};


public enum VoiceReportSentStatus
{
	Sussess,
	Failed,
	None
};

/*
 * UI Voice report and appropriate functional 
 */
public class VoiceReportMenu : MonoBehaviour
{
	public GameObject bigLogo;
	public GameObject smallLogo;

	public VoiceReportAction currentRecordingAction;
	public VoiceReportSentStatus reportSentStatus;
 
	private GameObject makeReportButton;
	private GameObject endReportButton;
	private GameObject playReportButton;
	private GameObject stopPlayingReportButton;
	private GameObject sendReportButton;
	private GameObject backFromReportButton;
	public TextMeshProUGUI reportLabel;
	public TextMeshProUGUI progressLabel;
	private RecordingProgress recordingProgress;

	private static VoiceReportMenu _instance;
	public static VoiceReportMenu Instance { get { return _instance; } }

	// Use this for initialization
	void Awake ()
	{
		_instance = this;
		recordingProgress       = GetComponent<RecordingProgress> ();
		makeReportButton        = GameObject.FindGameObjectsWithTag (Constants.btnMakeReport) [0] as GameObject;
		endReportButton         = GameObject.FindGameObjectsWithTag (Constants.btnEndReport) [0] as GameObject;
		playReportButton        = GameObject.FindGameObjectsWithTag (Constants.btnPlayReport) [0] as GameObject;
		sendReportButton        = GameObject.FindGameObjectsWithTag (Constants.btnSendReport) [0] as GameObject;
		backFromReportButton    = GameObject.FindGameObjectsWithTag (Constants.btnBackFromReport) [0] as GameObject;
		stopPlayingReportButton = GameObject.FindGameObjectsWithTag (Constants.btnStopPlayingReport) [0] as GameObject;

		progressLabel.text = "";
		SetReportLabelText ("");
		HideAllButtons ();
		ToogleLogo (true);
		DisplayProgressBar (false);
		makeReportButton.GetComponent<FadeUI> ().DoQuickFadeInOrOut (true); 
	}

	public void SetReportLabelText (string text)
	{
		reportLabel.text = text;
	}

	/// <summary>
	/// Toogle betwee logos.
	/// </summary>
	/// <param name="isDisplayBigLogo">If set to <c>true</c> is display big logo.</param>
	public void ToogleLogo (bool isDisplayBigLogo)
	{
		DisplayBigLogo (isDisplayBigLogo);
		DisplaySmallLogo (!isDisplayBigLogo);
	}


	/// <summary>
	/// Display the big logo.
	/// </summary>
	/// <param name="isDisplay">If set to <c>true</c> is display.</param>
	public void DisplayBigLogo (bool isDisplay)
	{
		bigLogo.SetActive (isDisplay);
	}

	/// <summary>
	/// Displaн the small logo.
	/// </summary>
	/// <param name="isDisplay">If set to <c>true</c> is display.</param>
	public void DisplaySmallLogo (bool isDisplay)
	{
		smallLogo.SetActive (isDisplay);
	}

	/// <summary>
	/// Check If menu is shown.
	/// </summary>
	/// <returns><c>true</c>, if menu shown was ised, <c>false</c> otherwise.</returns>
	public bool IsActiveVoiceReportMenu ()
	{
		bool result = gameObject.activeSelf;
		return result;
	}

	/// <summary>
	/// Hides all buttons.
	/// </summary>
	public void HideAllButtons ()
	{
		makeReportButton.GetComponent<FadeUI> ().DoQuickFadeInOrOut (false);
		endReportButton.GetComponent<FadeUI> ().DoQuickFadeInOrOut (false);
		playReportButton.GetComponent<FadeUI> ().DoQuickFadeInOrOut (false);
		sendReportButton.GetComponent<FadeUI> ().DoQuickFadeInOrOut (false);
		stopPlayingReportButton.GetComponent<FadeUI> ().DoQuickFadeInOrOut (false);
	}

	/// <summary>
	/// Display the make record button.
	/// </summary>
	/// <param name="isDisplay">If set to <c>true</c> is display.</param>
	public void DisplayMakeRecordButton (bool isDisplay)
	{
		if (isDisplay) {
			StartCoroutine (DisplayCurrentButton (Constants.btnMakeReport));
		} else {
			makeReportButton.GetComponent<FadeUI> ().DoQuickFadeInOrOut (false);
		}
	}

	/// <summary>
	/// Display the end record button.
	/// </summary>
	/// <param name="isDisplay">If set to <c>true</c> is display.</param>
	public void DisplayEndRecordButton (bool isDisplay)
	{
		if (isDisplay) {
			StartCoroutine (DisplayCurrentButton (Constants.btnEndReport));
		} else {
			sendReportButton.GetComponent<FadeUI> ().DoQuickFadeInOrOut (false);
		}
	}

	/// <summary>
	/// Display the stop playing record button.
	/// </summary>
	/// <param name="isDisplay">If set to <c>true</c> is display.</param>
	public void DisplayStopPlayingRecordButton (bool isDisplay)
	{
		if (isDisplay) {
			StartCoroutine (DisplayCurrentButton (Constants.btnStopPlayingReport));
		} else {
			stopPlayingReportButton.GetComponent<FadeUI> ().DoQuickFadeInOrOut (false);
		}
	}

	/// <summary>
	/// Display the play record button.
	/// </summary>
	/// <param name="isDisplay">If set to <c>true</c> is display.</param>
	public void DisplayPlayRecordButton (bool isDisplay)
	{
		if (isDisplay) {
			//HideAllButtons ();
			StartCoroutine (DisplayCurrentButton (Constants.btnPlayReport));
		} else {
			playReportButton.GetComponent<FadeUI> ().DoQuickFadeInOrOut (false);
		}
	}

	/// <summary>
	/// Display the send record button.
	/// </summary>
	/// <param name="isDisplay">If set to <c>true</c> is display.</param>
	public void DisplaySendRecordButton (bool isDisplay)
	{
		if (isDisplay) {
			StartCoroutine (DisplayCurrentButton (Constants.btnSendReport));
		} else {
			sendReportButton.GetComponent<FadeUI> ().DoQuickFadeInOrOut (false);
		}
	}

	/// <summary>
	/// Display the back button.
	/// </summary>
	/// <param name="isDisplay">If set to <c>true</c> is display.</param>
	public void DisplayBackButton (bool isDisplay)
	{
		makeReportButton.GetComponent<FadeUI> ().DoQuickFadeInOrOut (isDisplay);
	}

	/// <summary>
	/// Show selected button with fadeIn/Put effect.
	/// </summary>
	/// <returns>The current button.</returns>
	/// <param name="tag">Tag.</param>
	public IEnumerator DisplayCurrentButton (string tag)
	{
		yield return new WaitForSeconds (1);
		switch (tag) {

		case Constants.btnMakeReport:
			makeReportButton.GetComponent<FadeUI> ().DoQuickFadeInOrOut (true); 
			break;
		case Constants.btnEndReport:
			endReportButton.GetComponent<FadeUI> ().DoQuickFadeInOrOut (true);
			break;
		case Constants.btnPlayReport:
			playReportButton.GetComponent<FadeUI> ().DoQuickFadeInOrOut (true);
			break;
		case Constants.btnStopPlayingReport:
			stopPlayingReportButton.GetComponent<FadeUI> ().DoQuickFadeInOrOut (true);
			break;
		case Constants.btnSendReport:
			sendReportButton.GetComponent<FadeUI> ().DoQuickFadeInOrOut (true);
			break;

		default:
			break;
		}
	}

	/// <summary>
	/// Display the progress bar of current recording process.
	/// </summary>
	/// <param name="isDisplay">If set to <c>true</c> is display.</param>
	public void DisplayProgressBar (bool isDisplay)
	{
		if (isDisplay) {
			recordingProgress.recordingProgressBar.GetComponent<FadeUI> ().DoQuickFadeInOrOut (true);
			recordingProgress.PerformRecording (0);
		} else {
			recordingProgress.recordingProgressBar.GetComponent<FadeUI> ().DoQuickFadeInOrOut (false);
		}
	}

	/// <summary>
	/// Display the progress sending result.
	/// </summary>
	/// <param name="action">Action.</param>
	/// <param name="sendStatus">Send status.</param>
	/// <param name="recordingProgressValue">Recording progress value.</param>
	public void DisplayProgressResult (VoiceReportAction action, VoiceReportSentStatus sendStatus, float recordingProgressValue)
	{
		currentRecordingAction = action;
		reportSentStatus = sendStatus;
		progressLabel.color = Color.black;
		progressLabel.text = "";
		switch (currentRecordingAction) {

		case VoiceReportAction.Recording:
			progressLabel.text = "recording ... " + (int)recordingProgressValue + " seconds left.";
			float progressBarValue = Mathf.Abs (recordingProgressValue - Constants.voiceReportLenght);
			progressLabel.color = (int)progressBarValue % 2 == 0 ? Color.red : Color.black;
			SetProgressBarValue ((int)progressBarValue);
			break;

		case VoiceReportAction.Sending:
			progressLabel.color = (int)Time.deltaTime % 2 == 0 ? Color.red : Color.black;
			progressLabel.text = "Your report is sending...";
			break;

		case VoiceReportAction.Finished:
			switch (sendStatus) {
	
			case VoiceReportSentStatus.Sussess:
				progressLabel.color = Color.green;
				progressLabel.text = "The report was sent successfully. Thank you!";
				break;

			case VoiceReportSentStatus.Failed:
				progressLabel.color = Color.red;
				progressLabel.text = "Sorry, some error occured. Please try later.";
				break;

			default:
				progressLabel.text = "";
				break;
			}
			break;

		case VoiceReportAction.None:
			progressLabel.text = " ";
			break;

		default:
			progressLabel.text = "";
			break;
		}
	}

	/// <summary>
	/// Sets the progress bar value.
	/// </summary>
	/// <param name="progressBarValue">Progress bar value.</param>
	public void SetProgressBarValue (int progressBarValue)
	{
		recordingProgress.PerformRecording (progressBarValue);
	}
}
