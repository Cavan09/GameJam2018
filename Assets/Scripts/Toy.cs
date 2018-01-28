using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toy : MonoBehaviour {

    [Range(0.0f, 2.0f)]
    public float m_Momentum = 1.0f;
    public Animator m_Anim { get; set; }

    Renderer m_Renderer { get; set; }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    internal float UpdateMometum(float feed)
    {
        m_Momentum += feed;
        return (feed * -1);
    }
}
