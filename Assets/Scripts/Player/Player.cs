using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    
    public Life life;
    public Life shield;
    public GameController gController;
    private Vector3 spawnPoint;

    public AudioManager soundsManager;
    public Transform shotPos;

    private void Start()
    {
        spawnPoint = new Vector3(0, 6, 0);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Item item = gController.Use();
            if (item != null)
            {
                if (item.name == "bomb" && item.prefab != null)
                {
                    GameObject projectil = Instantiate(item.prefab, shotPos.position, shotPos.rotation);
                    Rigidbody rb = projectil.GetComponent<Rigidbody>();
                    rb.AddForce((transform.forward + new Vector3(0f, 0.2f, 0f)) * 10f, ForceMode.VelocityChange);
                }
                else if (item.name == "shield")
                {
                    shield.current = shield.max;
                }
                else if (item.name == "potion")
                {
                    life.current += Mathf.Min(life.max - life.current, 50);
                }
            }
        }
    }
    public void TakeDamage(int damage)
    {
        soundsManager.PlaySound("hurt");
        gController.setHitFeedback();

        if (shield.current == 0)
        {
            life.current -= damage;
        }
        else if (shield.current >= damage)
        {
            shield.current -= damage;
        }
        else
        {
            life.current -= (damage - shield.current);
            shield.current = 0;
        }
        if (life.current < 0)
        {
            life.current = 0;
        }
        CheckDeath();
    }

    private void CheckDeath()
    {
        if (life.current <= 0)
        {
            gController.GameOver();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Projectil")
        {
            Projectil s = collision.gameObject.GetComponent<Projectil>();
            TakeDamage(s.damage);
        }
    }

    public void Reset()
    {
        GetComponent<CharacterController>().enabled = false;
        transform.position = spawnPoint;
        transform.rotation = Quaternion.identity;
        GetComponent<CharacterController>().enabled = true;
        life.current = life.max;
        shield.current = 0;
    }

}
