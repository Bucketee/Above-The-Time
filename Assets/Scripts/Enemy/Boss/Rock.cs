using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : TimeLockObject
{
    private float life = 5f;
    private Coroutine countDownCoroutine;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.gameObject.layer == 21)
        {
            other.collider.gameObject.GetComponentInParent<Player>().GetDamaged(80f);
            StopCoroutine(countDownCoroutine);
            Destroy(this.gameObject);
        }
        else if(other.collider.gameObject.layer == 22)
        {
            other.collider.gameObject.GetComponentInParent<Boss>().GetDamaged(400f);
            StopCoroutine(countDownCoroutine);
            Destroy(this.gameObject);
        }
    }

    public void CountDown()
    {
        countDownCoroutine = StartCoroutine(CountDownCo());
    }

    private IEnumerator CountDownCo()
    {
        float elapsedtime = 0f;
        while(elapsedtime < life)
        {
            if (!timeLocked)
            {
                elapsedtime += Time.deltaTime;
            }
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
