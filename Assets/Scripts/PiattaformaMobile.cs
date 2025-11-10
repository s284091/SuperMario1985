using UnityEngine;
public class PiattaformaMobile:MonoBehaviour{
    private Vector2 posMax,posMin,posObj;
    private Rigidbody2D rigidBodyOggetto;
    private const int AltezzaMinimaVisibile=4,AltezzaMassimaVisibile=14;
    [SerializeField] private int velocità;
    [SerializeField] private Transform[] nemiciSopra;
    
////////////////////////////////////////////////// AWAKE ///////////////////////////////////////////////////////////////
    private void Awake(){
        posMax=new Vector2(transform.position.x,AltezzaMassimaVisibile);
        posMin=new Vector2(transform.position.x,AltezzaMinimaVisibile);
        rigidBodyOggetto=GetComponent<Rigidbody2D>();               // Inizializzazione
        rigidBodyOggetto.linearVelocityY=velocità;}
        
///////////////////////////////////////////////////// UPDATE ///////////////////////////////////////////////////////////
    private void Update(){
        if(transform.position.y>AltezzaMassimaVisibile){             // Riappare sotto
            transform.position=posMin;}
        else if(transform.position.y<AltezzaMinimaVisibile){            // Riappare sopra
            transform.position=posMax;}
        
        foreach(var transformNemico in nemiciSopra){           // new pos(x,h_piattaforma+0.1)
            posObj=transformNemico.position;
            posObj.y=transform.position.y+1.1f;
            transformNemico.position=posObj;}}}           // Sposta i nemici