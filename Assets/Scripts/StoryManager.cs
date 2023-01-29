using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class StoryManager : MonoBehaviour
{

    public TextAsset storyJSON;
    private Story story;

    void Awake() {
        story = new Ink.Runtime.Story(storyJSON.text);
    }

    void Update() {
        if (Input.GetButtonDown("Jump")) {
            ContinueStory();
        }
    }

    public void ContinueStory() {
        if (story.canContinue) {
            Debug.Log( story.Continue().Trim() );
        }
    }


}
