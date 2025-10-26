using System.Collections;
using UnityEngine;
public class SpawnerOggetti:MonoBehaviour{          // Gestisce un qualsiasi oggetto crea nemico
    private bool routine;
    private Vector2 posLoad;
    [SerializeField] private int pos;
    [SerializeField] private Transform[] nemiciDaAggiungere;
    
///////////////////////////////////////////////////// AWAKE ////////////////////////////////////////////////////////////
    private void Awake(){
        posLoad=transform.position;
        posLoad.y=pos==0? transform.position.y+1 : transform.position.y-1;}        // 0: sopra, 1: sotto
    
//////////////////////////////////////////////////// AVVIO /////////////////////////////////////////////////////////////
    private void OnBecameVisible(){
        routine=true;
        StartCoroutine(InserisciNemici());}          // Avvio
    private void OnBecameInvisible(){
        routine=false;}           // Stop

//////////////////////////////////////////////// ATTESA ////////////////////////////////////////////////////////////////
    private IEnumerator InserisciNemici(){
        var index=0;
        
        while(routine){
            if(index==nemiciDaAggiungere.Length){    // All'infinito, finché non viene fermata
                index=0;}
            
            nemiciDaAggiungere[index].position=posLoad;
            nemiciDaAggiungere[index].gameObject.SetActive(true);          // Via
            yield return new WaitForSeconds(2);
            index++;}}}