using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControls : MonoBehaviour
{
    // The movement speed of the character
    [SerializeField] private float horizontalSpeed = 5.0f;
    [SerializeField] private float verticalSpeed = 5.0f;
    private StoryManager storyManager;
    private Vector2 movement;
    private Rigidbody2D rb;

    void Awake() {
        storyManager = GameObject.Find("/GameManager").GetComponent<StoryManager>();
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal") * horizontalSpeed;
        movement.y = Input.GetAxisRaw("Vertical") * verticalSpeed;
        
    }

    void FixedUpdate() {
        // If a character's speed is set, move the character
        if (IsPlayerCharacter()) {
            rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (IsPlayerCharacter()) {
            storyManager.SetCurrentHotspot(col.name);
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        if (IsPlayerCharacter()) {
            storyManager.SetCurrentHotspot("");
        }
    }

    bool IsPlayerCharacter() {
        return storyManager.GetCurrentMode() == "overworld" && storyManager.GetPlayerCharacter() == gameObject;
    }

}
