using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class Matching : Level
{
    [SerializeField] List<MatchingPair> pairs;
    [SerializeField] SelectableSquare prefab;
    [SerializeField] GameObject lrPrefab;
    [SerializeField] Transform left;
    [SerializeField] Transform right;

    List<string> shuffledNames;
    List<int> currentSelection = new List<int>();
    List<RectTransform> lineRenderers = new List<RectTransform>();
    List<SelectableSquare> leftObjects = new List<SelectableSquare>();
    List<SelectableSquare> rightObjects = new List<SelectableSquare>();

    SelectableSquare currentlyPressed = null;
    private void Start()
    {
        shuffledNames = pairs.Select(p => p.name).OrderBy(_ => Random.value).ToList();

        for (int i = 0; i < pairs.Count; i++)
        {
            int index = i;
            leftObjects.Add(GameObject.Instantiate(prefab, left));
            leftObjects[^1].image.sprite = pairs[i].image;
            leftObjects[^1].GetComponent<Button>().onClick.AddListener(() => LeftImagePressed(index));

            rightObjects.Add(GameObject.Instantiate(prefab, right));
            rightObjects[^1].text.text = shuffledNames[i];
            rightObjects[^1].GetComponent<Button>().onClick.AddListener(() => RightImagePressed(index));

            currentSelection.Add(-1);

            lineRenderers.Add(GameObject.Instantiate(lrPrefab, transform).GetComponent<RectTransform>());
        }
    }

    private void LeftImagePressed(int index)
    {
        if (currentlyPressed == null)
        {
            // clear any existing connection from this left node
            if (currentSelection[index] != -1)
            {
                currentSelection[index] = -1;
                RenderLines();
                return;
            }

            currentlyPressed = leftObjects[index];
            currentlyPressed.Select();
        }
        else
        {
            // if previous press was on RIGHT, create link
            if (rightObjects.Contains(currentlyPressed))
            {
                int rightIndex = rightObjects.IndexOf(currentlyPressed);

                // remove any previous connection to this right node
                for (int i = 0; i < currentSelection.Count; i++)
                {
                    if (currentSelection[i] == rightIndex)
                        currentSelection[i] = -1;
                }

                // set new connection
                currentSelection[index] = rightIndex;
            }

            currentlyPressed = null;
            DeSelectAll();
        }

        RenderLines();
    }
    private void RightImagePressed(int index)
    {
        if (currentlyPressed == null)
        {
            // clear any existing connection to this right node
            for (int i = 0; i < currentSelection.Count; i++)
            {
                if (currentSelection[i] == index)
                {
                    currentSelection[i] = -1;
                    RenderLines();
                    return;
                }
            }

            currentlyPressed = rightObjects[index];
            currentlyPressed.Select();
        }
        else
        {
            // if previous press was on LEFT, create link
            if (leftObjects.Contains(currentlyPressed))
            {
                int leftIndex = leftObjects.IndexOf(currentlyPressed);

                // remove any previous connection to this right node
                for (int i = 0; i < currentSelection.Count; i++)
                {
                    if (currentSelection[i] == index)
                        currentSelection[i] = -1;
                }

                // set new connection
                currentSelection[leftIndex] = index;
            }

            currentlyPressed = null;
            DeSelectAll();
        }

        RenderLines();
    }
    public override bool Verify()
    {
        for (int i = 0; i < currentSelection.Count; i++)
        {
            int selection = currentSelection[i];
            if (selection == -1) return false;
            MatchingPair p = pairs[i];
            if (p.name != shuffledNames[selection]) return false;
        }
        return true;
    }

    private void DeSelectAll()
    {
        foreach (SelectableSquare s in leftObjects) s.DeSelect();
        foreach (SelectableSquare s in rightObjects) s.DeSelect();
    }
    private void RenderLines()
    {
        foreach (RectTransform r in lineRenderers) r.localScale = Vector3.zero;
        for (int i = 0; i < currentSelection.Count; i++)
        {
            if (currentSelection[i] == -1) continue;
            StretchBetween(lineRenderers[i], leftObjects[i].transform.position + new Vector3(55f, 0, 0), rightObjects[currentSelection[i]].transform.position + new Vector3(-55, 0, 0));
        }
    }
    private void StretchBetween(RectTransform line, Vector2 a, Vector2 b)
    {
        line.localScale = Vector3.one;
        RectTransform parent = line.parent as RectTransform;

        Vector2 localA, localB;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parent,
            RectTransformUtility.WorldToScreenPoint(null, a),
            null,
            out localA
        );
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parent,
            RectTransformUtility.WorldToScreenPoint(null, b),
            null,
            out localB
        );

        Vector2 dir = localB - localA;
        float length = dir.magnitude;

        line.anchoredPosition = localA + dir * 0.5f;

        float thickness = 4f;
        line.sizeDelta = new Vector2(length, thickness);

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        line.localRotation = Quaternion.Euler(0f, 0f, angle);
    }

    [System.Serializable]
    private struct MatchingPair
    {
        public Sprite image;
        public string name;
    }
}
