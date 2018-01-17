using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Dropzone leftBank;
    public Dropzone rightBank;
    public Boat boat;

    public RectTransform gameOverPanel;
    public Text gameOverText;

    private Vector3Int initalState = new Vector3Int(3, 3, 1);
    private Vector3Int goalState = new Vector3Int(0, 0, 0);
    private Vector3Int currentState;

    public LevelSpawner spawner;

    public Brain brain;

    private void Start() {
        currentState = initalState;

        spawner.SpawnState(initalState);

        gameOverPanel.gameObject.SetActive(false);
    }

    private void OnEnable() {
        EventManager.StartListening("BoatBankToggle", BoatBankToggleHandler);
    }

    private void OnDisable() {
        EventManager.StopListening("BoatBankToggle", BoatBankToggleHandler);
    }

    private void BoatBankToggleHandler() {
        Vector3Int boatState = new Vector3Int(0, 0, 1);
        Dropzone nextDropzone = (boat.onLeftBank) ? leftBank : rightBank;

        int childCount = boat.transform.childCount;
        List<Interactable> children = new List<Interactable>();

        for (int i = 0; i < childCount; i++) {
            Interactable interactable = boat.transform.GetChild(i).GetComponent<Interactable>();

            if (interactable.interactableType == Interactable.InteractableType.Missionary) {
                boatState += new Vector3Int(1, 0, 0);
            } else if (interactable.interactableType == Interactable.InteractableType.Cannibal) {
                boatState += new Vector3Int(0, 1, 0);
            }

            children.Add(interactable);
        }

        for(int i = 0; i < children.Count; i++) {
            nextDropzone.AddItem(children[i].gameObject);
            children[i].dropzone = boat.GetComponent<Dropzone>();
        }

        if (boat.onLeftBank) {
            currentState = currentState.AddState(boatState);
        } else {
            currentState = currentState.SubtractState(boatState);
        }

        bool isValid = currentState.IsValid(initalState);
        Debug.Log("Current State: " + currentState.ToString() + " isValid: " + isValid.ToString());

        if(!isValid) {
            Dropzone killzone = (currentState.x < currentState.y) ? leftBank : rightBank;
            KillMissonaries(killzone);
            ShowGameOver(isValid);
        } else if(currentState == goalState) {
            ShowGameOver(isValid);
        }
    }

    private void ShowGameOver(bool goalStateReached) {
        if(goalStateReached) {
            gameOverText.text = "Congratulations! You have cleared.";
        } else {
            gameOverText.text = "Oops! Try again?";
        }

        gameOverPanel.gameObject.SetActive(true);
    }

    private void KillMissonaries(Dropzone killzone) {
        for(int i = 0; i < killzone.transform.childCount; i++) {
            Transform item = killzone.transform.GetChild(i);
            Interactable interactable = item.GetComponent<Interactable>();
            if (interactable.interactableType == Interactable.InteractableType.Missionary) {
                item.GetComponent<Animator>().SetBool(Animator.StringToHash("isDead"),true);
            } else if(interactable.interactableType == Interactable.InteractableType.Cannibal) {
                item.GetComponent<Animator>().SetBool(Animator.StringToHash("shouldAttack"), true);
            }
        }
    }

    public void AutoPlay() {
        brain.Play();
    }

    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
