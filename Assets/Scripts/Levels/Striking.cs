using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.UI;

public class Striking : Level
{
    [SerializeField] private TMP_Text bellsText;
    [SerializeField] private float normalBellGap = 0.4f;
    [SerializeField] private ToggleGroup bellSelectionGroup;
    [SerializeField] private ToggleGroup speedGroup;
    List<float> times;
    List<float> correctTimes;
    void Start()
    {
        times = Enumerable.Repeat(0f, 8).ToList();
        for (int i = 0; i < 8; i++) times[i] = (i+1) * normalBellGap;
        correctTimes = times.ToList();

        int[] toChange = Enumerable.Range(1, 6)
                         .OrderBy(_ => Random.value)
                         .Take(4)
                         .ToArray();

        foreach (int i in toChange)
        {
            float offset = Random.Range(normalBellGap / 3, normalBellGap / 2);
            if (Random.value < 0.5) offset *= -1;
            times[i] += offset;
        }

        SetStatusText();
        StartCoroutine(Play());
    }
    public void ShoutAtPerson()
    {
        int selectedBell = int.Parse(bellSelectionGroup.GetFirstActiveToggle().name) - 1;
        bool faster = speedGroup.GetFirstActiveToggle().name == "Faster";

        if (faster)
        {
            if (times[selectedBell] > correctTimes[selectedBell]) times[selectedBell] = correctTimes[selectedBell];
            else times[selectedBell] -= normalBellGap / 2;
        }
        else
        {
            if (times[selectedBell] < correctTimes[selectedBell]) times[selectedBell] = correctTimes[selectedBell];
            else times[selectedBell] += normalBellGap / 2;
        }

        SetStatusText();
    }
    void SetStatusText()
    {
        string status = "";
        for (int i = 0; i < times.Count; i++)
        {
            if (times[i] == correctTimes[i]) status += $"{i+1} ";
            else status += $"<color=red>{i+1}</color> ";
        }
        bellsText.text = status;
    }
    IEnumerator Play()
    {
        while (true)
        {
            yield return StartCoroutine(PlayRow());
            yield return StartCoroutine(PlayRow());
            yield return new WaitForSeconds(normalBellGap);
        }
    }
    IEnumerator PlayRow()
    {
        List<float> _times = times.ToList(); //Can't change midRow
        float t = 0;
        List<bool> done = Enumerable.Repeat(false, 8).ToList();
        while (done.Any(b=>!b))
        {
            yield return null;
            t += Time.deltaTime;
            for (int i = 0; i < _times.Count; i++)
            {
                if (done[i] || _times[i] > t) continue;
                BellAudio.instc.PlayBellSound(i);
                done[i] = true;
            }
        }
    }

    public override bool Verify()
    {
        for (int i = 0; i < times.Count; i++) if (times[i] != correctTimes[i]) return false;
        return true;
    }
}
