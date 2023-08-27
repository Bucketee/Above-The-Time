using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeZoneLoading : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject loadingImage;
    [SerializeField] private float fadeTime = 2f;
    private Image image;
    private Color tempColor;
    private Vector2 tempSpeed;

    private void Awake()
    {
        image = loadingImage.GetComponent<Image>();
        tempColor = image.color;
        tempColor.a = 0f;
        image.color = tempColor;
    }

    public void LoadingFadeIn()
    {
        player.GetComponent<PlayerController>().timeChanging = true;
        tempSpeed = player.GetComponent<PlayerController>().speed;
        player.GetComponent<PlayerController>().speed = new Vector2(0f, 0f);
        player.GetComponentInChildren<Shooter>().isCool = true;

        tempColor = image.color;
        tempColor.a = 1f;
        image.color = tempColor;
        LoadingFadeOut();
    }
    /*
    private IEnumerator FadeIn()
    {
        for (int i = 0; i < 100; i++)
        {
            tempColor = image.color;
            tempColor.a += 0.01f;
            image.color = tempColor;

            yield return new WaitForSeconds(fadeTime / 100);
        }
        LoadingFadeOut();
    }
    */

    public void LoadingFadeOut()
    {
        StartCoroutine(FadeOut());
    }
    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(fadeTime);

        player.GetComponent<PlayerController>().timeChanging = false;
        player.GetComponent<PlayerController>().speed = tempSpeed;
        player.GetComponentInChildren<Shooter>().isCool = false;

        for (int i = 0; i < 100; i++)
        {
            tempColor = image.color;
            tempColor.a -= 0.01f;
            image.color = tempColor;

            yield return new WaitForSeconds(fadeTime / 100);
        }

    }
}
