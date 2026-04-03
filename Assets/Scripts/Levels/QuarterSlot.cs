using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuarterSlot : Level
{
    [SerializeField] Image benPic;
    [SerializeField] Image samPic;
    [SerializeField] Image yourPic;
    [SerializeField] TMP_Text yourText;

    private float startTime;
    private float endTime;
    private float time;

    private void Start()
    {
        benPic.enabled = false;
        samPic.enabled = false;
        yourPic.enabled = false;
        yourText.enabled = true;
        startTime = Random.Range(2, 5);
        endTime = startTime + 0.35f;
        time = 0f;
        clickCooldown = 0f;
    }
    private void Update()
    {
        time += Time.deltaTime;
        clickCooldown -= Time.deltaTime;
        if (time > startTime) benPic.enabled = true;
        if (time > endTime && !yourPic.enabled) samPic.enabled = true;
    }
    private float clickCooldown;
    public void Send()
    {
        if (clickCooldown > 0f) return;
        clickCooldown = 2f;

        if (time > startTime && time < endTime)
        {
            yourText.enabled = false;
            yourPic.enabled = true;
        }
    }
    public override bool Verify()
    {
        if (yourPic.enabled) return true;

        Start();

        return false;
    }
}
