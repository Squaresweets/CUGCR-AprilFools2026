using System.Collections;
using TMPro;
using UnityEngine;

public class TextBoxAnswer : Level
{
    [SerializeField] public TMP_InputField inputField;
    [SerializeField] public string correctAnswer;
    [SerializeField] public GameObject enableAfterOneWrong;
    [Space]
    [SerializeField] public string specialAnswer;
    [SerializeField] public GameObject special;
    public override bool Verify()
    {
        if (specialAnswer != "")
        {
            bool special = inputField.text.ToLower().Contains(specialAnswer);
            if (special) { StartCoroutine(SpecialAnim()); return false; }
        }
        bool b = inputField.text.ToLower().Contains(correctAnswer);
        if(!b && enableAfterOneWrong != null) enableAfterOneWrong.SetActive(true);
        return b;
    }
    private IEnumerator SpecialAnim()
    {
        special.SetActive(true);
        yield return new WaitForSeconds(2);
        special.SetActive(false);
    }
    public void Back() => GetComponentInParent<LevelManager>().PreviousLevel();
    public void Next() => GetComponentInParent<LevelManager>().NextLevel();
}
