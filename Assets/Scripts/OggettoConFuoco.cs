using System.Collections;
using UnityEngine;
public class OggettoConFuoco:MonoBehaviour{                 // Gestisce i lanciafiamme o le barre di fuoco
    private bool ruota;
    private CapsuleCollider2D colliderOggetto;
    private SpriteRenderer rendererOggetto;
    
/////////////////////////////////////////////// AWAKE //////////////////////////////////////////////////////////////////
    private void Awake(){
        rendererOggetto=GetComponent<SpriteRenderer>();
        colliderOggetto=GetComponent<CapsuleCollider2D>();             // Legge i componenti e decide se ruota
        ruota=name.Contains("Rotante");
        StartCoroutine(Opera());}
    
///////////////////////////////////////////// COROUTINE MOVIMENTO //////////////////////////////////////////////////////
    private IEnumerator Opera(){           // Se ruota=true è barra rotante (finché esiste)
        while(rendererOggetto){
            if(ruota){
                transform.Rotate(Vector3.forward,1);}
            else{                                                    // Sennò è lanciafiamme temporizzato
                yield return new WaitForSeconds(2);
                rendererOggetto.enabled=!rendererOggetto.enabled;
                colliderOggetto.enabled=!colliderOggetto.enabled;}}}}                 // Stop/Via