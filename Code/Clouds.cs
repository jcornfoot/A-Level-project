using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
Source:
https://www.youtube.com/watch?v=W9aVuOsc_k0
*/

public class Clouds : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool scrollLeft;

    float singleTextureWidth;
    // Start is called before the first frame update
    void Start()
    {
        SetupTexture();
        if (scrollLeft) moveSpeed = -moveSpeed;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Scroll();
        CheckReset();
    }

    void SetupTexture() {
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        singleTextureWidth = sprite.texture.width /sprite.pixelsPerUnit;
    }

    void Scroll() {
        float delta = moveSpeed * Time.deltaTime;
        transform.position += new Vector3(delta, 0f, 0f);
    }

    void CheckReset() {
        if ((Mathf.Abs(transform.position.x) - singleTextureWidth) > 0) {
            transform.position = new Vector3(0.0f, transform.position.y, transform.position.z);
        }
    }
}
