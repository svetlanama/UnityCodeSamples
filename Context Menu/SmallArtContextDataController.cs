using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;

/*
 * This class responsible for getting text from JSON 
 * grab properly and put it to the context(popup) menu
 */
public class SmallArtContextDataController : MonoBehaviour
{

	public TextMeshProUGUI textmeshProGUI;
	private static SmallArtContextDataController _instance;
	public static SmallArtContextDataController Instance { get { return _instance; } }

	void Awake ()
	{
		_instance = this;
	}

	/// <summary>
	/// Put description of selected Art to the menu.
	/// </summary>
	/// <param name="selectedObject">Selected object.</param>
	public void DisplayContextText (Art selectedObject)
	{
		string title = "";
		string artist = "";
		string artist2 = "";
		string year = "";
		string medium = "";
		string description = "";

		artist = "<size=90%><b>" + selectedObject.artist + "</b></size> ";
		artist2 = "<size=80%><b>" + selectedObject.artist2 + "</b></size> ";
		title = "<size=80%><b>" + selectedObject.title + "</b></size> ";  
		medium = "<size=70%>" + selectedObject.medium + "</size>"; 
		medium += "<size=70%>" + ", " + selectedObject.year + "</size>";
		description = "<size=75%>" + selectedObject.description + "</size>";

		string str = "";
		if (selectedObject.artist != "") {
			str += artist + "\n";
		}
		if (selectedObject.artist2 != "") {
			str += artist2 + "\n";
		}
		if (selectedObject.title != "") {
			str += title + "\n";
		}
		if (selectedObject.medium != "" || selectedObject.year != "") {
			str += medium + "\n";
		}
		if (selectedObject.description != "") {
			str += description;
		} 
		textmeshProGUI.text = str;
	} 
}
