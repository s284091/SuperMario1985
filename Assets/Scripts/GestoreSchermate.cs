using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GestoreSchermate:MonoBehaviour{
    private string nomeScena;
    [SerializeField] private string nomeScenaIntro;
    [SerializeField] private Button play,exit,noQuit,quit;
    [SerializeField] private Transform warning,panel;
    [SerializeField] private TMP_Text vite,youLost,puntiFinali;
    
/////////////////////////////////////////////// AWAKE //////////////////////////////////////////////////////////////////
    private void Awake(){
        nomeScena=SceneManager.GetActiveScene().name;         // Comportamento diverso in base alla scena
        
        if(nomeScena=="Morte"){
            StartCoroutine(AnimazioneVite());}            // Scena vita in meno
        else if(nomeScena=="Esito"){
            puntiFinali.text=GestorePartita.GetPunti().ToString();
            exit.onClick.AddListener(()=>SceneManager.LoadScene("Intro"));}         // Scena vittoria
        else if(nomeScena=="Intro"){
            play.onClick.AddListener(()=>SceneManager.LoadScene(nomeScenaIntro));
            quit.onClick.AddListener(()=>{Application.Quit(1);});
            exit.onClick.AddListener(()=>Handler(1));
            noQuit.onClick.AddListener(()=>Handler(2));}
        else if(nomeScena.Contains("Livello")){
            play.onClick.AddListener(()=>Handler(0));
            quit.onClick.AddListener(()=>{SceneManager.LoadScene("Intro");});      // Livello
            exit.onClick.AddListener(()=>Handler(1)); 
            noQuit.onClick.AddListener(()=>Handler(2));}}
    
////////////////////////////////////////////// HANDLER /////////////////////////////////////////////////////////////////
    private void Handler(int n){
        switch(n){
            case 0:
                Time.timeScale=1;
                panel.gameObject.SetActive(false);                // Fine pausa
                EventSystem.current.SetSelectedGameObject(null);
                break;
            case 1:
                play.gameObject.SetActive(false);
                exit.gameObject.SetActive(false);
                warning.gameObject.SetActive(true);                // Esci
                noQuit.gameObject.SetActive(true);
                quit.gameObject.SetActive(true);
                break;
            case 2:
                play.gameObject.SetActive(true);
                exit.gameObject.SetActive(true);
                warning.gameObject.SetActive(false);                // Ripensamento
                noQuit.gameObject.SetActive(false);
                quit.gameObject.SetActive(false);
                break;}}
        
///////////////////////////////////////////////// ANIMAZIONE VITE //////////////////////////////////////////////////////
    private IEnumerator AnimazioneVite(){
        var livello=GestorePartita.GetLivello();
        var nVite=GestorePartita.GetVite();              // Ottengo le vite
        vite.text=nVite.ToString();
        yield return new WaitForSecondsRealtime(1);
        nVite--;
        
        if(nVite==0){
            youLost.gameObject.SetActive(true);
            GestorePartita.RestoreVite();
            yield return new WaitForSecondsRealtime(1);
            SceneManager.LoadScene("Intro");}                  // Fine
        
        vite.text=nVite.ToString();
        yield return new WaitForSecondsRealtime(1);                    // Riavvio con una vita tolta
        SceneManager.LoadScene(livello);}}