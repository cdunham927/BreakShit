using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotSmasher : MonoBehaviour
{
    GameObject toSpawn;
    Rigidbody2D bod;
    public float breakTime;
    public bool falling;
    public LayerMask groundLayer;
    public float timeFalling;

    private void Awake()
    {
        bod = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        timeFalling = 0f;
        Invoke("Disable", 10f);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, 1f, groundLayer);

        if (hit.collider != null)
        {
            falling = false;
        }
        else falling = true;

        if (falling)
        {
            timeFalling += Time.deltaTime;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (timeFalling >= breakTime)
        {
            //Set broken pot
            toSpawn = GameController.GC.getObjectFromList(GameController.GC.brokenPots);
            if (toSpawn != null)
            {
                //Set each of the childs colors to the right color
                SpriteRenderer[] rends = toSpawn.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer sprites in rends)
                {
                    sprites.color = GetComponent<SpriteRenderer>().color;
                }

                toSpawn.transform.position = transform.position;
                toSpawn.transform.rotation = transform.rotation;
                toSpawn.SetActive(true);
            }
            //Change money
            GameController.GC.money -= 10;
            gameObject.SetActive(false);
        }
    }
}
