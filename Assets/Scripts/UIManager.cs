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

    private List<GameObject> buttons = new List<GameObject>();

    public void MoveTextBox(GameObject character) {
		// Get the transform of the text box so we can move it around
		RectTransform textBoxTransform = textBox.GetComponent<RectTransform>();

		Vector2 viewportPosition = mainCamera.WorldToViewportPoint(character.transform.position + new Vector3(0, 3f, 0));
        Vector2 screenPosition = new Vector2((viewportPosition.x * uiCanvas.sizeDelta.x) - (uiCanvas.sizeDelta.x * 0.5f), (viewportPosition.y * uiCanvas.sizeDelta.y) - (uiCanvas.sizeDelta.y * 0.5f));
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
        }
        choicesBox.GetComponent<RectTransform>().SetSizeWithCurrentAnchors( RectTransform.Axis.Vertical, choices.Count * 50 + 10 );
    }
}
