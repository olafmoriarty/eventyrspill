using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
	[SerializeField] private GameObject textBox;
	[SerializeField] private GameObject hotspotBox;
	[SerializeField] private Camera mainCamera;
	[SerializeField] private RectTransform uiCanvas;
	[SerializeField] private GameObject choicesBox;
    [SerializeField] private GameObject buttonPrefab;
    private StoryManager storyManager;
    private List<GameObject> buttons = new List<GameObject>();

    void Awake() {
        storyManager = GetComponent<StoryManager>();
    }

    public void MoveTextBox(GameObject character) {
		// Get the transform of the text box so we can move it around
		RectTransform textBoxTransform = textBox.GetComponent<RectTransform>();

        // First, set text box position to character position
		Vector2 viewportPosition = mainCamera.WorldToViewportPoint(character.transform.position + new Vector3(0, 2f, 0));
        Vector2 screenPosition = new Vector2((viewportPosition.x * uiCanvas.sizeDelta.x) - (uiCanvas.sizeDelta.x * 0.5f), (viewportPosition.y * uiCanvas.sizeDelta.y) - (uiCanvas.sizeDelta.y * 0.5f));

        // Then, move textbox if it's outside the screen
        if (screenPosition.x < (textBoxTransform.sizeDelta.x - uiCanvas.sizeDelta.x) / 2) {
            screenPosition.x = (textBoxTransform.sizeDelta.x - uiCanvas.sizeDelta.x) / 2;
        }
        else if (screenPosition.x > (uiCanvas.sizeDelta.x - textBoxTransform.sizeDelta.x) / 2) {
            screenPosition.x = (uiCanvas.sizeDelta.x - textBoxTransform.sizeDelta.x) / 2;
        }
        if (screenPosition.y < (textBoxTransform.sizeDelta.y - uiCanvas.sizeDelta.y) / 2) {
            screenPosition.y = (textBoxTransform.sizeDelta.y - uiCanvas.sizeDelta.y) / 2;
        }
        else if (screenPosition.y > (uiCanvas.sizeDelta.y - textBoxTransform.sizeDelta.y) / 2) {
            screenPosition.y = (uiCanvas.sizeDelta.y - textBoxTransform.sizeDelta.y) / 2;
        }

        // Finally, actually set text box position to created vector2
        textBoxTransform.anchoredPosition = screenPosition;

	}

    public void SetDialogueText(string text) {
        textBox.GetComponent<TMPro.TextMeshProUGUI>().text = text;
    }

    public void SetHotspotText(string text) {
        hotspotBox.GetComponent<TMPro.TextMeshProUGUI>().text = text;
    }

    public void ShowChoices(List<string> choices) {
        choicesBox.SetActive(true);
        buttons = new List<GameObject>();
        for (int i = 0; i < choices.Count; i++) {
            GameObject newButton = Instantiate(buttonPrefab, choicesBox.transform);
            newButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0 - (i * 50 + 30));
            newButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = choices[i];
            int buttonNumber = i;
            newButton.GetComponent<Button>().onClick.AddListener(() => ClickButton(buttonNumber));
            buttons.Add(newButton);
        }
        if (choices.Count > 0) {
            buttons[0].GetComponent<Button>().Select();
        }
        choicesBox.GetComponent<RectTransform>().SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical, choices.Count * 50 + 10 );
    }

    void ClickButton(int buttonNumber) {
        storyManager.MakeChoice(buttonNumber, true);
        choicesBox.SetActive(false);
        foreach (GameObject child in buttons) {
            Destroy(child);
        }
    }

    public bool ChoicesBoxActive() {
        return choicesBox.activeSelf;
    }
}
