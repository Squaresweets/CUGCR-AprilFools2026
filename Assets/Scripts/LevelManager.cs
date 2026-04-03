using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private List<LevelSO> levels;
    [SerializeField] private GameObject topAndBottom;
    [SerializeField] private RectTransform previousPuzzle;
    [SerializeField] private RectTransform verifyButton;
    [SerializeField] private TMP_Text captionText;
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private TMP_Text hintText;
    private Level currentLevel;
    private int levelID;
    private int tries;
    private float wobbleTimer;
    private float wobbleTimeStart;

    private void Start()
    {
        levelID = -1; //Start from 0
        NextLevel();
    }
    public void PreviousLevel()
    {
        levelID -= 2; // Goofy
        NextLevel(); 
    }
    public void NextLevel()
    {
        levelID++;

        tries = 0;
        hintText.enabled = false;

        if (currentLevel != null)
        {
            if (levelID == 0 || levels[levelID - 1].hideTopAndBottom || levels[levelID].hideTopAndBottom)
                GameObject.Destroy(currentLevel.gameObject);
            else StartCoroutine(LevelSwitchAnim());
        }

        currentLevel = GameObject.Instantiate(levels[levelID].levelPrefab, transform).GetComponent<Level>();
        captionText.text = levels[levelID].topCaption;
        titleText.text = levels[levelID].boldText;
        hintText.text = levels[levelID].hint == "" ? "" : "Hint:\n" + levels[levelID].hint;

        topAndBottom.SetActive(!levels[levelID].hideTopAndBottom);
    }
    private IEnumerator LevelSwitchAnim()
    {
        RectTransform puzzleShift = previousPuzzle.parent.GetComponent<RectTransform>();
        currentLevel.transform.SetParent(previousPuzzle);
        currentLevel.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        for (int i = 0; i < 10; i++)
        {
            puzzleShift.anchoredPosition = new Vector2(Mathf.Lerp(547, 0, i/10f), 0);
            yield return null;
        }
        GameObject.Destroy(previousPuzzle.GetChild(0).gameObject);
        puzzleShift.anchoredPosition = Vector2.zero;
    }
    public void Verify() //From button
    {
        bool complete = currentLevel.Verify();
        if (complete) NextLevel();
        else
        {
            tries++;
            wobbleTimer = 0.3f;
            wobbleTimeStart = Time.time;
            if (tries > 1) hintText.enabled = true;
        }
    }
    private void Update()
    {
        if (wobbleTimer < 0) { verifyButton.rotation = Quaternion.identity; return; }
        verifyButton.rotation = Quaternion.Euler(0, 0, 60 * wobbleTimer * Mathf.Sin((Time.time - wobbleTimeStart) * 20));
        wobbleTimer -= Time.deltaTime;
    }
}
