using System.Collections;
using UnityEngine;
public class OggettoConFuoco:MonoBehaviour{                 // Gestisce i lanciafiamme o le barre di fuoco
    private bool ruota;
    private CapsuleCollider2D colliderOggetto;
    private SpriteRenderer rendererOggetto;
    [SerializeField] private int versoRotazione;
    [SerializeField] private Vector2 puntoFisso;
    
/////////////////////////////////////////////// AWAKE //////////////////////////////////////////////////////////////////
    private void Awake(){
        rendererOggetto=GetComponent<SpriteRenderer>();
        colliderOggetto=GetComponent<CapsuleCollider2D>();             // Legge i componenti e decide se ruota
        ruota=name.Contains("Barra");
        StartCoroutine(Opera());}
    
///////////////////////////////////////////// COROUTINE MOVIMENTO //////////////////////////////////////////////////////
    private IEnumerator Opera(){           // Se ruota=true è barra rotante (finché esiste)
        while(Time.timeScale>0){
            if(ruota){
                transform.RotateAround(puntoFisso,Vector3.forward,versoRotazione);   // Ruota
                yield return null;}
            else{                                                    // Sennò è lanciafiamme temporizzato
                yield return new WaitForSeconds(2);
                rendererOggetto.enabled=!rendererOggetto.enabled;
                colliderOggetto.enabled=!colliderOggetto.enabled;}}}}                 // Stop/Via