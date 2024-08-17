using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class IntroCamTarget : MonoBehaviour
{
    public Animation animationClip;

    public void Awake()
    {
        Debug.Log("playing " + animationClip.isPlaying + " auto " + animationClip.playAutomatically);
        //animationClip.Play();
    }
}

