using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animacoes : MonoBehaviour
{
    public Animator animacao;
    public Player player;

    private bool noChao = false; // true quando toca o chão

    void Start()
    {
        animacao = GetComponent<Animator>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        float velocidadeX = Mathf.Abs(player.rb.velocity.x);
        float velocidadeY = player.rb.velocity.y;

        // ======== CORRER ========
        bool estaCorrendo = velocidadeX > 0.1f && noChao;
        animacao.SetBool("correr", estaCorrendo);

        // ======== PULAR ========
        bool estaPulando = velocidadeY > 0.1f && !noChao;
        animacao.SetBool("pulando", estaPulando);

        // ======== CAIR ========
        bool estaCaindo = velocidadeY < -0.1f && !noChao;
        animacao.SetBool("caindo", estaCaindo);
    }

    // ======== TRIGGER ========
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Chao"))
        {
            noChao = true;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Chao"))
        {
            noChao = false;
        }
    }
}
