using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedGunUI : MonoBehaviour
{
    public static EquippedGunUI manager;

    [SerializeField] Sprite def;
    [SerializeField] Sprite smg;
    [SerializeField] Sprite rif;
    [SerializeField] Sprite lmg;
    [SerializeField] Sprite deg;

    private Image img;

    void Awake()
    {
        if ( manager == null ) {
            manager = this;
        } else {
            Destroy( this );
        }

        img = this.GetComponent<Image>();
    }

    public void ChangeSprite( string gun )
    {
        switch ( gun ) {
            case "smg":
                img.sprite = smg;
                break;
            case "rifle":
                img.sprite = rif;
                break;
            case "lmg":
                img.sprite = lmg;
                break;
            case "deagle":
                img.sprite = deg;
                break;
            default:
                img.sprite = def;
                break;
        }
    }
}
