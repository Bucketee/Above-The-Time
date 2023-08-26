using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minutehand : MonoBehaviour
{
    [SerializeField] private GameObject mouseCursor;
    [SerializeField] private float timeAngle;
    private float angle;
    private float minuteHandAngle;
    private float minuteHandAngleNext;
    private float minuteHandAnglePre;
    private Vector2 mouseDirection;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetMouseButton(0) && other.tag == "Cursor")
        {
            if (GameManager.Instance.TimeZoneManager.Operatinghand == null)
            {
                GameManager.Instance.TimeZoneManager.Operatinghand = gameObject;
            }
        }

    }

    private void Update()
    {
        if (GameManager.Instance.TimeZoneManager.Operatinghand == gameObject)
        {
            mouseDirection = mouseCursor.transform.position - Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2f, Screen.height / 2f));
            angle = Mathf.Atan2(mouseDirection.y, mouseDirection.x) * Mathf.Rad2Deg;

            timeAngle = (90f - angle);
            if(timeAngle < 0f) { timeAngle += 360f; }

            minuteHandAngle = abs(GameManager.Instance.TimeZoneManager.minuteHandPlus * 30f);
            minuteHandAngleNext = abs(minuteHandAngle + 30f);
            minuteHandAnglePre = abs(minuteHandAngle - 30f);

            if (Mathf.Abs(timeAngle - minuteHandAngleNext) < 1f) { GameManager.Instance.TimeZoneManager.minuteHandPlus += 1; }
            if (Mathf.Abs(timeAngle - minuteHandAnglePre) < 1f) { GameManager.Instance.TimeZoneManager.minuteHandPlus -= 1; }

            transform.rotation = Quaternion.Euler(0f, 0f, -minuteHandAngle);
            //Debug.Log(mouseDirection);
            //Debug.Log(angle);
        }
    }

    private float abs(float num)
    {
        if(num < 0f) { return num % 360 + 360f; }
        else { return num % 360f; }
    }
}
