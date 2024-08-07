using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    [SerializeField] float transSpeed = 1;
    [SerializeField] Color fadeColor = Color.black;

    private Image img;
    private Scene active;
    private bool fading;

    void Awake()
    {
        img = this.GetComponent<Image>();
        active = SceneManager.GetActiveScene();
        fading = false;

        if ( active.name == "Death" )
            fadeColor = Color.red;
    }

    void Start()
    {
        FadeIn();
    }

    public void SetSpeed( float num )
    {
        transSpeed = num;
    }

    public void FadeIn()
    {
        StartCoroutine( startfadein() );
        IEnumerator startfadein() {
            float timer = 0;
            img.color = fadeColor;
            while ( timer < transSpeed ) {
                yield return null;
                timer += Time.deltaTime;
                img.color = new Color( fadeColor.r, fadeColor.g, fadeColor.b, 1f - ( timer / transSpeed) );
            }
            img.color = Color.clear;
        }
    }

    public void FadeOut( string next )
    {
        if ( fading ) { return; }
        if ( next == "Death" )
            fadeColor = Color.red;

        fading = true;
        StartCoroutine( startfadeout() );
        IEnumerator startfadeout() {
            float timer = 0;
            img.color = Color.clear;
            while ( timer < transSpeed ) {
                yield return null;
                timer += Time.deltaTime;
                img.color = new Color( fadeColor.r, fadeColor.g, fadeColor.b, timer / transSpeed );
            }
            img.color = fadeColor;
            SceneManager.LoadScene( next );
        }
    }
}
