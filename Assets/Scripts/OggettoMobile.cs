using UnityEngine;
public class OggettoMobile:MonoBehaviour{                 // Gestisce un qualsiasi oggetto mobile
    private SpriteRenderer rendererOggetto;
    private Rigidbody2D rigidbodyOggetto;
    private const int AltezzaMinimaVisibile=3;
    [SerializeField] private SpawnerOggetti sorgente;
    [SerializeField] private int velocità;
    
////////////////////////////////////////////////////// AWAKE ///////////////////////////////////////////////////////////
    private void Awake(){
        rigidbodyOggetto=GetComponent<Rigidbody2D>();
        rendererOggetto=GetComponent<SpriteRenderer>();}               // Inizializzazione
    
//////////////////////////////////////////////////////// UPDATE ////////////////////////////////////////////////////////
    private void Update(){
        if(!rendererOggetto.isVisible){
            return;}
        
        rigidbodyOggetto.linearVelocityX=rigidbodyOggetto.linearVelocityY==0? velocità : 0;
        if(transform.position.y<AltezzaMinimaVisibile){          // Disattivato
            gameObject.SetActive(false);
            if(sorgente){
                sorgente.Ricarica(transform);}}}          // Ricaricato in lista
            
////////////////////////////////////////////// COLLISIONI //////////////////////////////////////////////////////////////
    private void OnCollisionEnter2D(Collision2D collision){              // |Y1-Y2|<1 -> stesso piano
        if(Mathf.Abs(collision.transform.position.y-transform.position.y)<1){
            velocità=-velocità;
            rendererOggetto.flipX=!rendererOggetto.flipX;}}}