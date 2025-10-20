using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class SpawnerOggetti:MonoBehaviour{          // Gestisce un qualsiasi oggetto crea nemici
    private Vector2 posLoad;
    [SerializeField] private List<Transform> nemiciDaAggiungere=new();
    
//////////////////////////////////////////////// AWAKE /////////////////////////////////////////////////////////////////
    private void Awake(){
        posLoad=new Vector2(transform.position.x,transform.position.y+1);}        // Sopra il tubo
    
//////////////////////////////////////////////////// AVVIO /////////////////////////////////////////////////////////////
    private void OnBecameVisible(){
        StartCoroutine(InserisciNemici());}
    
/////////////////////////////////////////////////// RICARICA ///////////////////////////////////////////////////////////
    public void Ricarica(Transform oggetto){
        nemiciDaAggiungere.Add(oggetto);}

//////////////////////////////////////////////// ATTESA ////////////////////////////////////////////////////////////////
    private IEnumerator InserisciNemici(){
        var index=0;
        
        while(index<nemiciDaAggiungere.Count){                       // Finché ne ha e si vede
            nemiciDaAggiungere[index].position=posLoad;
            nemiciDaAggiungere[index].gameObject.SetActive(true);          // Via
            index++;
            yield return new WaitForSeconds(2);}}}