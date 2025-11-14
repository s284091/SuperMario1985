using System.Collections;
using UnityEngine;
public class SpawnerOggetti:MonoBehaviour{          // Gestisce un qualsiasi oggetto crea nemico
    private bool routine;
    private Vector2 posLoad;
    [SerializeField] private int pos;
    [SerializeField] private Transform[] nemiciDaAggiungere;
    
///////////////////////////////////////////////////// AWAKE ////////////////////////////////////////////////////////////
    private void Awake(){
        switch(pos){
            case 0:
                posLoad=new Vector2(transform.position.x,transform.position.y+1);          // 0: sotto
                break;
            case 1:
                posLoad=new Vector2(transform.position.x,transform.position.y-1);          // 1: sopra
                break;
            case 2:
                posLoad=new Vector2(transform.position.x+2,transform.position.y+0.5f);          // 2: destra
                break;
            default:
                posLoad=new Vector2(transform.position.x-2,transform.position.y+0.5f);          // 3: sinistra
                break;}}
    
//////////////////////////////////////////////////// AVVIO /////////////////////////////////////////////////////////////
    private void OnBecameVisible(){
        routine=true;
        StartCoroutine(InserisciNemici());}          // Avvio
    private void OnBecameInvisible(){
        routine=false;}                                    // Stop

//////////////////////////////////////////////// ATTESA ////////////////////////////////////////////////////////////////
    private IEnumerator InserisciNemici(){
        var index=0;
        
        while(routine){
            if(index==nemiciDaAggiungere.Length){                      // All'infinito, finché non viene fermata
                index=0;}
            
            if(!nemiciDaAggiungere[index].gameObject.activeSelf){            // Se non è già fuori
                nemiciDaAggiungere[index].position=posLoad;
                nemiciDaAggiungere[index].gameObject.SetActive(true);          // Via
                yield return new WaitForSeconds(2);}
            index++;}}}