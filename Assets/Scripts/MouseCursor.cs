using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseCursor : MonoBehaviour
{
    [SerializeField] private Sprite[] cursorSprites;

    [Header("Time Lock")]
    private LayerMask timelockLayers;

    private SpriteRenderer spriteRenderer;

    private GameStateManager gameStateManager;

    [SerializeField] private List<GameObject> timeLocks = new();

    private void Awake()
    {
        Cursor.visible = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        timelockLayers = ~ (1 << LayerMask.NameToLayer("Mouse") | (1 << LayerMask.NameToLayer("Player")) | (1 << LayerMask.NameToLayer("UI")) | (1 << LayerMask.NameToLayer("Floor")) | (1 << LayerMask.NameToLayer("Default")));
    }

    private void Start()
    {
        spriteRenderer.sprite = cursorSprites[0];
        gameStateManager = GameManager.Instance.GameStateManager;
    }

    private void Update()
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        if (gameStateManager.NowGameState != GameState.Playing)
        {
            return;
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (timeLocks.Count > 0)
            {
                TimeLockObject timelockObject = timeLocks[0].GetComponent<TimeLockObject>();
                doTimelock(timelockObject);
            }
        }
    }

    private void doTimelock(TimeLockObject timelockObject)
    {
        if (timelockObject == null) return;
        timelockObject.HandleTimeLock();
    }

    public void AddThisObject(GameObject gameObject)
    {
        if (timeLocks.Contains(gameObject)) return;
        timeLocks.Add(gameObject);
    }

    public void RemoveThisObject(GameObject gameObject)
    {
        if (!timeLocks.Contains(gameObject)) return;
        timeLocks.Remove(gameObject);
    }
}
