using System;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class Cannon : Level
{
    [SerializeField] private RectTransform cannon;
    [SerializeField] private RectTransform rilla;
    [SerializeField] private TMP_Text fireText;
    [SerializeField] private Image face;
    [SerializeField] private float velocity = 10f;
    [SerializeField] private float gravity = 30f;
    [SerializeField] private float timeScale = 5f;

    bool rotating = true;
    Vector2 origin;
    float startingRotation;
    float flightTime;

    Vector2 prevPos;

    void Update()
    {
        //Angle goes between -21 and 56
        cannon.localRotation = Quaternion.Euler(0, 0, Mathf.Sin(Time.time) * 38.5f + 17.5f);

        if (rotating)
        {
            face.enabled = true;
            rilla.gameObject.SetActive(false);
        }
        else //Flying
        {
            flightTime += Time.deltaTime * timeScale;
            float x = velocity * Mathf.Cos(startingRotation) * flightTime;
            float y = velocity * Mathf.Sin(startingRotation) * flightTime - 0.5f * 9.8f * flightTime * flightTime;

            rilla.anchoredPosition = origin + new Vector2(x, y);
            if (prevPos != rilla.anchoredPosition)
            {
                Vector2 change = rilla.anchoredPosition - prevPos;
                rilla.localRotation = Quaternion.Euler(0, 0, 360 - Mathf.Atan2(change.x,change.y) * Mathf.Rad2Deg);
            }
            prevPos = rilla.anchoredPosition;

            //Check if oob
            if (rilla.anchoredPosition.x > 386 || rilla.anchoredPosition.y < -404)
            {
                ResetCannon();
            }
        }
    }

    public void ButtonPressed()
    {
        if (rotating) Fire();
        else ResetCannon();
    }
    private void Fire()
    {
        rotating = false;
        face.enabled = false;
        rilla.gameObject.SetActive(true);

        rilla.transform.position = face.transform.position;

        origin = rilla.anchoredPosition;
        prevPos = rilla.anchoredPosition;

        Vector2 dir = face.rectTransform.right; // UI space direction
        startingRotation = Mathf.Atan2(dir.y, dir.x);

        flightTime = 0;

        fireText.text = "RESET";
    }
    private void ResetCannon()
    {
        rotating = true;
        face.enabled = true;
        rilla.gameObject.SetActive(false);

        fireText.text = "FIRE";
    }

    public override bool Verify()
    {
        bool b = Vector2.Distance(rilla.anchoredPosition, new Vector2(159, 109)) < 50f;
        if (b) { rotating = true; rilla.gameObject.SetActive(false); }
        return b;
    }
}
