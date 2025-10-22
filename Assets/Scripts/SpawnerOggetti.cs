using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class SpawnerOggetti:MonoBehaviour{          // Gestisce un qualsiasi oggetto crea nemico
    private bool routine;
    [SerializeField] private List<Transform> nemiciDaAggiungere=new();
    
//////////////////////////////////////////////////// AVVIO /////////////////////////////////////////////////////////////
    private void OnBecameVisible(){
        routine=true;
        StartCoroutine(InserisciNemici());}          // Avvio
    private void OnBecameInvisible(){
        routine=false;}           // Stop

//////////////////////////////////////////////// ATTESA ////////////////////////////////////////////////////////////////
    private IEnumerator InserisciNemici(){
        var index=0;
        var posLoad=new Vector2(transform.position.x,transform.position.y+1);             // Sopra il tubo
        
        while(routine){                       // Finché si vede
            if(!nemiciDaAggiungere[index].gameObject.activeSelf){
                nemiciDaAggiungere[index].position=posLoad;
                nemiciDaAggiungere[index].gameObject.SetActive(true);}          // Via
            index++;
            
            if(index==nemiciDaAggiungere.Count){              // All'infinito, finché non viene fermata
                index=0;}
            yield return new WaitForSeconds(2);}}}