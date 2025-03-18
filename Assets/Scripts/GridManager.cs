using System.Collections;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int columns;
    [SerializeField] private Vector2 cellMargin;
    [SerializeField] private Vector2 gridMargin;

    public void SortCollection()
    {
        if (isActiveAndEnabled)
        {
            StartCoroutine(PositionItems());
        }
    }

    private IEnumerator PositionItems()
    {
        int rows;

        if ((Mathf.CeilToInt(transform.childCount) % 4).Equals(0)) 
        {
            rows = Mathf.CeilToInt(transform.childCount / 4);
        }

        else 
        {
            rows = Mathf.CeilToInt(transform.childCount / 4) + 1;
        }

        yield return new WaitForEndOfFrame();
        
        GetComponent<RectTransform>().sizeDelta = new Vector2(cellMargin.x * columns + gridMargin.x * 2, cellMargin.y * rows + gridMargin.y * 2);

        int j = -1;

        for (int i = 0; i < transform.childCount; i++)
        {
            if ((i % columns).Equals(0))
            {
                j++;
            }

            float xPosition = cellMargin.x * ((i % columns) + 1) - (cellMargin.x / 2) + gridMargin.x;
            float yPosition = -cellMargin.y * ((j % rows) + 1) + (cellMargin.y / 2) - gridMargin.y;

            transform.GetChild(i).transform.localPosition = new Vector3(xPosition, yPosition);
        }
    }
}