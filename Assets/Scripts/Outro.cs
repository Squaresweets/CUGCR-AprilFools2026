using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.InteropServices;

public class Outro : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void CloseModal();

    [SerializeField] private TMP_Text congratsText;
    void Start()
    {
        float seconds = Timer.GetTime();
        congratsText.text += $"{(int)(seconds / 60):00}:{(int)(seconds % 60):00}:{(int)(seconds * 1000) % 1000:000}";
    }

    public void Retry() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    public void Close()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        CloseModal();
#endif
        Application.Quit();
    }
}
