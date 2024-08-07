using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResolutionHandler : MonoBehaviour
{
    [SerializeField] TMP_Dropdown res_d;

    private Resolution[] resolutions;
    private List<string> options;
    private int          currResIdx;
    private bool         isFull;

    void Awake()
    {
        resolutions = Screen.resolutions;
        options = new List<string>();
        res_d.ClearOptions();

        foreach ( Resolution res in resolutions ) {
            string restr = res.width + "x" + res.height;
            options.Add( restr );
        }

        res_d.AddOptions( options );
        isFull = true;
    }

    public void SetRes( int idx )
    {
        Resolution res = resolutions[idx];
        Screen.SetResolution( res.width, res.height, true );
    }

    public void ToggleFullScreen()
    {
        if ( isFull ) {
            Screen.fullScreen = false;
            isFull = false;
        } else {
            Screen.fullScreen = true;
            isFull = true;
        }
    }
}
