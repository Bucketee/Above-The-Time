using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TMP_Text statusText;
    private string frameText = "";

    private void LateUpdate()
    {
        statusText.text = frameText;
        frameText = "";
    }

    public void AddText(string text)
    {
        frameText = frameText + text + "\n";
    }

    public void AddText(string text, float time)
    {
        StartCoroutine(TextCo(text, time));
    }

    IEnumerator TextCo(string text, float time)
    {
        float due = Time.time + time;
        while (due > Time.time)
        {
            yield return null;
            AddText(text);
        }
    }
}
