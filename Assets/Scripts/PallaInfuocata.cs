using System.Collections;
using UnityEngine;
public class PallaInfuocata:MonoBehaviour{                 // Gestisce una palla di fuoco
    private Rigidbody2D rigidbodyOggetto;
    private SpriteRenderer rendererOggetto;
    private Vector2 capovolta;
    [SerializeField] private int velocità;

////////////////////////////////////////////////////// AWAKE ///////////////////////////////////////////////////////////
    private void Awake(){
        rendererOggetto=GetComponent<SpriteRenderer>();
        rigidbodyOggetto=GetComponent<Rigidbody2D>();
        
        capovolta=new Vector2(1,-1);
        rigidbodyOggetto.linearVelocityY=velocità;}               // Inizializzazione
    
///////////////////////////////////////////////////// RIPARTE //////////////////////////////////////////////////////////
    private IEnumerator Riparte(){
        rendererOggetto.enabled=false;                       // Tolta
        yield return new WaitForSeconds(1);
        rendererOggetto.enabled=true;
        transform.localScale=Vector2.one;
        rigidbodyOggetto.linearVelocityY=velocità;}           // Riparte dopo 1 sec
        
//////////////////////////////////////////////////// COLLISIONE ////////////////////////////////////////////////////////
    private void OnCollisionEnter2D(Collision2D collision){
        var nameGameObject=collision.gameObject.name.Split()[0];        // Tolgo il numero
        
        switch(nameGameObject){
            case "CuboNonDistruttibile":                // Inversione
                transform.localScale=capovolta;
                rigidbodyOggetto.linearVelocityY=-velocità;
                break;
            case "Pianta":                                   // Solo la pianta conta
                StartCoroutine(Riparte());
                break;}}}
                 