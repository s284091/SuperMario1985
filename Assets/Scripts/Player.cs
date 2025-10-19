using System.Collections;
using UnityEngine;
public class Player:MonoBehaviour{                         // Gestisce i movimenti del giocatore
    private PlayerInput input;
    private SpriteRenderer rendererMario;
    private Rigidbody2D rigidBodyMario;
    private AudioSource[] audioGame=new AudioSource[3];                     // [Salto,PowerUp/Down,Oggetti]
    private short direzione;
    private float posYCollider;                    // Variabili di appoggio
    private string nameObject;
    private bool boolCollisioneCubo,boolInvincibile;
    private Vector2 marioPiccolo,marioGrande;
    private const int NLampeggi=6,FattoreSpostamento=10,FattoreSalto=30;
    [SerializeField] private int altezzaMinimaVisibile;
    [SerializeField] private Sprite marioSalta,marioMorto;
    [SerializeField] private GestorePartita mainSchermo;
    [SerializeField] private AudioClip salto,danno,powerUp,moneta,blocco,kill;
    
///////////////////////////////////////////////// AWAKE ////////////////////////////////////////////////////////////////
    private void Awake(){
        marioPiccolo=new Vector2(1,1);                  // Dimensioni
        marioGrande=new Vector2(1.2f,1.5f);
        input=GetComponent<PlayerInput>();                 // Carica i componenti (stesso GameObject -> GetComponent)
        rendererMario=GetComponent<SpriteRenderer>();
        rigidBodyMario=GetComponent<Rigidbody2D>();
        audioGame=GetComponents<AudioSource>();}
    
///////////////////////////////////////////////// INVINCIBILITA' ///////////////////////////////////////////////////////
    private IEnumerator Invincibile(){
        var n=0;
        boolInvincibile=true;
        gameObject.layer=LayerMask.NameToLayer("Ghost");        // Il layer "Ghost" disabilita la collisione
        
        while(n<NLampeggi){
            rendererMario.enabled=!rendererMario.enabled;            // Lampeggio
            n++;
            yield return new WaitForSeconds(0.25f);}
        boolInvincibile=false;
        gameObject.layer=LayerMask.NameToLayer("Default");}        // Ricomincia a subire
    
/////////////////////////////////////////////////// GESTIONE POWERUP ///////////////////////////////////////////////////
    private void SwitchPowerUp(bool up){               // Lo potenzia o indebolisce
        if(up){
            audioGame[1].clip=powerUp;           // Suono
            audioGame[1].Play();
            transform.localScale=marioGrande;}
        else if(transform.localScale.y>1){
            audioGame[1].clip=danno;           // Suono
            audioGame[1].Play();
            StartCoroutine(Invincibile());
            transform.localScale=marioPiccolo;}
        else{
            rendererMario.sprite=marioMorto;
            StartCoroutine(mainSchermo.Morte());}}                      // Morte

//////////////////////////////////////////////// UPDATE ////////////////////////////////////////////////////////////////
    private void Update(){
        if(Time.timeScale==0){
            return;}
        if(input.Pausa()){
            mainSchermo.SetPausa();                 // In pausa
            return;}
        
        direzione=input.GetDirezione();
        if(transform.position.y<altezzaMinimaVisibile){        // Cade
            StartCoroutine(mainSchermo.Morte());}
        else if(input.GetSalto() && rigidBodyMario.linearVelocity.y==0){
            audioGame[0].Play();
            rendererMario.sprite=marioSalta;
            rigidBodyMario.linearVelocityY=FattoreSalto;}                  // Salta
        
        rigidBodyMario.linearVelocityX=FattoreSpostamento*direzione;
        if(direzione!=0){
            rendererMario.flipX=direzione<0;}}                                    // Flip
            
////////////////////////////////////////////////// COLLISIONI //////////////////////////////////////////////////////////
    private void OnCollisionEnter2D(Collision2D collisione){              // |Y1-Y2|<0.5 -> stesso piano
        posYCollider=collisione.transform.position.y;
        nameObject=collisione.gameObject.name.Split()[0];      // Tolgo il numero dietro
        
        if(nameObject=="Fungo"){                             // Tocca il fungo
            mainSchermo.AggiungiPunti(1000);
            collisione.gameObject.SetActive(false);
            SwitchPowerUp(true);}
        else if(nameObject=="Moneta"){
            collisione.gameObject.SetActive(false);
            audioGame[2].clip=moneta;
            audioGame[2].Play();
            mainSchermo.AggiungiMoneta();}                   // Monetina
        else if(nameObject=="Pianta" && !boolInvincibile){
            SwitchPowerUp(false);}                      // Colpito da pianta (rischio di 2 assieme)
        else if(nameObject=="Asta"){
            StartCoroutine(mainSchermo.Vittoria());}
        
        //// ASSE X
        else if(Mathf.Abs(posYCollider-transform.position.y)<1 && nameObject=="Goomba" && !boolInvincibile){
            SwitchPowerUp(false);}                                  // Colpito dal Goomba
        
        //// ASSE Y
        else{
            if(posYCollider>transform.position.y){
                if(nameObject=="Goomba"){                       // Mi cade addosso
                    SwitchPowerUp(false);}
                else if(!boolCollisioneCubo){
                    switch(nameObject){
                        case "CuboDistruttibile":                // Distrugge il cubo se grande
                            boolCollisioneCubo=true;
                            if(transform.localScale.y>1){
                                mainSchermo.AggiungiPunti(50);
                                audioGame[2].clip=blocco;
                                audioGame[2].Play();
                                collisione.gameObject.SetActive(false);}
                            else{
                                StartCoroutine(mainSchermo.CuboVuoto(collisione.gameObject.transform));}
                            break;
                        case "CuboOggetto":
                            boolCollisioneCubo=true;
                            if(mainSchermo.RilasciaOggetto(collisione.gameObject.name)){
                                mainSchermo.AggiungiMoneta();
                                audioGame[2].clip=moneta;       // Suono moneta
                                audioGame[2].Play();}
                            break;}}}
            else if(nameObject=="Goomba"){                 // Nemico
                mainSchermo.AggiungiPunti(100);                 // Nuovi punti
                audioGame[2].clip=kill;
                audioGame[2].Play();
                collisione.gameObject.SetActive(false);
                rigidBodyMario.linearVelocityY=FattoreSalto/2f;}            // Rimbalzo
            else{
                boolCollisioneCubo=false;}}}}        // Terra