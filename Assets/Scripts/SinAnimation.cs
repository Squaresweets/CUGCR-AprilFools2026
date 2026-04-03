using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class SinAnimation : MonoBehaviour
{
    [SerializeField] public bool active = true;
    [SerializeField] public SinAnimationField xPosition;
    [SerializeField] public SinAnimationField yPosition;
    [SerializeField] public SinAnimationField rotation;
    [SerializeField] public SinAnimationField scale;
    public bool react = false;

    private Vector3 originalPos;
    private Vector3 originalScale;
    private float originalRotation;
    private float randomStartOffset;


    static SinAnimationField zero = new SinAnimationField();

    private void Start()
    {
        originalRotation = transform.localEulerAngles.z;
        originalPos = transform.localPosition;
        randomStartOffset = UnityEngine.Random.Range(0, 100);
        originalScale = transform.localScale;

    }
    void Update()
    {
        if (!active)
        {
            transform.localPosition = originalPos;
            transform.localEulerAngles.Set(transform.localEulerAngles.x, transform.localEulerAngles.y, originalRotation);
            return;
        }

        if (xPosition != zero || yPosition != zero)
        {
            float x = Mathf.Sin(Time.time * (xPosition.speed) + randomStartOffset) * xPosition.amplitude;
            float y = Mathf.Sin(Time.time * (yPosition.speed) + randomStartOffset) * yPosition.amplitude;
            transform.localPosition = originalPos + new Vector3(x, y);
        }


        if (rotation != zero)
        {
            float rotationModifier = Mathf.Sin(Time.time * rotation.speed + randomStartOffset) * rotation.amplitude;
            transform.localEulerAngles = new Vector3(0, 0, originalRotation + rotationModifier);
        }

        if (scale != zero)
        {
            transform.localScale = new Vector2(Mathf.Abs(Mathf.Sin(Time.time * scale.speed + randomStartOffset) * scale.amplitude + scale.amplitude + originalScale.x),
                                               Mathf.Abs(Mathf.Sin(Time.time * scale.speed + randomStartOffset) * scale.amplitude + scale.amplitude + originalScale.y));
        }

    }
    public void SetActive(bool _active)
    {
        active = _active;
    }

    public void SetActiveForSeconds(float seconds)
    {
        StopAllCoroutines();
        StartCoroutine(E_ActiveForSeconds(seconds));
    }

    private IEnumerator E_ActiveForSeconds(float seconds)
    {
        SetActive(true);
        yield return new WaitForSeconds(seconds);
        if (active) SetActive(false);
    }
}

[Serializable]
public struct SinAnimationField  //what does this do? inspector serialization
{
    public float amplitude;
    public float speed;

    public SinAnimationField(float _amplitude = 0, float _speed = 0)
    {
        amplitude = _amplitude;
        speed = _speed;
    }

    public override bool Equals(object obj)
    {

        if (obj == null || GetType() != obj.GetType()) return false;

        SinAnimationField s1 = (SinAnimationField)obj;
        return this.amplitude == s1.amplitude && this.speed == s1.speed;
    }

    public override int GetHashCode()
    {
        return (int)Mathf.Floor(amplitude + speed);
    }

    public static bool operator ==(SinAnimationField s1, SinAnimationField s2)
    {
        return s1.Equals(s2);
    }

    public static bool operator !=(SinAnimationField s1, SinAnimationField s2)
    {
        return !s1.Equals(s2);
    }
}
