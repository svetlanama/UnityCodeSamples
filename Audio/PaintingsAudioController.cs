using UnityEngine;
using System.Collections;

/*
* This class is responsible for playing the audio of each painting
*/
public class PaintingsAudioController : MonoBehaviour
{
	
	private AudioSource audioSource;
	private float scaledRate;
	private bool isStopping;

	private float minVolumeValue = Constants.minVolume;
	private float maxVolumeValue = Constants.maxVolume;
	private float waitingTime = Constants.waitingTime;
	private int _currentPlayingArtAudioId;
	private AudioCrossFadingManager audioCrossFadingManager; 

	public int currentPlayingArtAudioId {
		get { return _currentPlayingArtAudioId; }
		set { _currentPlayingArtAudioId = value; }
	}

	private static PaintingsAudioController _instance;
	public static PaintingsAudioController Instance { get { return _instance; } }

	// Use this for initialization
	void Start ()
	{
		_instance = this;
		audioCrossFadingManager = AudioCrossFadingManager.Instance;
		isStopping = false;
		audioSource = GetComponent<AudioSource> ();
		scaledRate = Constants.crossFadeRate * Constants.musicVolume;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (audioSource.volume < maxVolumeValue && !isStopping && !audioCrossFadingManager.audioCrossFading) {
			audioSource.volume += scaledRate * Time.deltaTime;
		}

		if (audioSource.clip != null && !audioSource.isPlaying && !isStopping && !audioCrossFadingManager.audioCrossFading) {
			resumeGenericAudio();
			resetCurrentPlayingAudio ();
		}

		if (isStopping && audioSource.clip != null && !audioCrossFadingManager.audioCrossFading) {
			audioSource.volume -= scaledRate * Time.deltaTime;
		}
	}

	/// <summary>
	/// Play/Stop selected audio.
	/// </summary>
	public void playAudio(GameObject hitObject)
	{
		Art selectedArt = hitObject.GetComponent<Art>().getArt();
		if (selectedArt.id == currentPlayingArtAudioId && isAudioPlaying())
		{
			stopSelectedAudio();
		} else	{
			playSelectedAudio(selectedArt);
		}
	}

	/// <summary>
	/// Load selected audio from file and play.
	/// </summary>
	/// <param name="selectedArt">Selected art.</param>
	public void playSelectedAudio (Art selectedArt)
	{
		if (selectedArt.audioFile != "") {
			string extension = System.IO.Path.GetExtension (selectedArt.audioFile);
			string result = selectedArt.audioFile.Substring (0, selectedArt.audioFile.Length - extension.Length);
			AudioClip clip = Resources.Load ("Audio/" + selectedArt.hallName + "/" + result) as AudioClip;
			setUpPlayingAudio (selectedArt.id);
			playAudio (clip);
		} else {
			print ("Sorry, this art does not contain audio!");
		}	
	}

	/// <summary>
	/// Stops the selected audio.
	/// </summary>
	public void stopSelectedAudio (bool isStopGradually = false)
	{
		if (audioSource == null) {
			return;
		}
		if (audioSource.isPlaying) {
			if (isStopGradually) { 
				audioCrossFadingManager.StartAudioCoroutine (stopAudioGradually ());
			} else {
				audioCrossFadingManager.StopAudioCoroutine();
				audioSource.Stop ();
			}
		}
	}

	/// <summary>
	/// Stop audio gradually.
	/// </summary>
	/// <returns>The audio gradually.</returns>
	private IEnumerator stopAudioGradually ()
	{
		audioCrossFadingManager.ActiveAudioCrossFading (true);
		isStopping = true;
		yield return new WaitForSeconds (waitingTime);

		audioSource.Stop ();
		isStopping = false;
		audioCrossFadingManager.ActiveAudioCrossFading (false);
	}

	/// <summary>
	/// Play audio gradually.
	/// </summary>
	/// <param name="clip">Clip.</param>
	private IEnumerator playAudioGradually (AudioClip clip)
	{
		audioCrossFadingManager.ActiveAudioCrossFading (true);
		yield return new WaitForSeconds (waitingTime);
		audioSource.clip = clip;
		audioSource.volume = minVolumeValue;
		audioSource.Play ();
		audioCrossFadingManager.ActiveAudioCrossFading (false);
	}

	/// <summary>
	/// Play the audio.
	/// </summary>
	/// <param name="clip">Clip.</param>
	private void playAudio (AudioClip clip)
	{
		if (audioCrossFadingManager.audioCrossFading) {
			return;
		}

		if (AudioController.Instance.isAudioPlaying ()) {
			AudioController.Instance.pauseAudio ();
		}

		if (isAudioPlaying ()) {
			stopSelectedAudio ();
		}

		audioCrossFadingManager.StartAudioCoroutine (playAudioGradually (clip));
	}

	/// <summary>
	/// Is the audio playing.
	/// </summary>
	/// <returns><c>true</c>, if audio is playing ,<c>false</c> otherwise.</returns>
	public bool isAudioPlaying ()
	{
	   return audioSource.isPlaying;
	}

	/// <summary>
	/// Reset current playing audio.
	/// </summary>
	private void resetCurrentPlayingAudio ()
	{
		currentPlayingArtAudioId = -1;
		audioSource.clip = null;
	}

	/// <summary>
	/// Set up playing audio.
	/// </summary>
	private void setUpPlayingAudio (int artId)
	{
		currentPlayingArtAudioId = artId;
	}

	/// <summary>
	/// Resume main background audio.
	/// </summary>
	private void resumeGenericAudio ()
	{
		if (audioCrossFadingManager.audioCrossFading) {
			return;       
		}	
		audioCrossFadingManager.StartAudioCoroutine (resumeGenericAudioGradually());
	}

	/// <summary>
	/// Resume main background audio gradually.
	/// </summary>
	/// <returns>The main background audio.</returns>
	private IEnumerator resumeGenericAudioGradually ()
	{
		audioCrossFadingManager.ActiveAudioCrossFading (true);
		yield return new WaitForSeconds (waitingTime);
		AudioController.Instance.resumeAudio ();
		audioCrossFadingManager.ActiveAudioCrossFading (false);
	}
}
