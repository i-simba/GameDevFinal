using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TwoFrameAnim : MonoBehaviour
{
    [SerializeField] Sprite frameOne;
    [SerializeField] Sprite frameTwo;
    [SerializeField] float  animSpeed;

    private Image image;
    
    private bool  isOne;
    private float timer;

    void Awake()
    {
        image = this.GetComponent<Image>();

        image.sprite = frameOne;
        isOne        = true;
        timer        = 0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if ( timer >= animSpeed ) {
            if ( isOne ) {
                image.sprite = frameTwo;
                isOne = false;
            } else {
                image.sprite = frameOne;
                isOne = true;
            }
            timer = 0f;
        }
    }
}
