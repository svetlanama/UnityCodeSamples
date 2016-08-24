using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;



 
/*
 * This class is responsible for performing audio recording 
 * and sending by email using Server
 */
public class AudioRecorder : MonoBehaviour
{

	private AudioSource reportSource = null;
	private float clipLength;
	private IEnumerator recordCoroutine;
	private float recordingProgressValue = 0;

	private const string filename = "myreport.wav";
	private const string emailURL = "http://api.com/mail.php";
	private const string email = "gmail@gmail.com"; 

	WWW www;
	private bool _isRecordingProcess;

	public bool isRecordingProcess {
		get { return _isRecordingProcess; }
	}

	private static AudioRecorder _instance;
	public static AudioRecorder Instance { get { return _instance; } }

	void Awake ()
	{
		_instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		_isRecordingProcess = false;
		clipLength = Constants.voiceReportLenght;
		recordingProgressValue = clipLength;
		reportSource = GetComponent<AudioSource> ();
		reportSource.Stop ();
	}

	// Update is called once per frame
	void Update ()
	{
		if (Microphone.IsRecording ("TestMicrophone")) {
			recordingProgressValue -= Time.deltaTime;
			VoiceReportMenu.Instance.DisplayProgressResult (VoiceReportAction.Recording, VoiceReportSentStatus.None, recordingProgressValue);
		}
	}

	/// <summary>
	/// Starts recording.
	/// </summary>
	public void StartRecording ()
	{
		recordingProgressValue = clipLength;
		_isRecordingProcess = true;
		AudioController.Instance.activeVoiceMailAudio (true);
		StartCoroutine (StartRecordingAfterSomeSeconds (AudioController.Instance.VoiceMailAudio.clip.length + 2f)); //Constants.voiceMailAudioLength
	}

	/// <summary>
	/// Starts the recording after a set period of time.
	/// </summary>
	/// <returns>The recording.</returns>
	/// <param name="waitSeconds">Wait seconds.</param>
	public IEnumerator StartRecordingAfterSomeSeconds (float waitSeconds)
	{
		yield return new WaitForSeconds (waitSeconds);

		playBeep ();
		VoiceReportMenu.Instance.DisplayEndRecordButton (true);
		reportSource.clip = Microphone.Start ("", false, (int)clipLength, 22050);  
		recordCoroutine = AutoFinishRecording (clipLength);
		StartCoroutine (recordCoroutine); 
	}

	/// <summary>
	/// Stops the recording.
	/// </summary>
	/// <returns>The recording.</returns>
	/// <param name="waitSeconds">Wait seconds.</param>
	public void StopRecording ()
	{
		ResetProgressRecordingValue ();
		Microphone.End (null);
		if (recordCoroutine != null) {
			StopCoroutine (recordCoroutine);
		}
	}

	/// <summary>
	/// Saves the recording.
	/// </summary>
	public void SaveRecording ()
	{
		RemoveFilesFromDisk ();
		SavWav.Save (filename, reportSource.clip);
	}

	//UI
	public IEnumerator AutoFinishRecording (float waitSeconds)
	{
		yield return new WaitForSeconds (waitSeconds);

		StopRecording ();
		VoiceReportMenu.Instance.HideAllButtons ();
		VoiceReportMenu.Instance.DisplayProgressResult (VoiceReportAction.None, VoiceReportSentStatus.None, 0);
		VoiceReportMenu.Instance.SetReportLabelText ("");
		VoiceReportMenu.Instance.DisplayProgressBar (false);
		VoiceReportMenu.Instance.DisplayPlayRecordButton (true);
		VoiceReportMenu.Instance.DisplaySendRecordButton (true);
	}

	/// <summary>
	/// Zips the audio.
	/// </summary>
	public void zipAudio ()
	{
		var filepath = Path.Combine(Application.persistentDataPath, filename);
		if(!File.Exists(filepath)){
			Debug.LogError(filepath + "is not found!");
				return;
			}

		Debug.Log("zipPath" + zipPath);
		ZipUtil.Zip(zipPath, filepath);
		System.Diagnostics.Process.Start(Path.GetDirectoryName(zipPath));
		if (File.Exists(zipPath))
		{
			Debug.Log("zip created");
		}
	}

