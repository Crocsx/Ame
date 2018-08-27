using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RainDropEffectText : MonoBehaviour
{
    public Outline[] AVAILABLE_OUTLINES;

    List<Outline> outlines;
    float RIPPLE_DELAY = 0.5f;
    float RIPPLE_SPEED = 0.5f;
    float MAX_RIPPLE_DIST = 35f;
    float MIN_RIPPLE_ALPHA = 0f;
    float MAX_RIPPLE_ALPHA = 0.4f;

    float timer = 0;

    private void Start()
    {
        outlines = new List<Outline>();
        Unselect();
    }
    // Update is called once per frame
    void Update ()
    {
        OutlineTimer();
        for(int i = 0; i< outlines.Count; i++)
        {
            HandleOutline(outlines[i]);
        }
    }

    public void Unselect()
    {

    }

    public void Select()
    {

    }

    void OutlineTimer()
    {
        if(outlines.Count != AVAILABLE_OUTLINES.Length)
        {
            timer += Time.deltaTime;
            if(timer > RIPPLE_DELAY)
            {
                AddNewRipple();
                timer = 0;
            }
        }
    }

    void AddNewRipple()
    {
        int index = outlines.Count;
        outlines.Add(AVAILABLE_OUTLINES[index]);
    }

    void HandleOutline(Outline outl)
    {
        outl.effectDistance += new Vector2(RIPPLE_SPEED, RIPPLE_SPEED);
        Color currColor = outl.effectColor;
        currColor.a = Mathf.Lerp(MAX_RIPPLE_ALPHA, MIN_RIPPLE_ALPHA, outl.effectDistance.magnitude / MAX_RIPPLE_DIST);
        outl.effectColor = currColor;
        if (outl.effectDistance.magnitude > MAX_RIPPLE_DIST)
        {
            outl.effectDistance = new Vector2(0.1f, 0.1f);
        }
    }
}
