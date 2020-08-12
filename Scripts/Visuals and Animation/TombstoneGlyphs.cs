using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TombstoneGlyphs : MonoBehaviour
{
    private TextMeshPro dttext;
    private TextMeshPro reset;
    private TextMeshPro next;
    private TextMeshPro cycle;
    private InputController ic;

    void Start()
    {
        ic = FindObjectOfType<InputController>();
        dttext = GameObject.Find("TMPDT").GetComponent<TextMeshPro>();
        reset = GameObject.Find("TMPReset").GetComponent<TextMeshPro>();
        next = GameObject.Find("TMPNext").GetComponent<TextMeshPro>();
        cycle = GameObject.Find("TMPCycle").GetComponent<TextMeshPro>();
        dttext.SetText(ic.Detrizide.ToString());
        reset.SetText(ic.Reset.ToString());
        next.SetText(ic.NextWeapon.ToString());
        cycle.SetText(ic.CycleWeapons.ToString());
    }

}
