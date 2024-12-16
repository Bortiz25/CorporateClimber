using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMouseScale : MonoBehaviour
{
    public float scaleSpeed = 0.1f;
    public float maxScale = 1.2f;
    public float minScale = 1f; 

    public void PointerEnter()
    {
        transform.localScale = new Vector2(maxScale, maxScale);
    }

    public void PointerExit()
    {
        transform.localScale = new Vector2(minScale, minScale);
    }
}