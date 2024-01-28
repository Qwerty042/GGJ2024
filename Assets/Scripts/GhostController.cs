using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GhostController : MonoBehaviour
{
    float alpha = 1.0f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (alpha >= 0.0f)
        {
            GetComponent<SpriteRenderer>().color = new Color(1.0f, 1.0f, 1.0f, alpha);
            transform.position += new Vector3(0, 1.0f * Time.deltaTime);
            alpha -= 0.5f * Time.deltaTime;
        }
    }
}
