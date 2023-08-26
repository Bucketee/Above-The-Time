using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoardAnimation : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private float timing = 0.4f;

    [SerializeField] private List<Sprite> sprites = new();
    private int current = 0;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        StartCoroutine(ChangeState());
    }

    IEnumerator ChangeState()
    {
        yield return new WaitForSeconds(timing);
        current += 1;
        spriteRenderer.sprite = sprites[current % sprites.Count];
        StartCoroutine(ChangeState());
    }
}
