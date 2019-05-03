using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Start is called before the first frame update
    public float health = 200f;
    public bool soundRandomPitch = false;
    public AudioClip death;
    public GameObject sndSrc;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damage) {   // damage = Base Damage, pen = Armour Penetration
        health -= damage;
        if (health <= 0) {
            Die();
        }
    }

    void Die() {
        MakeSound(death, death.length, false);
        Destroy(gameObject);
    }

    void MakeSound(AudioClip sound, float sLength, bool isRandom) {
        AudioSource source = sndSrc.GetComponent<AudioSource>();
        source.clip = sound;
        source.volume = 0.5f;
        if (isRandom) source.pitch = Random.Range(0.75f, 1.25f);
        GameObject oSound = Instantiate(sndSrc, transform.position, transform.rotation);
        oSound.GetComponent<TimedDestroy>().maxTime = sLength;
    }
}
