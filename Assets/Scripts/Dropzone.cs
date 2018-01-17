using System.Collections.Generic;
using UnityEngine;

public class Dropzone : MonoBehaviour {

    public enum RepositionBy {
        None,
        AscendingSortingOrder,
        DescendingSortingOrder
    }

    public Vector2 boundingBoxSize = new Vector2(5f, 2.5f);

    [Range(1,10)]
    public int maxItems;

    public float maxPositionVariance = 0.2f;

    public bool automaticReposition = true;
    public bool flipSprites = false;
    public bool shouldFlip = true;

    public RepositionBy repositionSortingBy;

    public bool visualizeZone = true;

    public void AddItem(GameObject item) {
        if(transform.childCount < maxItems) {
            item.transform.parent = transform;
            item.transform.localPosition = Vector3.zero;

            if(automaticReposition) {
                ReorderItems();
            }

            if(shouldFlip) {
                item.GetComponent<SpriteRenderer>().flipX = flipSprites;
            }
        } else {
            Debug.LogError("Cannot drop more objects. Limit reached.");
        }
    }

    private void ReorderItems() {
        int childCount = transform.childCount;

        Vector2 deltaPosition = boundingBoxSize / (childCount + 1);

        //List<short> childrenIndex = GetSortedChildrenIndices();

        for(short i = 0; i < childCount; i++) {
            transform.GetChild(i).localPosition = (new Vector3(deltaPosition.x, 0f, 0f)) * i - new Vector3(deltaPosition.x, 0f, 0f) * (childCount - 1) / 2;
        }
    }

    // Implementation is not complete.
    private List<short> GetSortedChildrenIndices() {
        List<short> sortedIndexList = new List<short>();

        for(short i = 0; i < transform.childCount; i++) {
            if (sortedIndexList.Count > 0) {
                int currentOrder = transform.GetChild(i).GetComponent<SpriteRenderer>().sortingOrder;

                for (short j = 0; j < sortedIndexList.Count; j++) {
                    int order = transform.GetChild(sortedIndexList[j]).GetComponent<SpriteRenderer>().sortingOrder;

                    if(repositionSortingBy == RepositionBy.AscendingSortingOrder) {
                        if(currentOrder < order) {
                            sortedIndexList.Insert(j, i);
                        }
                    } else if(repositionSortingBy == RepositionBy.DescendingSortingOrder) {
                        if (currentOrder > order) {
                            sortedIndexList.Insert(j + 1, i);
                        }
                    } else {
                        sortedIndexList.Add(i);
                    }
                }
            } else {
                sortedIndexList.Add(i);
            }
        }

        return sortedIndexList;
    }

    private void OnDrawGizmos() {
        if (visualizeZone) {
            Gizmos.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
            Gizmos.DrawCube(transform.position + new Vector3(0f, boundingBoxSize.y / 2, 0f), new Vector3(boundingBoxSize.x, boundingBoxSize.y, 1f));
        }
    }
}
