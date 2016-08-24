using UnityEngine;
using System.Collections;

/*
 * This class allows to manage audio playing using 
 * crossfading for playing/stopping audio gradually
 */
public class AudioCrossFadingManager: MonoBehaviour {

	private bool _audioCrossFading;
	public bool audioCrossFading {
		get { return _audioCrossFading; }
	}
	private IEnumerator audioCoroutine;

	private static AudioCrossFadingManager _instance;
	public static AudioCrossFadingManager Instance { get { return _instance; } }

	void Awake ()
	{
		_instance = this;
		_audioCrossFading = false;
	}

	public void StopAudioCoroutine() {
		if (audioCoroutine == null) {
			return;
		}
		StopCoroutine (audioCoroutine);
	}

	public void ActiveAudioCrossFading(bool isActive) {
		_audioCrossFading = isActive;
	}

	public void StartAudioCoroutine(IEnumerator coroutine) {
		StopAudioCoroutine();
		audioCoroutine = coroutine;
		StartCoroutine (audioCoroutine);
	}
}
