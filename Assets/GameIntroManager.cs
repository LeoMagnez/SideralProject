using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameIntroManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float timer;
    public float turnTime;
    public float turnSpeed;
    public Transform Ship;
    public Player pScript;
    public _movePlayer mScript;
    public Claude claude;
    public GameObject warpEffect;

    public bool InAnim;

    void Start()
    {
        InAnim = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(InAnim)
        {
            if (timer > 0)
            {

                timer -= Time.deltaTime;
            }
            else
            {
                if(turnTime > 0)
                {
                    Ship.localRotation = Quaternion.Lerp(Ship.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * turnSpeed);
                    turnTime -= Time.deltaTime;
                }
                else
                {
                    Ship.localRotation = Quaternion.Euler(0, 0, 0);
                    InAnim= false;
                }

            }

        }
        else
        {
            warpEffect.SetActive(true);
            pScript.enabled = true;
            mScript.enabled = true;
            claude.enabled = true;
            gameObject.GetComponent<GameIntroManager>().enabled = false;
        }

    }
    IEnumerator TurnAnim()
    {
        yield return new WaitForSeconds(turnTime);
        InAnim= false;
    }
}
