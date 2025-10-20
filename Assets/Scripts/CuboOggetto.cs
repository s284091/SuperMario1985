using System.Collections;
using UnityEngine;
class CuboOggetto:MonoBehaviour{               // Gestisce che cosa uscirà dal cubo oggetto
    private SpriteRenderer rendererCuboOggetto;
    [SerializeField] private Transform oggettoNascosto;
    [SerializeField] private Sprite cuboVuoto;
    
/////////////////////////////////////////////// AWAKE //////////////////////////////////////////////////////////////////    
    private void Awake(){
        rendererCuboOggetto=GetComponent<SpriteRenderer>();}
    
///////////////////////////////////////////////// MONETA ///////////////////////////////////////////////////////////////
    public bool PresenzaMoneta(){
        return oggettoNascosto.name.Contains("Moneta");}
    
///////////////////////////////////////////////// GIA' VISTO ///////////////////////////////////////////////////////////
    public bool GetRilasciato(){
        return rendererCuboOggetto.sprite==cuboVuoto;}
    
///////////////////////////////////////////// CANCELLA LA MONETA ///////////////////////////////////////////////////////
    private IEnumerator CancellazioneMoneta(){
        yield return new WaitForSeconds(1);                    // La toglie dopo 1 secondo
        oggettoNascosto.gameObject.SetActive(false);}
 
//////////////////////////////////////////////// RILASCIA OGGETTO //////////////////////////////////////////////////////
    public void Rilascia(){
        oggettoNascosto.gameObject.SetActive(true);
        rendererCuboOggetto.sprite=cuboVuoto;        // Via

        if(oggettoNascosto.name.Contains("Moneta")){
            StartCoroutine(CancellazioneMoneta());}}}            // Toglie la moneta