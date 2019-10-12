using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControls : MonoBehaviour
{
    public GameObject SFX;
    public GameObject MFX;
    public GameObject Vox;

    public GameObject SFXlabel;
    public GameObject MFXlabel;
    public GameObject Voxlabel;

    public AudioMixer mixer;

    public void ChangeSFX()
    {
        mixer.SetFloat("SFX", Mathf.Log10(SFX.GetComponent<Slider>().value) * 20);
        float volume = SFX.GetComponent<Slider>().value * 100;
        SFXlabel.GetComponent<Text>().text = volume.ToString("0");
    }

    public void ChangeMusic()
    {
        mixer.SetFloat("MFX", (Mathf.Log10(MFX.GetComponent<Slider>().value) * 20) - 5);
        float volume = MFX.GetComponent<Slider>().value * 100;
        MFXlabel.GetComponent<Text>().text = volume.ToString("0");
    }

    public void ChangeVox()
    {
        mixer.SetFloat("VOX", (Mathf.Log10(Vox.GetComponent<Slider>().value) * 20) + 5);
        float volume = Vox.GetComponent<Slider>().value * 100;
        Voxlabel.GetComponent<Text>().text = volume.ToString("0");
    }
}
