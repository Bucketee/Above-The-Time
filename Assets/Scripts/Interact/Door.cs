using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] List<Lever> levers = new();
    [Tooltip("off is 0, on is 1")]
    [SerializeField] private string password;

    [SerializeField] private bool open;

    private void Update()
    {
        string cur = "";
        foreach (Lever lever in levers)
        {
            cur += (int)lever.leverState % 2;
        }
        if (password.Equals(cur))
        {
            open = true;
        }
        else
        {
            open = false;
        }
    }
}
