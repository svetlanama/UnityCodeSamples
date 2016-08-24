using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

/*
 * This class is responsible
 * for counter amination like 5,4,3,2,1 
 */
public class VRCounter : MonoBehaviour {

	public TextMeshProUGUI texCounter;
	public GameObject panelCounter;
	private float _timer = 0;
	private IEnumerator _caroutine;

	private static VRCounter _instance;
	public static VRCounter Instance { get { return _instance; } }

	void Awake() 
	{
		_instance = this;
	}

	// Update is called once per frame
	void Update () 
	{
		if (_timer > 0) {
			_timer -= Time.deltaTime;
			UpdateText ();
			shrink ();
		}
	}

	/// <summary>
	/// Text counter updating.
	/// </summary>
	void UpdateText() {
		texCounter.text = ((int)_timer).ToString();
	}

	/// <summary>
	/// Starts the counter.
	/// </summary>
	public void StartCounter() 
	{
		_caroutine = StartTimerCounter ();
		StartCoroutine (_caroutine);
	}

	/// <summary>
	/// Courutine of starts the timer counter.
	/// </summary>
	/// <returns>The timer counter.</returns>
	public IEnumerator StartTimerCounter() 
	{
		yield return new WaitForSeconds (1.0f); 
		_timer = Constants.VRCounter;
	}

	/// <summary>
	/// Stops the counter.
	/// </summary>
	public void StopCounter() {
		_timer = 0;
		UpdateText ();
		StopCoroutine (_caroutine);
	} 

	/// <summary>
	/// Shrink this instance.
	/// </summary>
	public void shrink() {
		Vector3 v = panelCounter.transform.localScale;
		v.x = _timer/10;
		v.y = _timer/10;
		v.z = _timer/10;
		panelCounter.transform.localScale = v;
	}
}
