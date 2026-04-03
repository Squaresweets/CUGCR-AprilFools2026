using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class SelectGrid : Level
{
    [SerializeField] List<SelectableImage> selectables;
    [SerializeField] SelectableSquare prefab;
    [SerializeField] GridLayoutGroup parent;
    [SerializeField] int squareSize = 3;

    List<SelectableSquare> squares = new List<SelectableSquare>();

    private void Start()
    {
        float squareWidth = parent.GetComponent<RectTransform>().rect.width / squareSize;
        parent.cellSize = new Vector2(squareWidth, squareWidth);

        selectables = selectables.OrderBy(_ => Random.value).ToList();
        for (int i = 0; i < selectables.Count; i++)
        {
            squares.Add(GameObject.Instantiate(prefab, parent.transform));
            squares[^1].image.sprite = selectables[i].image;
            squares[^1].correct = selectables[i].correct;
        }
    }
    public override bool Verify()
    {
        foreach (SelectableSquare s in squares)
            if (!s.IsSelectedCorrectly) return false;
        return true;
    }

    [System.Serializable]
    private struct SelectableImage
    {
        public Sprite image;
        public bool correct;
    }
}
