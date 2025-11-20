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
    private readonly Dictionary<(bool,bool),(int,int)> dizionarioProbabilità=new(){
        [(true,true)]=(4,9),
        [(true,false)]=(7,-1),             // Se d>DistanzaMinima -> Se ha fuoco -> PMovimento, PSpara
        [(false,true)]=(1,-1),
        [(false,false)]=(2,-1)};
    
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
                    /*SparaFuoco();*/}
                else{
                    rigidbodyBowser.linearVelocityX=0;
                    rigidbodyBowser.linearVelocityY=VelocitàY;}}                        // Salta

            yield return new WaitForSeconds(TempoLoadY);}}}            // Aspetta così
