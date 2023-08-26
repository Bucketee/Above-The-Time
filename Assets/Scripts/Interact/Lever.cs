using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : InteractionObject
{
    private bool interactable = true;
    private TimeLockLever timeLockLever;
    private TimeZoneLever timeZoneLever;
    public LeverState leverState;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private List<Sprite> sprites;
    private Coroutine coroutine;

    public enum LeverState
    {
        Off,
        On,
        OffBroken,
        OnBroken,
    }

    private void Awake()
    {
        timeLockLever = GetComponent<TimeLockLever>();
        timeZoneLever = GetComponent<TimeZoneLever>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        LeverInitialize();
    }

    private void Update()
    {
        if (!interactable) return;
        spriteRenderer.sprite = sprites[(int)leverState * 2];
    }

    public void LeverInitialize()
    {
        if ((int)leverState <= 1)
        {

        }
        else
        {
            timeLockLever.leverStates.AddLast(leverState - 2);
        }
    }

    public override void Interact()
    {
        if (!interactable)
        {
            GameManager.Instance.UIManager.AddText("<color=#EC591A>Wait for lever to be interactable!</color>", 3);
            return;
        }
        if ((int) leverState <= 1)
        {
            if (timeLockLever.TimeLocked) timeLockLever.GetTimeUnLocked();
            timeLockLever.Interacted();
            SetLeverState(1 - leverState);
            OnStateChange();
        }
        else
        {
            GameManager.Instance.UIManager.AddText("<color=#EC591A>A broken lever cannot be controlled!</color>", 3);
        }
    }

    public void SetLeverState(LeverState state)
    {
        leverState = state;
    }
    
    public void OnStateChange()
    {
        interactable = false;
        timeLockLever.enabled = false;
        timeZoneLever.Record(GameManager.Instance.TimeZoneManager.NowTimeZone);
        if (coroutine != null) coroutine = null;
        coroutine = StartCoroutine(ChangeStateAnimation(leverState));
    }

    private IEnumerator ChangeStateAnimation(LeverState leverState)
    {
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.sprite = sprites[1];
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.sprite = sprites[(int)leverState * 2];
        interactable = true;
        timeLockLever.enabled = true;
    }
}