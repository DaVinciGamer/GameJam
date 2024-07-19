using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Health Bar Idea based on https://www.youtube.com/watch?v=_lREXfAMUcE
public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Camera camera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    void Start()
    {
        // Set the camera to the main camera
        if (camera == null)
        {
            camera = Camera.main;
        }
    }

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = camera.transform.rotation; // Necessary to prevent Health Bar from rotating
        transform.position = target.position + offset;  // Position + Offset --> Show health bar above opponent (y-Value in Inspector)
    }
}
