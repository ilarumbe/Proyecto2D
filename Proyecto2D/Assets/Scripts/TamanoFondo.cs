using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TamanoFondo : MonoBehaviour
{
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        float height = Camera.main.orthographicSize * 2f;
        float width = height * Camera.main.aspect;

        Vector2 spriteSize = sr.sprite.bounds.size;

        transform.localScale = new Vector3(
            width / spriteSize.x,
            height / spriteSize.y,
            1f
        );
    }
}
