using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bowser:MonoBehaviour{                               // Gestisce Bowser
    private int azione,indiceFuoco,maxMovimento,maxSpara;
    private float diffPosizioni;
    private Rigidbody2D rigidbodyBowser;
    private Vector2 posLoad;
    
    private readonly Vector2 capovolto=new(-1,1);
    private readonly Dictionary<(bool,bool),(int,int)> dizionarioProbabilità=new(){     // range(0,9) compresi
        [(true,true)]=(3,9),             // 40% si muove, 60% spara, 0% salta
        [(true,false)]=(7,-1),           // 80% si muove, 0% spara, 20% salta
        [(false,true)]=(1,5),            // 20% si muove, 40% spara, 40% salta
        [(false,false)]=(2,-1)};         // 30% si muove, 0% spara, 70% salta
    
    private const int LoadX=4,TempoLoadY=1,DistanzaMinima=5,VelocitàX=-4,VelocitàY=10,DimP=10;
    [SerializeField] private Rigidbody2D[] palleDiFuoco;
    [SerializeField] private Transform mario;
    
////////////////////////////////////////////////// AWAKE ///////////////////////////////////////////////////////////////    
    private void Awake(){
        posLoad=Vector2.zero;
        
        /*foreach(var fuoco in palleDiFuoco){
            fuoco.linearVelocity=2*Vector2.one;}*/
        rigidbodyBowser=GetComponent<Rigidbody2D>();}              // Inizializzazione
    
///////////////////////////////////////////////////// VISIBILE /////////////////////////////////////////////////////////
    private void OnBecameVisible(){
        StartCoroutine(Agisci());}                // Parte
    
//////////////////////////////////////////////////// SPARA FUOCO ///////////////////////////////////////////////////////
    private void SparaFuoco(){                  // Attiva la palla in posizione indiceFuoco
        Debug.Log("SparaFuoco");
        palleDiFuoco[indiceFuoco].gameObject.SetActive(true);}
    
////////////////////////////////////////////////// AGISCE //////////////////////////////////////////////////////////////
    private IEnumerator Agisci(){
        while(Time.timeScale>0){
            if(mario.position.x>transform.position.x && transform.localScale.x>0){           // Si gira verso di me
                transform.localScale=capovolto;}
            else if(mario.position.x<transform.position.x && transform.localScale.x<0){
                transform.localScale=Vector2.one;}
            
            if(rigidbodyBowser.linearVelocityY==0){             // Sta saltando -> non fa nulla
                azione=Random.Range(0,DimP);                                         // Cosa farà
                diffPosizioni=Math.Abs(mario.position.x-transform.position.x);               // Quanto sono distanti
                indiceFuoco=Array.FindIndex(palleDiFuoco,fuoco=>!fuoco.gameObject.activeSelf);
                (maxMovimento,maxSpara)=dizionarioProbabilità[(diffPosizioni>DistanzaMinima,indiceFuoco>=0)];
                
                if(azione<=maxMovimento){
                    rigidbodyBowser.linearVelocityX=transform.localScale.x*VelocitàX;}     // Si muove
                else if(azione<=maxSpara){
                    rigidbodyBowser.linearVelocityX=0;                // Prima si ferma
                    SparaFuoco();}
                else{
                    rigidbodyBowser.linearVelocityX=0;
                    rigidbodyBowser.linearVelocityY=VelocitàY;}}                        // Salta

            yield return new WaitForSeconds(TempoLoadY);}}}            // Aspetta così
