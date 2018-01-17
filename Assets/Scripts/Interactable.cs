using UnityEngine;

public class Interactable : MonoBehaviour {

    public enum InteractableType {
        Missionary,
        Cannibal,
        Boat
    }

    public InteractableType interactableType;

    public Color normalColor = Color.white;
    public Color hoverColor = Color.white;

    public bool canInteract = true;

    public Dropzone dropzone;

    private SpriteRenderer spriteRenderer;

    private float colorDelta = 0.1f;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = normalColor;
    }

    private void OnMouseEnter() {
        if(canInteract) {
            spriteRenderer.color = hoverColor;
        }
    }

    private void OnMouseExit() {
        if(canInteract) {
            spriteRenderer.color = normalColor;
        }
    }

    private void OnMouseDown() {
        Dropzone previousDropzone = transform.parent.GetComponent<Dropzone>();

        if(canInteract && dropzone != null) {
            dropzone.AddItem(gameObject);
            dropzone = previousDropzone;
        }
    }
}
