using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class StoryManager : MonoBehaviour
{

	public TextAsset storyJSON;
	private Story story;
	private GameObject textBox;
	private Camera mainCamera;
	private RectTransform uiCanvas;
	private string currentMode;
	private Dictionary<string, GameObject> characterObjects = new Dictionary<string, GameObject>();
	private GameObject activeCharacter;


	void Awake() {
		story = new Ink.Runtime.Story(storyJSON.text);
		textBox = GameObject.Find("/UI/Dialogue");
		mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
		uiCanvas = GameObject.Find("/UI").GetComponent<RectTransform>();
	}

	void Update() {
		if (Input.GetButtonDown("Jump")) {
			ContinueStory();
		}
	}

	void ContinueStory() {
		if (story.canContinue) {
			string nextSentence = story.Continue().Trim();

			// Set tags
			if (story.currentTags.Count > 0) {
				foreach (string tag in story.currentTags) {
					string[] tagArray = tag.Split(':');
					if (tagArray.Length > 1) {
						if (tagArray[0] == "mode") {
							currentMode = tagArray[1];
						}
					}
				}
			}

			if (currentMode == "conversation") {
				// Check if line contains name of speaker
				Regex rx = new Regex(@"^(?<char>[A-Z]+):");
				MatchCollection matches = rx.Matches(nextSentence);
				if (matches.Count > 0) {
					// It does. Convert that string to a game object.
					GroupCollection groups = matches[0].Groups;
					string charName = groups["char"].Value;
					if (characterObjects.ContainsKey(charName)) {
						// Character is already in object list! Get it.
						activeCharacter = characterObjects[charName];
					}
					else {
						// Character is not in object list. Set it!
						activeCharacter = GameObject.Find(charName);
						characterObjects.Add(charName, activeCharacter);
					}
					MoveTextBox(activeCharacter);
					nextSentence = nextSentence.Substring(nextSentence.IndexOf(':') + 2);
				}
			}

			if (nextSentence == "CUTSCENE" || nextSentence == "SETTINGS") {
				textBox.GetComponent<TMPro.TextMeshProUGUI>().text = "";
				ContinueStory();
			}
			else {
				textBox.GetComponent<TMPro.TextMeshProUGUI>().text = nextSentence;
			}
		}
	}

	void MoveTextBox(GameObject character) {
		// Get the transform of the text box so we can move it around
		RectTransform textBoxTransform = textBox.GetComponent<RectTransform>();

		Vector2 viewportPosition = mainCamera.WorldToViewportPoint(character.transform.position + new Vector3(0, 3f, 0));
        Vector2 screenPosition = new Vector2((viewportPosition.x * uiCanvas.sizeDelta.x) - (uiCanvas.sizeDelta.x * 0.5f), (viewportPosition.y * uiCanvas.sizeDelta.y) - (uiCanvas.sizeDelta.y * 0.5f));
        textBoxTransform.anchoredPosition = screenPosition;
	}
}
