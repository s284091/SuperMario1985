using UnityEngine;
public class OggettoMobile:MonoBehaviour{                 // Gestisce un qualsiasi oggetto mobile autonom
    private SpriteRenderer rendererOggetto;
    private Rigidbody2D rigidbodyOggetto;
    [SerializeField] private int velocità,altezzaMinimaVisibile;
    
////////////////////////////////////////////////////// AWAKE ///////////////////////////////////////////////////////////
    private void Awake(){
        rigidbodyOggetto=GetComponent<Rigidbody2D>();
        rendererOggetto=GetComponent<SpriteRenderer>();}               // Inizializzazione
    
//////////////////////////////////////////////////////// UPDATE ////////////////////////////////////////////////////////
    private void Update(){
        if(!rendererOggetto.isVisible){                   // Si muove se si vede
            return;}
        
        rigidbodyOggetto.linearVelocityX=rigidbodyOggetto.linearVelocityY==0? velocità : 0;
        if(transform.position.y<altezzaMinimaVisibile){
            gameObject.SetActive(false);}}                            // Non ha più motivo di esserci
            
////////////////////////////////////////////// COLLISIONI //////////////////////////////////////////////////////////////
    private void OnCollisionEnter2D(Collision2D collision){              // |Y1-Y2|<1 -> stesso piano
        if(Mathf.Abs(collision.transform.position.y-transform.position.y)<1){
            velocità=-velocità;
            rendererOggetto.flipX=!rendererOggetto.flipX;}}}