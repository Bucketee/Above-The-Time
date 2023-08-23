using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLockTree : TimeLockObject
{
    enum TreeState
    {
        seed,
        sprout,
        tree
    }

    [SerializeField] private Sprite[] treeSprites;
    
    private SpriteRenderer spriteRenderer;
    private TreeState state;
    private bool timeLocked => TimeLocked;
    private bool canTransform;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        state = TreeState.seed;
        canTransform = true;
    }

    private void Update()
    {
        if (timeLocked && canTransform)
        {
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            if(scroll > 0.5f && (int)state < 2) 
            { 
                canTransform = false;
                state = (TreeState)((int)state + 1);
                StartCoroutine(TreeGrowMotion());
            }
            else if (scroll < -0.5f && (int)state > 0)
            {
                canTransform = false;
                state = (TreeState)((int)state - 1);
                StartCoroutine(TreeReverseMotion());
            }
        }


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
}