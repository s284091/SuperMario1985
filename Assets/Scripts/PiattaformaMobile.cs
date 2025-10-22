using UnityEngine;
public class PiattaformaMobile:MonoBehaviour{
    private Vector2 posMax,posMin;
    private Rigidbody2D rigidBodyOggetto;
    private const int AltezzaMinimaVisibile=4,AltezzaMassimaVisibile=14;
    [SerializeField] private int velocità;
    
////////////////////////////////////////////////// AWAKE ///////////////////////////////////////////////////////////////
    private void Awake(){
        posMax=new Vector2(transform.position.x,AltezzaMassimaVisibile);
        posMin=new Vector2(transform.position.x,AltezzaMinimaVisibile);
        rigidBodyOggetto=GetComponent<Rigidbody2D>();               // Inizializzazione
        rigidBodyOggetto.linearVelocityY=velocità;}
        
///////////////////////////////////////////////////// UPDATE ///////////////////////////////////////////////////////////
    private void Update(){
        if(transform.position.y>=AltezzaMassimaVisibile){             // Riappare sotto
            transform.position=posMin;}}}