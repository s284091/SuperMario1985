using System.Collections;
using UnityEngine;
public class PallaInfuocata:MonoBehaviour{                 // Gestisce una palla di fuoco
    private Rigidbody2D rigidbodyOggetto;
    private SpriteRenderer rendererOggetto;
    private CircleCollider2D colliderOggetto;
    private Vector2 capovolta;
    [SerializeField] private int velocità;

////////////////////////////////////////////////////// AWAKE ///////////////////////////////////////////////////////////
    private void Awake(){
        rendererOggetto=GetComponent<SpriteRenderer>();
        rigidbodyOggetto=GetComponent<Rigidbody2D>();             // Componenti
        colliderOggetto=GetComponent<CircleCollider2D>();
        
        capovolta=new Vector2(1,-1);
        rigidbodyOggetto.linearVelocityY=velocità;}               // Inizializzazione
    
///////////////////////////////////////////////////// RIPARTE //////////////////////////////////////////////////////////
    private IEnumerator Riparte(){
        rendererOggetto.enabled=false;                       // Tolta
        colliderOggetto.enabled=false;
        yield return new WaitForSeconds(1);
        rendererOggetto.enabled=true;
        colliderOggetto.enabled=true;
        transform.localScale=transform.localScale.y.Equals(1)? capovolta : Vector2.one;
        rigidbodyOggetto.linearVelocityY=velocità;}           // Riparte dopo 1 sec
        
//////////////////////////////////////////////////// COLLISIONE ////////////////////////////////////////////////////////
    private void OnCollisionEnter2D(Collision2D collision){
        var nameGameObject=collision.gameObject.name.Split()[0];        // Tolgo il numero
        
        switch(nameGameObject){
            case "CuboNonDistruttibile":                // Inversione
                transform.localScale=transform.localScale.y.Equals(1)? capovolta : Vector2.one;
                rigidbodyOggetto.linearVelocityY=-velocità;
                break;
            case "Pianta":                                   // Solo la pianta conta
                StartCoroutine(Riparte());
                break;}}}
                 