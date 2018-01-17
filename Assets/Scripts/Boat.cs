using UnityEngine;

public class Boat : Interactable {

    public float boatMovementSpeed = 2f;

    public Vector3 leftBankPosition;
    public Vector3 rightBankPosition;

    public Dropzone leftBankDropzone;
    public Dropzone rightBankDropzone;

    public bool visualizeBankPositions;

    public bool onLeftBank = true;

    private Vector3 nextPosition;
    private Dropzone boatDropzone;
    private bool shouldMove;

    private void Start() {
        Vector3 initialBoatPosition = (onLeftBank) ? leftBankPosition : rightBankPosition;
        transform.position = initialBoatPosition;

        boatDropzone = GetComponent<Dropzone>();

        UpdateNextPosition();
    }

    private void Update() {
        if(shouldMove) {
            if(Mathf.Abs(Vector3.Distance(transform.position, nextPosition)) <= 0.2f) {
                shouldMove = false;
                onLeftBank = !onLeftBank;
                UpdateNextPosition();

                Vector2Int boatState = Vector2Int.zero;

                for(int i = 0; i < transform.childCount; i++) {
                    Interactable interactable = transform.GetChild(i).GetComponent<Interactable>();
                    interactable.dropzone = boatDropzone;
                    interactable.canInteract = true;
                }

                EventManager.TriggerEvent("BoatBankToggle");
            }

            transform.position = Vector3.Lerp(transform.position, nextPosition, Time.deltaTime * 2f);
        }
    }

    private void UpdateNextPosition() {
        nextPosition = (onLeftBank) ? rightBankPosition : leftBankPosition;
        boatDropzone = (onLeftBank) ? leftBankDropzone : rightBankDropzone;
    }

    private void OnMouseDown() {
        if(transform.childCount > 0) {
            shouldMove = true;

            for (int i = 0; i < transform.childCount; i++) {
                transform.GetChild(i).GetComponent<Interactable>().canInteract = false;
            }
        }
    }

    private void OnDrawGizmos() {
        if (visualizeBankPositions) {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(leftBankPosition, 0.2f);

            Gizmos.color = Color.green;
            Gizmos.DrawSphere(rightBankPosition, 0.2f);
        }
    }
}
