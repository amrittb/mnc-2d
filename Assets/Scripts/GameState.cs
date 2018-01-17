using System;
using UnityEngine;

public static class GameState {

    // x - Num missionaries in left side
    // y - Num cannibals in right side
    // z - 1 if boat in left, 0 if boat in right

    public static bool IsValid(this Vector3Int state, Vector3Int initialState) {
        int missionariesLeft = state.x;
        int missionariesRight = (initialState.x - state.x);
        int cannibalsLeft = state.y;
        int cannibalsRight = (initialState.y - state.y);


        return (missionariesLeft >= 0 && missionariesRight >= 0 && cannibalsLeft >= 0 && cannibalsRight >=0
                && (missionariesLeft == 0 || missionariesLeft >= cannibalsLeft)
                && (missionariesRight == 0 || missionariesRight >= cannibalsRight));
    }

    public static Vector3Int AddState(this Vector3Int state, Vector3Int newState) {
        state.x += newState.x;
        state.y += newState.y;
        state.z += newState.z;

        return state;
    }

    public static Vector3Int SubtractState(this Vector3Int state, Vector3Int newState) {
        state.x -= newState.x;
        state.y -= newState.y;
        state.z -= newState.z;

        return state;
    }
}
