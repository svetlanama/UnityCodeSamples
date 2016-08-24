using UnityEngine;
using System.Collections;
using UnityEngine.VR;

/*
 * This class is responsible for switching between scenes 
 * and between VR/NonVR modes
 */
public class GameManager : MonoBehaviour
{

	private static GameManager _instance;
	public static GameManager Instance { get { return _instance; } }

	private bool? _isVRModeEnabled = null;

	void Awake ()
	{
		_instance = this;
		DontDestroyOnLoad (this.gameObject);
	}

	void OnLevelWasLoaded (int level)
	{
		if (level == 0) {
			_isVRModeEnabled = null;
			GvrViewer.Instance.VRModeEnabled = false;

		} else if (_isVRModeEnabled.HasValue) {
			GvrViewer.Instance.VRModeEnabled = (bool)_isVRModeEnabled;
		}  	
	}

	/// <summary>
	/// Loads the VoiceReport level.
	/// </summary>
	public void LoadVoiceReportLevel ()
	{
		_isVRModeEnabled = GvrViewer.Instance.VRModeEnabled;
		Application.LoadLevel (Constants.VoiceReportScene);
	}

	/// <summary>
	/// Loads the Main level.
	/// </summary>
	public void LoadMainLevel (bool isVRModeOn)
	{
		_isVRModeEnabled = isVRModeOn;
		Application.LoadLevel (Constants.MainScene);
	}

	/// <summary>
	/// Loads the Main level.
	/// </summary>
	public void LoadLaunchUILevel ()
	{
		Application.LoadLevel (Constants.LaunchScene);
	}

	/// <summary>
	/// Gos to start.
	/// </summary>
	public void Exit ()
	{
		PaintingsAudioController.Instance.stopSelectedAudio ();	
		AudioController.Instance.stopAudio ();
		AudioController.Instance.activeBackgroundAudio (false);
		LoadLaunchUILevel ();
	}

}
