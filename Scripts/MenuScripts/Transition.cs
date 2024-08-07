using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    [SerializeField] Image bg;
    [SerializeField] Image lg;
    [SerializeField] Button sBtn;
    [SerializeField] Button oBtn;
    [SerializeField] Button eBtn;

    private List<Button> l_btn;
    private float transSpeed;

    void Awake()
    {
        transSpeed = 1;
        l_btn = new List<Button>
        {
            sBtn,
            oBtn,
            eBtn
        };
    }

    void Start()
    {
        SlideIn();
        ZoomOut();
    }

    public void SlideIn()
    {
        StartCoroutine( SlideRoutine() );
        IEnumerator SlideRoutine() {
            float timer = 0;
            float mult  = 0.35f;
            float lgY   = lg.transform.position.y;
            float[] btn_loc = {
                l_btn[0].transform.position.y,
                l_btn[1].transform.position.y,
                l_btn[2].transform.position.y
            };

            while( timer < transSpeed ) {
                yield return null;
                timer += Time.deltaTime;
                int i;
                for ( i = 0; i < l_btn.Count; i++ ) {
                    l_btn[i].transform.position = new Vector3( l_btn[i].transform.position.x, btn_loc[i] - ( timer * mult ), 1 );
                }
                lg.transform.position = new Vector3( lg.transform.position.x, lgY + ( timer * mult ), 1 );
            }
        }
    }

    public void ZoomOut()
    {
        StartCoroutine( ZoomRoutine() );
        IEnumerator ZoomRoutine() {
            float timer = 0;
            float mult  = 0.2f;
            float newS  = bg.transform.localScale.x;
            while ( timer < transSpeed ) {
                yield return null;
                timer += Time.deltaTime;
                bg.transform.localScale = new Vector3( newS - ( timer * mult ), newS - ( timer * mult ), 1 );
            }
        }
    }
}
