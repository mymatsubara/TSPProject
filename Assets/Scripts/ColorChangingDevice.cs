using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChangingDevice : MonoBehaviour
{
    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void Operate()
    {
        _renderer.material.color = Random.ColorHSV();
    }
}
