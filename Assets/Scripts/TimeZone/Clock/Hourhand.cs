using Unity.VisualScripting;
using UnityEngine;

public class Hourhand : MonoBehaviour
{
    [SerializeField] private GameObject mouseCursor;
    [SerializeField] private float timeAngle;
    private float angle;
    private float hourHandAngle;
    private float hourHandAngleNext;
    private float hourHandAnglePre;
    private Vector2 mouseDirection;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetMouseButton(0) && other.tag == "Cursor")
        {
            if(GameManager.Instance.TimeZoneManager.Operatinghand == null)
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
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

            timeAngle = (90f - angle);
            if (timeAngle < 0f) { timeAngle += 360f; }

            hourHandAngle = abs(GameManager.Instance.TimeZoneManager.hourHandPlus * 30f);
            hourHandAngleNext = abs(hourHandAngle + 30f);
            hourHandAnglePre = abs(hourHandAngle - 30f);

            if (Mathf.Abs(timeAngle - hourHandAngleNext) < 1f) { GameManager.Instance.TimeZoneManager.hourHandPlus += 1; }
            if (Mathf.Abs(timeAngle - hourHandAnglePre) < 1f) { GameManager.Instance.TimeZoneManager.hourHandPlus -= 1; }
            //Debug.Log(mouseDirection);
            //Debug.Log(angle);
        }
    }

    private float abs(float num)
    {
        if (num < 0f) { return num % 360 + 360f; }
        else { return num % 360f; }
    }
}
