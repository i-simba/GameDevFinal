using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsHandler : MonoBehaviour
{
    [SerializeField] Image container;

    void Awake()
    {
        container.transform.localPosition = Vector3.zero;
        this.GetComponent<Canvas>().enabled = false;
    }

    public void Back()
    {
        this.GetComponent<Canvas>().enabled = false;
    }
}
