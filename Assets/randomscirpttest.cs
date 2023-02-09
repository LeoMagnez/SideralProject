using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomscirpttest : MonoBehaviour
{
    //brute method, a changé si possible
    [SerializeField]
    private GameObject GameObject;
    [SerializeField]
    private GameObject GameObject1;
    [SerializeField]
    private GameObject GameObject2;
    [SerializeField]
    private GameObject GameObject3;
    [SerializeField]
    private GameObject GameObject4;
    [SerializeField]
    private GameObject GameObject5;
    [SerializeField]
    private GameObject GameObject6;
    [SerializeField]
    private GameObject GameObject7;
    [SerializeField]
    private GameObject GameObject8;
    [SerializeField]
    private GameObject GameObject9;
    [SerializeField]
    private GameObject GameObject10;
    private void Start()
    {
        GameObject.SetActive(true);
        GameObject1.SetActive(false);
        GameObject2.SetActive(false);
        GameObject3.SetActive(false);
        GameObject4.SetActive(false);
        GameObject5.SetActive(false);
        GameObject6.SetActive(false);
        GameObject7.SetActive(false);
        GameObject8.SetActive(false);
        GameObject9.SetActive(false);
        GameObject10.SetActive(false);
    }
    public void PRINTEVENT(String S)
    {
        Debug.Log(S);
        GameObject.SetActive(false);
        GameObject1.SetActive(true);
        GameObject2.SetActive(true);
        GameObject3.SetActive(true);
        GameObject4.SetActive(true);
        GameObject5.SetActive(true);
        GameObject6.SetActive(true);
        GameObject7.SetActive(true);
        GameObject8.SetActive(true);
        GameObject9.SetActive(true);
        GameObject10.SetActive(true);
    }

    public void PRINTEVENT2(String D)
    {
        Debug.Log(D);

        this.gameObject.SetActive(false);
    }
}
