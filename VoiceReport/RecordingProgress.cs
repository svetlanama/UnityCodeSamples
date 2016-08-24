using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/*
 * Displaing audio recirding progress 
 */
public class RecordingProgress : MonoBehaviour {

	public Scrollbar recordingProgressBar;
	public float progress = 0f;
 
	public void PerformRecording(float value) {

		progress = 100 * value / Constants.voiceReportLenght;
		recordingProgressBar.size = progress / 100f; 
	}
}
