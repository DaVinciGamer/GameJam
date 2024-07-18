using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StateBar : MonoBehaviour
{
    float max_value = 100.0f;
    public static float inv_curr;
    public static float or_curr;
    float inc_speed = 1.9f;
    float dec_speed = 1.8f;
    public static float danger_zone_trigger = 95.0f; //triggers warning
    private VarInvertedWorld varInvertedWorld;

    public bool isDangerActive;

    public Slider or_slider;
    public Slider inv_slider;

    public float rotationDuration = 0.3f;
    public float rotationAngle = 15f;

    public RectTransform orImage;
    public RectTransform invImage;

    private Tween orRotationTween;
    private Tween invRotationTween;
    private bool orAnimationActive = false;
    private bool invAnimationActive = false;

    private float elapsed = 0.0f;
    private float barElapsed = 0.0f;
    public float updateInterval = 1.0f;
    private MusicController MusicController;

    private PlayerClass playerClass;
    public Slider healthSlider;


    void Start()
    {
        //set to default value
        inv_slider.value = or_slider.value = inv_curr = or_curr = max_value;
        varInvertedWorld = FindObjectOfType<VarInvertedWorld>();
        playerClass = FindAnyObjectByType<PlayerClass>();
        isDangerActive = false;
    }

    void Update()
    {
        elapsed += Time.deltaTime;
        barElapsed += Time.deltaTime;
        if (elapsed >= updateInterval)
        {
            elapsed = 0.0f;
            UpdateSliders();
        }
        if(inv_curr <= 0 || or_curr <= 0)
        {
            if (barElapsed >= updateInterval)
            {
                barElapsed = 0.0f;
                playerClass.currentHealth -= 5;
                healthSlider.value = (float)playerClass.currentHealth / playerClass.maxHealth;
            }
        }
    }

    void UpdateSliders()
    {
        if (VarInvertedWorld.invertedWorld != "true")
        {
            if (or_curr > 0)
            {
                if (or_curr <= danger_zone_trigger)
                {
                    if (!orAnimationActive)
                    {
                        orStartRotation();
                        orAnimationActive = true;
                    }
                }
                else
                {
                    if (orAnimationActive)
                    {
                        orStopRotation();
                        orAnimationActive = false;
                    }
                }
                or_curr -= dec_speed;
                or_slider.value = or_curr;
            }
            else
            {
                if (orAnimationActive)
                {
                    orStopRotation();
                    orAnimationActive = false;
                }
            }

            if (inv_curr < max_value)
            {
                inv_curr += inc_speed;
                inv_slider.value = inv_curr;
                if (invAnimationActive)
                {
                    invStopRotation();
                    invAnimationActive = false;
                }
            }
        }
        else
        {
            if (inv_curr > 0)
            {
                if (inv_curr <= danger_zone_trigger)
                {
                    if (!invAnimationActive)
                    {
                        invStartRotation();
                        invAnimationActive = true;
                    }
                }
                else
                {
                    if (invAnimationActive)
                    {
                        invStopRotation();
                        invAnimationActive = false;
                    }
                }
                inv_curr -= dec_speed;
                inv_slider.value = inv_curr;
            }
            else
            {
                if (invAnimationActive)
                {
                    invStopRotation();
                    invAnimationActive = false;
                }
            }

            if (or_curr < max_value)
            {
                or_curr += inc_speed;
                or_slider.value = or_curr;
                if (orAnimationActive)
                {
                    orStopRotation();
                    orAnimationActive = false;
                }
            }
        }
    }

    //rotation animation based on https://www.youtube.com/watch?v=Y8cv-rF5j6c&ab_channel=Tarodev and ChatGPT
    void orStartRotation()
    {
        isDangerActive = true;
        //Debug.Log("Start Danger Zone in Original");
        MusicController.Instance.FadeTo(2);
        orRotationTween = orImage.DORotate(new Vector3(0, 0, rotationAngle), rotationDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    void orStopRotation()
    {
        isDangerActive = false;
        // Debug.Log("End Danger Zone in Original");
        MusicController.Instance.FadeTo(1);
        orRotationTween.Kill();
        orImage.rotation = Quaternion.Euler(0, 0, 0);
    }

    void invStartRotation()
    {
        isDangerActive = true;
        Debug.Log("Start Danger Zone in Inv");
        MusicController.Instance.FadeTo(6);
        invRotationTween = invImage.DORotate(new Vector3(0, 0, rotationAngle), rotationDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    void invStopRotation()
    {
        isDangerActive = false;
        MusicController.Instance.FadeTo(5);
        invRotationTween.Kill();
        invImage.rotation = Quaternion.Euler(0, 0, 0);
    }

}
