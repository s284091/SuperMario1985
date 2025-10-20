using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class SpawnerOggetti:MonoBehaviour{          // Gestisce un qualsiasi oggetto crea nemico
    [SerializeField] private List<Transform> nemiciDaAggiungere=new();
    
//////////////////////////////////////////////////// AVVIO /////////////////////////////////////////////////////////////
    private void OnBecameVisible(){
        StartCoroutine(nameof(InserisciNemici));}          // Avvio
    private void OnBecameInvisible(){
        StopCoroutine(nameof(InserisciNemici));}           // Stop
    
/////////////////////////////////////////////////// RICARICA ///////////////////////////////////////////////////////////
    public void Ricarica(Transform oggetto){
        nemiciDaAggiungere.Add(oggetto);}

//////////////////////////////////////////////// ATTESA ////////////////////////////////////////////////////////////////
    private IEnumerator InserisciNemici(){
        var index=0;
        var posLoad=new Vector2(transform.position.x,transform.position.y+1);             // Sopra il tubo
        
        while(index<nemiciDaAggiungere.Count){                       // Finché ne ha e si vede
            nemiciDaAggiungere[index].position=posLoad;
            nemiciDaAggiungere[index].gameObject.SetActive(true);          // Via
            index++;
            yield return new WaitForSeconds(2);}}}