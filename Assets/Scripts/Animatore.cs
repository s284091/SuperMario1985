using System.Collections;
using UnityEngine;
public class Animatore:MonoBehaviour{
    private string nomeOggetto;
    private float tempo;
    private SpriteRenderer rendererOggetto;
    private Rigidbody2D rigidbodyOggetto;
    [SerializeField] private Sprite std,camminata;
    
/////////////////////////////////////////////// AWAKE //////////////////////////////////////////////////////////////////
    private void Awake(){
        rigidbodyOggetto=GetComponent<Rigidbody2D>();
        rendererOggetto=GetComponent<SpriteRenderer>();
        nomeOggetto=gameObject.name.Split()[0];                    // Toglie il numero
        StartCoroutine(Animazione());}

////////////////////////////////////////////////// ANIMAZIONE //////////////////////////////////////////////////////////
    private IEnumerator Animazione(){
        do{
            switch(nomeOggetto){
                case "Pianta":                // Identico comportamento
                case "Goomba":    
                    rendererOggetto.sprite=rendererOggetto.sprite==std? camminata:std;
                    tempo=0.5f;
                    break;
                case "Mario":
                    if(rigidbodyOggetto.linearVelocity==Vector2.zero){
                        rendererOggetto.sprite=std;}
                    else if(rigidbodyOggetto.linearVelocityX!=0 && rigidbodyOggetto.linearVelocityY==0){
                        rendererOggetto.sprite=rendererOggetto.sprite==std? camminata:std;}
                    tempo=0.2f;
                    break;}
            yield return new WaitForSeconds(tempo);}while(tempo>0);}}