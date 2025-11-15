using System.Collections;
using UnityEngine;
public class SpawnerOggetti:MonoBehaviour{          // Gestisce un qualsiasi oggetto crea nemico
    private bool routine;
    [SerializeField] private int tempoDiRitorno;
    [SerializeField] private Vector2 posLoad;
    [SerializeField] private Transform[] nemiciDaAggiungere;
    
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
                yield return new WaitForSeconds(tempoDiRitorno);}
            index++;}}}