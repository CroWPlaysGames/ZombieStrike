using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InfoHover : MonoBehaviour
{
    private RectTransform background;
    private Text text;
    private RectTransform info;
    [SerializeField] private RectTransform canvas;

    void Awake()
    {
        background = transform.Find("Background").GetComponent<RectTransform>();
        text = transform.Find("Text").GetComponent<Text>();
        info = transform.GetComponent<RectTransform>();
    }

    void Update()
    {
        info.anchoredPosition = Mouse.current.position.ReadValue();
    }

    private void SetText(string value)
    {
        text.text = value;
        background.sizeDelta = new Vector2(40, text.preferredWidth);
    }
}
