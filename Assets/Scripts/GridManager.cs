using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int columns;
    [SerializeField] private Vector2 cellMargin;
    [SerializeField] private Vector2 gridMargin;
    [SerializeField] private GameObject sort;


    void Start()
    {
        if (PlayerPrefs.HasKey("SortCollection") && sort != null)
        {
            sort.GetComponent<Dropdown>().value = PlayerPrefs.GetInt("SortCollection");
        }
    }

    public void SortCollection()
    {
        if (isActiveAndEnabled)
        {
            if (sort != null)
            {
                PlayerPrefs.SetInt("SortCollection", sort.GetComponent<Dropdown>().value);
                StartCoroutine(SortItems());
            }

            StartCoroutine(PositionItems());
        }
    }

    private IEnumerator SortItems()
    {
        yield return null;

        List<GameObject> sortedItems = new();

        foreach (Transform item in transform)
        {
            sortedItems.Add(item.gameObject);
        }

        switch (sort.GetComponentInChildren<Text>().text)
        {
            case "Rarity Ascending":
                sortedItems.Sort((x1, x2) => x1.GetComponent<Collection>().rarity.CompareTo(x2.GetComponent<Collection>().rarity));
                break;
            case "Rarity Descending":
                sortedItems.Sort((x1, x2) => x1.GetComponent<Collection>().rarity.CompareTo(x2.GetComponent<Collection>().rarity));
                sortedItems.Reverse();
                break;
            case "Name Ascending":
                sortedItems.Sort((x1, x2) => x1.GetComponent<Collection>().name.CompareTo(x2.GetComponent<Collection>().name));
                break;
            case "Name Descending":
                sortedItems.Sort((x1, x2) => x1.GetComponent<Collection>().name.CompareTo(x2.GetComponent<Collection>().name));
                sortedItems.Reverse();
                break;
        }

        for (int i = 0; i < sortedItems.Count; i++)
        {
            sortedItems[i].transform.SetSiblingIndex(i);
        }
    }

    private IEnumerator PositionItems()
    {
        yield return null;

        int rows;

        if ((Mathf.CeilToInt(transform.childCount) % columns).Equals(0)) 
        { 
            rows = Mathf.CeilToInt(transform.childCount / columns); 
        }

        else 
        { 
            rows = Mathf.CeilToInt(transform.childCount / columns) + 1; 
        }

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