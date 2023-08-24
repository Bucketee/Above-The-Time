using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float cameraSpeed = 0.02f;
    private float cameraZ;
    private float width, height;
    [Tooltip("minX, minY, maxX, maxY")]
    [SerializeField] private List<Border> borders;
    [SerializeField] private Border currentBorder;
    

    private void Awake()
    {
        cameraZ = transform.position.z;

        Vector2 v = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        width = v.x;
        height = v.y;
    }

    private void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(player.position.x, player.position.y, cameraZ);

        if (!IsInBorder(currentBorder))
        {
            FindBorder();
        }
        
        if (targetPos.x + width > currentBorder.maxX)
        {
            targetPos.x = currentBorder.maxX - width;
        }
        if (targetPos.x - width < currentBorder.minX)
        {
            targetPos.x = currentBorder.minX + width;
        }
        if (targetPos.y + height > currentBorder.maxY)
        {
            targetPos.y = currentBorder.maxY - height;
        }
        if (targetPos.y - height < currentBorder.minY)
        {
            targetPos.y = currentBorder.minY + height;
        }

        transform.position = Vector3.Lerp(transform.position, targetPos, cameraSpeed);
    }

    private void FindBorder()
    {
        foreach (Border border in borders)
        {
            if (IsInBorder(border))
            {
                currentBorder = border;
                break;
            }
        }
        Debug.LogWarning("Where are you?");
    }

    private bool IsInBorder(Border border)
    {
        if (border.minX < player.position.x && border.minY < player.position.y && player.position.x < border.maxX && player.position.y < border.maxY)
        {
            return true;
        }
        return false;
    }
}
