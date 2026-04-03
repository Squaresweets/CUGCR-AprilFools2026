using UnityEngine;

public class Intro : Level
{
    public void ButtonPressed()
    {
        GetComponentInParent<LevelManager>().Verify();
        Timer.StartTimer();
    }

    public override bool Verify() => true;
}