	/// <summary>
	/// Plaies the recording.
	/// </summary>
	public void playRecording (bool isPlay)
	{
		if (isPlay) {
			reportSource.Play ();
		} else {
			reportSource.Stop ();
		}
	}

	/// <summary>
	/// Plaies the beep.
	/// </summary>
	public void playBeep ()
	{
		AudioController.Instance.activeBeepAudio (true);
	}

	/// <summary>
	/// Send email.
	/// </summary>
	public void SendEmail ()
	{
		var filepath = Path.Combine (Application.persistentDataPath, filename);
		if (!File.Exists (filepath)) { 
			Finished (VoiceReportSentStatus.Failed);
			return;
		}
		if (!Utils.IsNetworkAvaliable ()) {
			Finished (VoiceReportSentStatus.Failed);
			return;
		}

		StartCoroutine (SendEmailWithAudio ());
	}

	IEnumerator SendEmailWithAudio ()
	{
		// We should only read the screen after all rendering is complete
		yield return new WaitForEndOfFrame ();

		var filepath = Path.Combine (Application.persistentDataPath, filename);

		// Create a Web Form
		WWWForm form = new WWWForm ();
		form.AddField ("email", email);
		
		if (File.Exists (filepath)) {
			var fileInfo = new System.IO.FileInfo (filepath);
			FileStream stream = new FileStream (filepath, FileMode.Open, FileAccess.Read, FileShare.Read);//zipPath
			BinaryReader reader = new BinaryReader (stream);
			form.AddBinaryData ("file", reader.ReadBytes ((int)reader.BaseStream.Length), filename); // zipName
			stream.Close ();
		}

		// Upload to a cgi script
		www = new WWW (emailURL, form);

		yield return www;
		if (!string.IsNullOrEmpty (www.error)) {
			print ("Error Uploading Audio:" + www.error);
			Finished (VoiceReportSentStatus.Failed);
		} else if (www.isDone) {
			print ("Finished Uploading Audio");
			Finished (VoiceReportSentStatus.Sussess);
		} else {
			print ("For some reason the recording hasn't been send.");
			Finished (VoiceReportSentStatus.Failed);
		}
	}

	/// <summary>
	/// Perform appropriate functional after finish sending audio file.
	/// </summary>
	/// <param name="status">Status.</param>
	public void Finished (VoiceReportSentStatus status)
	{
		if (www != null) {
			www.Dispose ();
		}

		RemoveFilesFromDisk ();
		_isRecordingProcess = false;
		VoiceReportMenu.Instance.DisplayBackButton (true);
		VoiceReportMenu.Instance.DisplayMakeRecordButton (true);
		VoiceReportMenu.Instance.DisplayProgressResult (VoiceReportAction.Finished, status, 0);
	}

	/// <summary>
	/// Removes files from disk.
	/// </summary>
	public void RemoveFilesFromDisk ()
	{
		var filepath = Path.Combine (Application.persistentDataPath, filename);
		if (File.Exists (filepath)) {
			File.Delete (filepath);
		}
	}

	/// <summary>
	/// Changes the progress recording status.
	/// </summary>
	/// <param name="isProgressRecording">If set to <c>true</c> is progress recording.</param>
	public void ChangeProgressRecordingStatus (bool isProgressRecording)
	{
		_isRecordingProcess = isProgressRecording;
	}

	/// <summary>
	/// Resets the progress recording value.
	/// </summary>
	public void ResetProgressRecordingValue ()
	{
		recordingProgressValue = 0;
	}

	/// <summary>
	/// Stop recording immediately.
	/// </summary>
	public void ExtraFinishRecording ()
	{ 
		if (www != null) {
			www.Dispose ();
		}
		if (reportSource.isPlaying) {
			reportSource.Stop ();
		}

		StopRecording ();
		RemoveFilesFromDisk ();
		ChangeProgressRecordingStatus (false);
		ResetProgressRecordingValue ();
	}
}
