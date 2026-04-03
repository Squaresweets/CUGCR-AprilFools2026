using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectableSquare : MonoBehaviour
{
    [SerializeField] private RectTransform scaleTransform;
    public TMP_Text text;
    public Image image;
    public bool undoWhenPressedTwice;

    bool selected;

    public bool correct;

    public void Select()
    {
        if (undoWhenPressedTwice) selected = !selected;
        else selected = true;

        SetScale();
    }
    public void DeSelect()
    {
        selected = false;
        SetScale();
    }
    public bool IsSelectedCorrectly => correct == selected;
    private void SetScale() => scaleTransform.localScale = Vector3.one * (selected ? 0.9f : 1f);
}
