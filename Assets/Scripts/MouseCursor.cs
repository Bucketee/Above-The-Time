using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    [SerializeField] private Sprite[] cursorSprites;

    [Header("Time Lock")]
    private LayerMask timelockLayers;

    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        Cursor.visible = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        timelockLayers = ~ (1 << LayerMask.NameToLayer("Mouse") | (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("UI")) | (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("Default")));
    }

    private void Start()
    {
        spriteRenderer.sprite = cursorSprites[0];
    }

    private void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        if (Input.GetMouseButtonDown(1))
        {
            var hit = Physics2D.Raycast(transform.position, Vector2.zero, 1, timelockLayers);
            if (hit.collider)
            {
                TimeLockObject timelockObject = hit.collider.GetComponent<TimeLockObject>();
                doTimelock(timelockObject);
            }
        }
    }

    private void doTimelock(TimeLockObject timelockObject)
    {
        if (timelockObject == null) return;
        timelockObject.HandleTimeLock();
    }
}
