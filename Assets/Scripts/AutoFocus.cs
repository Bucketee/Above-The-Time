using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class AutoFocus : MonoBehaviour
{
    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject != this.gameObject)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }
    }
}