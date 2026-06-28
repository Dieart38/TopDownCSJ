using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeController : MonoBehaviour
{
    [SerializeField] private float treeHealth;
    [SerializeField] private Animator anim;

    [SerializeField] private GameObject woodPrefab;
    [SerializeField] private int totalWood;

    [SerializeField] private ParticleSystem folhas;

    bool isCut = true;

    void Start()
    {
       
        totalWood = Random.Range(1, 5);
    }

    public void OnHit()
    {
        treeHealth--;

        //anim.SetTrigger("isHit");


        if (treeHealth <= 0)
        {
            if (isCut)
            {
                for (int i = 0; i < totalWood; i++)
                {   // cria o toco e instancia os drops(madeira)
                    Instantiate(woodPrefab, transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f), transform.rotation);
                    isCut = false;
                }
            }


            anim.SetTrigger("cut");
        }
        else
        {
            folhas.Play();
            anim.SetTrigger("isHit");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Axe"))
        {

            OnHit();
        }
    }
}
