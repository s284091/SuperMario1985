using UnityEngine;
public class OggettoMobile:MonoBehaviour{                 // Gestisce un qualsiasi oggetto mobile
    private SpriteRenderer rendererOggetto;
    private Rigidbody2D rigidbodyOggetto;
    private string nomeGo;
    private const int AltezzaMinimaVisibile=3;
    [SerializeField] private int velocità;
    
////////////////////////////////////////////////////// AWAKE ///////////////////////////////////////////////////////////
    private void Awake(){
        nomeGo=name.Split()[0];
        rigidbodyOggetto=GetComponent<Rigidbody2D>();
        rendererOggetto=GetComponent<SpriteRenderer>();}               // Inizializzazione
    
//////////////////////////////////////////////////////// UPDATE ////////////////////////////////////////////////////////
    private void Update(){
        rigidbodyOggetto.linearVelocityX=rigidbodyOggetto.linearVelocityY==0? velocità : 0;
        if(transform.position.y<AltezzaMinimaVisibile){          // Disattivato
            gameObject.SetActive(false);}}
            
////////////////////////////////////////////// COLLISIONI //////////////////////////////////////////////////////////////
    private void OnCollisionEnter2D(Collision2D collision){  
        if(nomeGo=="BulletBill"){                       // Il bullet esplode se si schianta
            if(transform.localScale.x>1 && collision.gameObject.name.Contains("CuboDistruttibileBill")){
                collision.gameObject.SetActive(false);}
            else{
                gameObject.SetActive(false);}}
        else if(Mathf.Abs(collision.transform.position.y-transform.position.y)<1){    // |Y1-Y2|<1 -> stesso piano
            velocità=-velocità;
            rendererOggetto.flipX=!rendererOggetto.flipX;}}}