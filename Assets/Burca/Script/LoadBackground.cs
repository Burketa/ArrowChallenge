using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadBackground : MonoBehaviour
{
    public ParticleSystem particle_system_back, particle_system_front;
    public SpriteRenderer bg;
    public ColorCombination[] combinations;
    public float contrast = 3;
    public Image timerColorLight, timerColorDark;

	void Start ()
    {
        int index = Random.Range(0, combinations.Length);

        var ps_back = transform.GetChild(1).GetComponent<ParticleSystemRenderer>();
        var ps_front = transform.GetChild(2).GetComponent<ParticleSystemRenderer>();
        var trail = ps_front.GetComponent<ParticleSystem>().trails;

        bg.sprite = combinations[index].sprite;
        ps_back.material.SetColor("_TintColor", combinations[index].particleColor/ contrast);
        ps_front.material.SetColor("_TintColor", combinations[index].particleColor);
        trail.colorOverLifetime = combinations[index].particleColor/ contrast;

        timerColorLight.color = combinations[index].particleColor;
        timerColorDark.color = combinations[index].particleColor/contrast;
    }
}
