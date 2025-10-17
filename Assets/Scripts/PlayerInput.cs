using UnityEngine;
public class PlayerInput:MonoBehaviour{                   // Riceve i comandi da tastiera
    private short direzione;
    
///////////////////////////////// PER RENDERE PUBBLICA LA DIREZIONE ////////////////////////////////////////////////////
    public short GetDirezione(){              // Per condividere la direzione
        return direzione;}
    
////////////////////////////////////////////// PAUSA ///////////////////////////////////////////////////////////////////
    public bool Pausa(){
        return Input.GetKeyDown(KeyCode.Escape);}            // Vai in pausa
    
////////////////////////////////////// PER SAPERE SE VUOLE SALTARE /////////////////////////////////////////////////////
    public bool GetSalto(){
        return Input.GetKeyDown(KeyCode.Space);}            // Salta
    
//////////////////////////////////////////////// UPDATE ////////////////////////////////////////////////////////////////
    private void Update(){
        if(Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)){              // Vai a sinistra
            direzione=-1;}
        else if(Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D)){           // Vai a destra
            direzione=1;}
        else{
            direzione=0;}}}              // Non si muove