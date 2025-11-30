using System.Collections;
using UnityEngine;
class CuboOggetto:MonoBehaviour{               // Gestisce che cosa uscirà dal cubo oggetto
    private SpriteRenderer rendererCuboOggetto;
    [SerializeField] private AudioSource audioCamera;
    [SerializeField] private Transform oggettoNascosto;
    [SerializeField] private Sprite cuboVuoto;
    [SerializeField] private GestorePartita cameraPartita;
    
/////////////////////////////////////////////// AWAKE //////////////////////////////////////////////////////////////////    
    private void Awake(){
        rendererCuboOggetto=GetComponent<SpriteRenderer>();}
    
///////////////////////////////////////////// CANCELLA LA MONETA ///////////////////////////////////////////////////////
    private IEnumerator CancellazioneMoneta(){
        audioCamera.Play();
        yield return new WaitForSeconds(1);                    // Suona e a toglie dopo 1 secondo
        oggettoNascosto.gameObject.SetActive(false);}
 
//////////////////////////////////////////////// COLLISIONE MARIO //////////////////////////////////////////////////////
    private void OnCollisionEnter2D(Collision2D collision){               // Mario mi colpisce da sotto
        if(collision.transform.position.y>=transform.position.y || rendererCuboOggetto.sprite==cuboVuoto){
            return;}
        
        oggettoNascosto.gameObject.SetActive(true);
        rendererCuboOggetto.sprite=cuboVuoto;        // Via
        if(oggettoNascosto.name.Contains("Moneta")){
            cameraPartita.AggiungiMoneta();
            StartCoroutine(CancellazioneMoneta());}}}            // Toglie la moneta e suona