using UnityEngine;
public class FuocoBowser:MonoBehaviour{                 // Script solo per la collisione
    
///////////////////////////////////////////// COLLISIONE ///////////////////////////////////////////////////////////////
    private void OnCollisionEnter2D(Collision2D collision){
        var diff=Mathf.Abs(collision.transform.position.y-transform.position.y);
        var nameGameObject=collision.gameObject.name.Split()[0];             // Tolgo il numero
        
        if(nameGameObject=="CuboNonDistruttibile" && diff<=1){                // Si disattiva se collide su X
            gameObject.SetActive(false);}}}