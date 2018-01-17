using UnityEngine;

public class LevelSpawner : MonoBehaviour {

    public GameObject missionary;
    public GameObject cannibal;
    public Dropzone boat;

    public Dropzone initialDropzone;

    public float maxAnimationVariance = 0.2f;

    public void SpawnState(Vector3Int spawnState) {
        // Missionaries Spawnning
        for (int i = 1; i <= spawnState.x; i++) {
            GameObject newMissionary = Instantiate(missionary, transform.position, Quaternion.identity);
            newMissionary.GetComponent<SpriteRenderer>().sortingOrder = i + spawnState.x;
            newMissionary.GetComponent<Animator>().speed = 1f + UnityEngine.Random.Range(-maxAnimationVariance, maxAnimationVariance);
            newMissionary.GetComponent<Interactable>().dropzone = boat;


            initialDropzone.AddItem(newMissionary);
        }

        // Cannibal Spawnning
        for (int i = 1; i <= spawnState.y; i++) {
            GameObject newCannibal = Instantiate(cannibal, transform.position, Quaternion.identity);
            newCannibal.GetComponent<SpriteRenderer>().sortingOrder = spawnState.y - i + 1;
            newCannibal.GetComponent<Animator>().speed = 1f + UnityEngine.Random.Range(-maxAnimationVariance, maxAnimationVariance);
            newCannibal.GetComponent<Interactable>().dropzone = boat;

            initialDropzone.AddItem(newCannibal);
        }
    }
}
