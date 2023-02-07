using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    public UnityEngine.UI.Image fadeToBlackPanel;

    private bool okOuPas = false;

    // Start is called before the first frame update
    void Start()
    {
        fadeToBlackPanel.color = new Vector4(fadeToBlackPanel.color.r, fadeToBlackPanel.color.g, fadeToBlackPanel.color.b, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (okOuPas && fadeToBlackPanel.color.a < 1)
        {
            fadeToBlackPanel.color = new Vector4(fadeToBlackPanel.color.r, fadeToBlackPanel.color.g, fadeToBlackPanel.color.b, fadeToBlackPanel.color.a + 0.05f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Player>())
        {
            okOuPas = true;
        }
    }
}
