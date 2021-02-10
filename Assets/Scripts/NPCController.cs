using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public enum states { idleleft, idleright, getitem, exitarea };
    public states curState = states.idleright;
    public float lowSpd;
    public float highSpd;
    float spd;
    Rigidbody2D bod;
    SpriteRenderer rend;
    public SpriteRenderer wantRend;
    public Pot.colors wantColor;
    public SpriteRenderer bubble;
    Vector3 bubbleStart;
    GameObject potToGet;

    //The NPC wants a certain color pot
    //Once they spawn in, they will walk across the screen
    //A bubble above their head will indicate what kind of pot they want
    //If a pot of that kind is placed in their vicinity they will approach and take it
    //They will then leave the screen, giving the player money

    private void Awake()
    {
        bubbleStart = bubble.transform.localPosition;
        rend = GetComponent<SpriteRenderer>();
        bod = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        //Get random speed
        spd = Random.Range(lowSpd, highSpd);
        //Switch to random want
        //Change sprite accordingly
        wantColor = (Pot.colors)Random.Range(0, System.Enum.GetValues(typeof(Pot.colors)).Length);
        switch (wantColor)
        {
            case (Pot.colors.white):
                wantRend.color = Color.white;
                break;
            case (Pot.colors.red):
                wantRend.color = Color.red;
                break;
            case (Pot.colors.green):
                wantRend.color = Color.green;
                break;
            case (Pot.colors.blue):
                wantRend.color = Color.blue;
                break;
            case (Pot.colors.yellow):
                wantRend.color = Color.yellow;
                break;
            case (Pot.colors.orange):
                wantRend.color = new Color32(255, 165, 0, 255);
                break;
            case (Pot.colors.pink):
                wantRend.color = new Color32(255, 192, 203, 255);
                break;
        }

        if (transform.position.x > 0) curState = states.idleleft;
        else curState = states.idleright;
    }

    public void IdleLeft()
    {
        rend.flipX = true;
        bubble.flipX = true;
        wantRend.transform.localPosition = new Vector3(1.75f, 0.45f);
        transform.Translate(Vector2.left * Time.deltaTime * spd);
    }

    public void IdleRight()
    {
        rend.flipX = false;
        bubble.flipX = false;
        wantRend.transform.localPosition = new Vector3(-1.75f, 0.45f);
        transform.Translate(Vector2.right * Time.deltaTime * spd);
    }

    public void GetItem()
    {
        potToGet.SetActive(false);
        GameController.GC.money += 15;
        curState = states.exitarea;
    }

    public void ExitArea()
    {
        if (transform.position.x > 0)
        {
            curState = states.idleleft;
        }
        else
        {
            curState = states.idleright;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (curState != states.exitarea)
        {
            if (collision.CompareTag("Pot"))
            {
                Pot pot = collision.GetComponentInParent<Pot>();
                if (pot != null && pot.col == wantColor)
                {
                    bool canPickUp = ((collision.GetComponentInParent<PotSmasher>().timeFalling > 0) ? true : false && ((collision.GetComponentInParent<PotSmasher>().falling) ? false : true));
                    if (canPickUp)
                    {
                        Debug.Log("Wants to pick up this pot", collision.transform.parent.gameObject);
                        potToGet = collision.transform.parent.gameObject;
                        curState = states.getitem;
                    }
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (curState != states.exitarea)
        {
            if (collision.CompareTag("Pot"))
            {
                Pot pot = collision.GetComponentInParent<Pot>();
                if (pot != null && pot.col == wantColor)
                {
                    if (transform.position.x > 0) curState = states.idleleft;
                    else curState = states.idleright;
                }
            }
        }
    }

    private void Update()
    {
        switch (curState)
        {
            case states.idleleft:
                IdleLeft();
                break;
            case states.idleright:
                IdleRight();
                break;
            case states.getitem:
                GetItem();
                break;
            case states.exitarea:
                ExitArea();
                break;
        }

        if (!rend.isVisible)
        {
            Invoke("Disable", 3f);
        }
        else
        {
            CancelInvoke();
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    void Disable()
    {
        gameObject.SetActive(false);
    }
}
