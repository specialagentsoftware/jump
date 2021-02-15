using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class CameraScript : MonoBehaviour
{
    [SerializeField] Text lvldisplay;
    [SerializeField] Text hpdisplay;
    [SerializeField] Text fuelDisplay;
    Camera cam;
    Canvas canvas;
    Transform tmplist;

    // Start is called before the first frame update
    void Start()
    {
        lvldisplay.text = "0";
        hpdisplay.text = "100";
        fuelDisplay.text = "100";
    }

    public void SetLvl(float lvl)
    {
        lvldisplay.text = lvl.ToString();
    }

    public void SetHp(int damage)
    {
        int hp = Convert.ToInt32(hpdisplay.text);
        hp -= damage;
        if(hp > 0)
        {
            hpdisplay.text = hp.ToString();
        }
        else
        {
            hpdisplay.text = "0";
            hpdisplay.color = new Color(255, 0, 0);
        }
        
    }

    public void UseFuel(int fuelused)
    {
        int fuel = Convert.ToInt32(fuelDisplay.text);
        fuel -= fuelused;
        if(fuel > 0)
        {
            fuelDisplay.text = fuel.ToString();
            fuelDisplay.color = new Color(255, 255, 255);
        }
        else
        {
            fuelDisplay.text = "0";
            fuelDisplay.color = new Color(255, 0, 0);
        }
    }

    public void SetFuel(int fuel)
    {
        fuelDisplay.text = Convert.ToString(fuel);
    }

    public int GetFuel()
    {
        int fuel = Convert.ToInt32(fuelDisplay.text);
        return fuel;
    }

    public int GetHp()
    {
        int hp = Convert.ToInt32(hpdisplay.text);
        return hp;
    }

    public int GetLvl()
    {
        int lvl = Convert.ToInt32(lvldisplay.text);
        return lvl;
    }
}
