using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomscirpttest : MonoBehaviour
{
    //brute method, a changé si possible
    [SerializeField]
    private BoxCollider BoxCollider;
    [SerializeField]
    private BoxCollider BoxCollider1;
    [SerializeField]
    private BoxCollider BoxCollider2;
    [SerializeField]
    private BoxCollider BoxCollider3;
    [SerializeField]
    private BoxCollider BoxCollider4;
    [SerializeField]
    private BoxCollider BoxCollider5;
    [SerializeField]
    private BoxCollider BoxCollider6;
    [SerializeField]
    private BoxCollider BoxCollider7;

    public void PRINTEVENT(String S)
    {
        Debug.Log(S);
        BoxCollider.enabled = true;
        BoxCollider1.enabled = true;
        BoxCollider2.enabled = true;
        BoxCollider3.enabled = true;
        BoxCollider4.enabled = true;
        BoxCollider5.enabled = true;
        BoxCollider6.enabled = true;
        BoxCollider7.enabled = true;

        gameObject.SetActive(false);
    }
}
