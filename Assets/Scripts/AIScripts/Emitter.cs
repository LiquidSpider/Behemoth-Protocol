using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emitter : MonoBehaviour
{

    /// <summary>
    /// The starting time of the emitter
    /// </summary>
    private float startTimer;

    /// <summary>
    /// The life of the emitter
    /// </summary>
    public float Life;

    /// <summary>
    /// the rate at which the emitter emits.
    /// </summary>
    public float growthrate;

    /// <summary>
    /// The amount of damage to deal to the player.
    /// </summary>
    public float Damage;

    // Start is called before the first frame update
    void Start()
    {
        this.startTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {

        // increase scale.
        float scale = this.transform.localScale.x;
        float increase = Mathf.Lerp(scale, 40000, growthrate);
        this.transform.localScale = new Vector3(increase, increase, increase);

        // Destroy this object after it's hit it's life.
        if(this.startTimer >= Life)
        {
            Destroy(this.gameObject);
        }

        // Increment timer.
        this.startTimer += Time.deltaTime;
    }

    /// <summary>
    /// When the collider enters another.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Take damage for the player
            if(other.gameObject.GetComponent<PlayerHealth>())
                other.gameObject.GetComponent<PlayerHealth>().TakeDamage(Damage);
            // Disable the collider.
            this.GetComponent<Collider>().enabled = false;
        }
    }
}
