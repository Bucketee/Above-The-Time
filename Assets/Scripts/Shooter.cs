using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Bullet Type")]
    [SerializeField] private List<GameObject> bullets;

    [Header("About Bullet")]
    [SerializeField] private float speed;
    [SerializeField] private float life;

    private GameObject player;
    private Vector3 mousePoint;

    private void Start()
    {
        player = GetComponentInParent<Player>().gameObject;
    }

    private void Update()
    {
        PositionMove();
        bool[] mouseState = GetMouseInput();
        if (mouseState[0])
        {
            ShootBullet(0, GetDirection(), speed, life);
        }
        else if (mouseState[1])
        {
            ShootBullet(1, GetDirection(), speed, life);
        }
        else
        {

        }
    }

    #region postion
    /// <summary>
    /// move position of shooter to position of mouse
    /// </summary>
    private void PositionMove()
    {
        mousePoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
        transform.position = player.transform.position + (mousePoint - player.transform.position).normalized;
    }

    /// <summary>
    /// get direction of shooter for shooting bullets
    /// </summary>
    /// <returns>return direction</returns>
    private Vector2 GetDirection()
    {
        return (mousePoint - player.transform.position).normalized;
    }
    #endregion

    #region shooting
    /// <summary>
    /// Return right, left click state of mouse into array
    /// </summary>
    /// <returns></returns>
    private bool[] GetMouseInput()
    {
        bool[] mouseState = new bool[2];
        mouseState[0] = Input.GetMouseButtonDown(0);
        mouseState[1] = Input.GetMouseButtonDown(1);
        return mouseState;
    }

    /// <summary>
    /// shoot bullet
    /// </summary>
    /// <param name="bulletType">type of bullet</param>
    /// <param name="dir">direction of bullet to be shot</param>
    /// <param name="speed">speed of bullet</param>
    /// <param name="life">life time of bullet</param>
    private void ShootBullet(int bulletType, Vector2 dir, float speed, float life)
    {
        GameObject bullet = Instantiate(bullets[bulletType], transform.position, Quaternion.identity);
        bullet.GetComponent<Bullet>().Init(bulletType, dir, speed, life);
    }
    #endregion
}
