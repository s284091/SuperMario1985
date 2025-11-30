using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Bowser:MonoBehaviour{                               // Gestisce Bowser
    private int azione,indiceFuoco,tempoSalto,tempoFuoco;
    private float diffPosizioni;
    private Rigidbody2D rigidbodyBowser;
    private Vector2 posLoad=Vector2.zero,scalaLoad=Vector2.one;
    
    private readonly Vector2 capovolto=new(-1,1);
    private readonly Dictionary<(bool,bool),(int,int)> dizionarioProbabilità=new(){     // range(0,9) compresi
        [(true,true)]=(3,9),             // 40% si muove, 60% spara, 0% salta
        [(true,false)]=(7,-1),           // 80% si muove, 0% spara, 20% salta
        [(false,true)]=(1,5),            // 20% si muove, 40% spara, 40% salta
        [(false,false)]=(2,-1)};         // 30% si muove, 0% spara, 70% salta
    
    private const int LoadX=3,LoadY=1,DistanzaMinima=5,VelocitàX=-4,VelocitàY=10,DimP=10,PosMinX=250;
    private const int TempoRitornoSalto=3,TempoRitornoFuoco=2,TempoAzione=1;
    [SerializeField] private Rigidbody2D[] palleDiFuoco;
    [SerializeField] private Transform mario;
    
////////////////////////////////////////////////// AWAKE ///////////////////////////////////////////////////////////////    
    private void Awake(){
        rigidbodyBowser=GetComponent<Rigidbody2D>();}              // Inizializzazione
    
///////////////////////////////////////////////////// VISIBILE /////////////////////////////////////////////////////////
    private void OnBecameVisible(){
        StartCoroutine(Agisci());}                // Parte
    
//////////////////////////////////////////////////// SPARA FUOCO ///////////////////////////////////////////////////////
    public void SparaFuoco(){                  // Attiva la palla in posizione indiceFuoco
        posLoad.x=transform.position.x-LoadX*transform.localScale.x;
        posLoad.y=transform.position.y+LoadY;
        palleDiFuoco[indiceFuoco].transform.position=posLoad;           // Posizione
        
        scalaLoad.x=transform.localScale.x;
        palleDiFuoco[indiceFuoco].transform.localScale=scalaLoad;       // Scala
        
        palleDiFuoco[indiceFuoco].gameObject.SetActive(true);
        palleDiFuoco[indiceFuoco].linearVelocityX=transform.localScale.x*VelocitàX;}        // Velocità
    
////////////////////////////////////////////////// AGISCE //////////////////////////////////////////////////////////////
    private IEnumerator Agisci(){
        while(azione>=0){
            if(mario.position.x>transform.position.x && transform.localScale.x>0){           // Si gira verso di me
                transform.localScale=capovolto;}
            else if(mario.position.x<transform.position.x && transform.localScale.x<0){
                transform.localScale=Vector2.one;}
            
            if(rigidbodyBowser.linearVelocityY==0){             // Sta saltando -> non fa nulla
                diffPosizioni=Math.Abs(mario.position.x-transform.position.x);               // Quanto sono distanti
                indiceFuoco=Array.FindIndex(palleDiFuoco,fuoco=>!fuoco.gameObject.activeSelf);
                var (maxMovimento,maxSpara)=dizionarioProbabilità[(diffPosizioni>DistanzaMinima,indiceFuoco>=0)];
                
                do{                                          // Cosa farà
                    azione=Random.Range(0,DimP);}
                
                // Non può saltare due volte di seguito ne sparare
                // Come non saltare: azione>maxPrecedente (spara se >0 o movimento)
                // Come non sparare: azione>maxMovimento e azione<maxSpara (se può sparare)
                while((((maxSpara>0 && azione>maxSpara) || (maxSpara<0 && azione>maxMovimento)) && tempoSalto>0)
                      || (azione>maxMovimento && maxSpara>0 && azione<=maxSpara && tempoFuoco>0));
                
                if(azione<=maxMovimento){
                    tempoSalto--;
                    tempoFuoco--;
                    if(transform.localScale.x>0){                    // Mario -> Bowser -> Ascia
                        rigidbodyBowser.linearVelocityX=transform.position.x>PosMinX? VelocitàX : -VelocitàX;}
                    else{
                        rigidbodyBowser.linearVelocityX=-VelocitàX;}}             // Mi insegue
                else if(azione<=maxSpara){
                    tempoSalto--;
                    tempoFuoco=TempoRitornoFuoco;
                    rigidbodyBowser.linearVelocityX=0;                // Prima si ferma
                    SparaFuoco();}
                else{
                    tempoFuoco--;
                    tempoSalto=TempoRitornoSalto;
                    rigidbodyBowser.linearVelocityX=0;
                    rigidbodyBowser.linearVelocityY=VelocitàY;}}          // Salta e non lo rifarà subito

            yield return new WaitForSeconds(TempoAzione);}}}            // Aspetta così
