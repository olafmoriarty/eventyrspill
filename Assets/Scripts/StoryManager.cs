using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class StoryManager : MonoBehaviour
{

	[SerializeField] private TextAsset storyJSON;
	private Story story;
	private UIManager ui;
	private string currentMode;
	private Dictionary<string, GameObject> characterObjects = new Dictionary<string, GameObject>();
	private GameObject talkingCharacter;
	private GameObject playerCharacter;
	[SerializeField] private List<string> currentChoices = new List<string>();
	private Dictionary<string, int> currentHotspots = new Dictionary<string, int>();
	private int currentChoice = -1;


	void Awake() {
		story = new Ink.Runtime.Story(storyJSON.text);
		ui = GetComponent<UIManager>();
		playerCharacter = CharacterObjectFromName("ANITA");
	}

	void Start() {
		ContinueStory();
		
	}

	void Update() {
		if (Input.GetButtonDown("Jump")) {
			if (currentChoices.Count > 0 && currentChoice >= 0) {
				MakeChoice(currentChoice);
			}
			else {
				ContinueStory();
			}
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
					string characterName = groups["char"].Value;
					talkingCharacter = CharacterObjectFromName(characterName);
					ui.MoveTextBox(talkingCharacter);
					nextSentence = nextSentence.Substring(nextSentence.IndexOf(':') + 2);
				}
			}

			if (nextSentence == "CUTSCENE" || nextSentence == "SETTINGS") {
				ui.SetDialogueText("");
				ContinueStory();
			}
			else {
				ui.SetDialogueText(nextSentence);
			}
		}
		// Is there a choice to make?
		else if (story.currentChoices.Count > 0) {
			List<string> newChoices = new List<string>();
			Dictionary<string, int> newHotspots = new Dictionary<string, int>();
            for (int i = 0; i < story.currentChoices.Count; i++) {
				
				string choiceText = story.currentChoices[i].text.Trim();
				if (currentMode == "overworld") {
					string[] hotspotArray = choiceText.Split(": ");
					newHotspots.Add(hotspotArray[0], i);
					choiceText = hotspotArray[1];
				}
                newChoices.Add(choiceText);
            }
            currentChoices = newChoices;
			currentHotspots = newHotspots;

			if (currentMode == "conversation") {
				ui.ShowChoices(newChoices);
			}
		}
	}

	public void MakeChoice(int option, bool fromSelect = false) {
		if (option >= 0 && option < story.currentChoices.Count) {
			story.ChooseChoiceIndex(option);
			currentChoice = -1;
			currentChoices = new List<string>();
			currentHotspots = new Dictionary<string, int>();
			if (!fromSelect || !Input.GetButtonDown("Jump")) {
				ContinueStory();
			}
		}

	}

	GameObject CharacterObjectFromName(string characterName) {
		GameObject character;
		if (characterObjects.ContainsKey(characterName)) {
			// Character is already in object list! Get it.
			character = characterObjects[characterName];
		}
		else {
			// Character is not in object list. Set it!
			character = GameObject.Find(characterName);
			characterObjects.Add(characterName, character);
		}
		return character;
	}

	public string GetCurrentMode() {
		return currentMode;
	}

	public GameObject GetPlayerCharacter() {
		return playerCharacter;
	}

	public void SetCurrentHotspot(string colliderName) {
		if (colliderName == "") {
			currentChoice = -1;
			ui.SetHotspotText("");
		}
		else if (currentHotspots.ContainsKey(colliderName)) {
			currentChoice = currentHotspots[colliderName];
			ui.SetHotspotText(currentChoices[currentChoice]);
		}
	}
}
