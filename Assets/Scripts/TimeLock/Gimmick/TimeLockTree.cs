using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLockTree : TimeLockObject
{
    public enum TreeState
    {
        seed,
        sprout,
        tree,
    }

    [SerializeField] private Sprite[] treeSprites;
    
    private SpriteRenderer spriteRenderer;
    public TreeState state;
    private bool timeLocked => TimeLocked;
    private bool canTransform;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        //state = TreeState.seed;
        canTransform = true;
        timeManager = GameManager.Instance.TimeManager;
        gameStateManager = GameManager.Instance.GameStateManager;
        if (GameManager.Instance.StoryManager.CurrentStory <= StoryProgress.PlantTree)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (state == TreeState.tree && GameManager.Instance.StoryManager.CurrentStory < StoryProgress.TopTower && GameManager.Instance.TimeZoneManager.NowTimeZone == TimeZone.Future)
        {
            spriteRenderer.sprite = null;
        }
        //if (gameStateManager.NowGameState != GameState.Playing)
        //{
        //    return;
        //}

        //if (GameManager.Instance.StoryManager.CurrentStory == StoryManager.StoryProgress.BackToTree)
        //{
        //    this.enabled = true;
        //}
        //else
        //{
        //    this.enabled = false;
        //}

        //if (timeLocked && canTransform && GameManager.Instance.TimeManager.timeWindCost > 20f)
        //{
        //    var scroll = Input.GetAxis("Mouse ScrollWheel");
        //    if (scroll > 0.5f && (int)state < 2)
        //    {
        //        GameManager.Instance.TimeManager.timeWindCost -= 20f;
        //        //canTransform = false;
        //        state = (TreeState)((int)state + 1);
        //        spriteRenderer.sprite = treeSprites[(int)state];
        //        //StartCoroutine(TreeGrowMotion());
        //    }
        //    else if (scroll < -0.5f && (int)state > 0)
        //    {
        //        GameManager.Instance.TimeManager.timeWindCost -= 20f;
        //        //canTransform = false;
        //        state = (TreeState)((int)state - 1);
        //        spriteRenderer.sprite = treeSprites[(int)state];
        //        //StartCoroutine(TreeReverseMotion());
        //    }
        //    if (scroll > 0.5f && (int)state == 1)
        //    {
        //        GameManager.Instance.TimeManager.timeWindCost -= 20f;
        //        //canTransform = false;
        //        state = (TreeState) 2;
        //        spriteRenderer.sprite = treeSprites[2];
        //        //StartCoroutine(TreeGrowMotion());
        //    }
        //}


    }


    IEnumerator TreeGrowMotion()
    {
        for (int i = 0; i < 100; i++)
        {
            transform.localScale = transform.localScale + new Vector3(0f, 0.01f, 0f);
            transform.position += new Vector3(0f, 0.5f/100, 0f);
            yield return new WaitForSeconds(0.01f);
        }
        spriteRenderer.sprite = treeSprites[(int)state];
        canTransform = true;
    }

    IEnumerator TreeReverseMotion()
    {
        for (int i = 0; i < 100; i++)
        {
            transform.localScale = transform.localScale - new Vector3(0f, 0.01f, 0f);
            transform.position += new Vector3(0f, -0.5f / 100, 0f);
            yield return new WaitForSeconds(0.01f);
        }
        spriteRenderer.sprite = treeSprites[(int)state];
        canTransform = true;
    }

    public void ChangeShape()
    {
        if (GameManager.Instance.StoryManager.CurrentStory < StoryProgress.GoToFuture && GameManager.Instance.TimeZoneManager.NowTimeZone == TimeZone.Future)
        {
            spriteRenderer.sprite = null;
        }
        else
        {
            spriteRenderer.sprite = treeSprites[(int)state];
        }
    }
}