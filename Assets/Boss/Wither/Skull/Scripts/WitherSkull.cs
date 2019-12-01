using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using System;

[RequireComponent(typeof(AudioSource))]
public class WitherSkull : MonoBehaviour
{
    private Vector3 destination;
    public GameObject[] particles;
    public float speed = 5f;

    public int hitDamage = 10;
    public int explodeDamage = 5;


    private bool isAlive = true;
    private AudioSource source;
    void Start()
    {
        source = GetComponent<AudioSource>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        destination = player.transform.position;
    }

    void Update()
    {
        if (gameObject != null)
        {
            if (transform.position == destination && isAlive) {
                Explode();                
            }
            else
            {
                transform.LookAt(destination);
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(-90, 0, 90));
                transform.position = Vector3.MoveTowards(transform.position, destination, speed);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (gameObject != null && isAlive && gameObject.GetComponent<WitherBodyPart>() == null)
        {
            Explode();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(explodeDamage);
            }
        }
    }

    private async void Explode()
    {
        isAlive = false;
        source.Play();
        GetComponent<MeshRenderer>().enabled = false;
        ArrayList tmp_p = new ArrayList();
        float delay = source.clip.length;
        foreach (GameObject particle in particles)
        {
            GameObject p = Instantiate(particle, transform.position,transform.rotation);
            tmp_p.Add(p);
            if (particle.GetComponent<ParticleSystem>() != null)
            {
                delay = Mathf.Max(delay, particle.GetComponent<ParticleSystem>().main.duration);
                await Task.Delay(TimeSpan.FromSeconds(delay));
            }
        }
       
        if (gameObject != null)
        {
            Destroy(gameObject);
        }
        foreach(GameObject particle in tmp_p)
        {
            Destroy(particle);
        }
    }
    
    
}
