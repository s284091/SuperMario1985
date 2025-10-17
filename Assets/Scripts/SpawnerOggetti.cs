using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class SpawnerOggetti:MonoBehaviour{          // Gestisce un qualsiasi oggetto crea nemici
    private Vector2 posLoad=Vector2.zero;
    private int index;
    private bool avvio;
    [SerializeField] private int direzione;
    [SerializeField] private List<Transform> nemiciDaAggiungere=new();
    
//////////////////////////////////////////////// AWAKE /////////////////////////////////////////////////////////////////
    private void Awake(){
        posLoad.x=transform.position.x;
        posLoad.y=direzione==1? transform.position.y-1 : transform.position.y+1;}        // Sotto o sopra il tubo
    
//////////////////////////////////////////////////// AVVIO /////////////////////////////////////////////////////////////
    private void OnBecameVisible(){
        avvio=true;
        if(index==0){
            StartCoroutine(InserisciNemici());}}
    private void OnBecameInvisible(){
        avvio=false;}

//////////////////////////////////////////////// ATTESA ////////////////////////////////////////////////////////////////
    private IEnumerator InserisciNemici(){
        while(index<nemiciDaAggiungere.Count && avvio){                       // Finché ne ha e si vede
            nemiciDaAggiungere[index].position=posLoad;
            nemiciDaAggiungere[index].gameObject.SetActive(true);          // Via
            index++;
            yield return new WaitForSeconds(2);}}}