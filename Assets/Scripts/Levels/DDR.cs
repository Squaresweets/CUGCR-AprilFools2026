using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DDR : Level
{
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] float speed = 150f;
    [SerializeField] TMP_Text highscoreText;
    [SerializeField] TMP_Text howGoodText;
    [SerializeField] List<RectTransform> lanes;
    [SerializeField] List<float> laneRotations;

    [Header("Hit Distance Thresholds")]
    [SerializeField] float perfectThreshold = 15f;
    [SerializeField] float greatThreshold = 35f;
    [SerializeField] float goodThreshold = 60f;
    [SerializeField] float missThreshold = 100f;

    List<int> line = new List<int>() { 0, 2, 0, 1, 2, 1, 0, 2, 0, 1, 2, 0, 2, 2, 2, 0, 2, 2, 2, 2, 2, 0, 2, 0, 2, 1, 0, 2, 0, 0, 0, 0 };

    int score = 0;

    // We use an array of lists so we can track active arrows PER LANE.
    // Index 0 = Left, Index 1 = Down, Index 2 = Right
    List<RectTransform>[] activeArrowsPerLane;

    private void Start()
    {
        Restart();
    }

    private void Update()
    {
        HandleKeyboardInput();
        MoveAndCheckArrows();
        UpdateUI();
    }

    public void Restart()
    {
        score = 0;
        howGoodText.text = "";

        // If restarting, clean up existing arrows
        if (activeArrowsPerLane != null)
        {
            for (int i = 0; i < activeArrowsPerLane.Length; i++)
            {
                foreach (var arrow in activeArrowsPerLane[i])
                {
                    if (arrow != null)
                        Destroy(arrow.gameObject);
                }
            }
        }

        // Recreate lane lists
        activeArrowsPerLane = new List<RectTransform>[lanes.Count];
        for (int i = 0; i < lanes.Count; i++)
            activeArrowsPerLane[i] = new List<RectTransform>();

        // Spawn arrows
        for (int i = 0; i < line.Count; i++)
        {
            int lane = line[i];
            RectTransform newArrow = Instantiate(arrowPrefab, lanes[lane]).GetComponent<RectTransform>();
            newArrow.rotation = Quaternion.Euler(0, 0, laneRotations[lane]);
            newArrow.anchoredPosition = new Vector2(0, -300 - 100 * i);

            activeArrowsPerLane[lane].Add(newArrow);
        }
    }

    private void HandleKeyboardInput()
    {
        if (Keyboard.current.leftArrowKey.wasPressedThisFrame) TryHitArrow(0);
        if (Keyboard.current.downArrowKey.wasPressedThisFrame) TryHitArrow(1);
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame) TryHitArrow(2);
    }

    public void TryHitArrow(int laneIndex)
    {
        // If the user presses a button but there are no arrows left in that lane
        if (activeArrowsPerLane[laneIndex].Count == 0)
        {
            score -= 10; // Slight penalty for button mashing empty lanes
            return;
        }

        // The next arrow to hit is always the first one in the lane's list (index 0)
        RectTransform targetArrow = activeArrowsPerLane[laneIndex][0];
        
        // Check how far the arrow's Y position is from exactly 0
        float distance = Mathf.Abs(targetArrow.anchoredPosition.y);

        if (distance <= perfectThreshold)
        {
            score += 150;
            RemoveArrow(laneIndex, targetArrow);
            howGoodText.text = "<color=yellow>+150 Perfect!!";
        }
        else if (distance <= greatThreshold)
        {
            score += 50;
            RemoveArrow(laneIndex, targetArrow);
            howGoodText.text = "<color=green>+50 Great!";
        }
        else if (distance <= goodThreshold)
        {
            score += 10;
            RemoveArrow(laneIndex, targetArrow);
            howGoodText.text = "<color=lightblue>+10 Good";
        }
        else if (distance <= missThreshold) 
        {
            // The arrow is somewhat close, but pressed too early/late = Miss
            score -= 50; 
            RemoveArrow(laneIndex, targetArrow);
            howGoodText.text = "<color=red>-50 Miss";
        }
        else
        {
            // The arrow is still way too far down the screen.
            // Ignore the arrow, but penalize the player for button mashing.
            score -= 10;
            howGoodText.text = "";
        }
    }

    private void MoveAndCheckArrows()
    {
        float amountToMoveUp = Time.deltaTime * speed;

        for (int lane = 0; lane < activeArrowsPerLane.Length; lane++)
        {
            // Iterate backwards through the list because we might destroy objects while looping
            for (int i = activeArrowsPerLane[lane].Count - 1; i >= 0; i--)
            {
                RectTransform arrow = activeArrowsPerLane[lane][i];
                arrow.anchoredPosition += new Vector2(0, amountToMoveUp);

                // If the arrow moves past the hit zone entirely without being pressed
                if (arrow.anchoredPosition.y > missThreshold)
                {
                    score -= 50; // Penalty for missing
                    RemoveArrow(lane, arrow, i);
                }
            }
        }
    }

    private void RemoveArrow(int laneIndex, RectTransform arrow, int listIndex = 0)
    {
        Destroy(arrow.gameObject);
        activeArrowsPerLane[laneIndex].RemoveAt(listIndex);
    }

    private void UpdateUI()
    {
        // Keep the score from dropping below 0 (optional, delete if you want negative scores)
        score = Mathf.Max(0, score); 
        highscoreText.text = $"Score: {score}\nHighscore: 2000";
    }

    public override bool Verify()
    {
        return score > 2000;
    }
}