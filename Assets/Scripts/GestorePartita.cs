using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine.SceneManagement;
public class GestorePartita:MonoBehaviour{
    private CuboOggetto cuboOggettoCollisione;
    private int monete,tempo=150;
    private static int _vite=Vite,_punti,_puntiPrecedenti;
    private AudioSource musica;
    private float posMinX,posMaxX;
    private Vector3 posCamera,posOggetto,scalaOggetto;
    private const float DeltaT=0.0005f,DeltaS=0.2f,DeltaCuboVuoto=0.1f;
    private const int TempoVelocitàDoppia=50,Vite=10;
    private static string _nomeLivello="Livello1";
    [SerializeField] private float hMax;
    [SerializeField] private int terra,hSogliaTop,altezzaMinimaVisibile;
    [SerializeField] private Transform posBandiera;
    [SerializeField] private Transform player,pausaPanel;
    [SerializeField] private List<CuboOggetto> cubiOggetto;
    [SerializeField] private List<TMP_Text> infoPartita;
    [SerializeField] private AudioClip morte,pochiSecondi,musicaStd,musicaVittoria;
    
////////////////////////////////////////////////// AWAKE ///////////////////////////////////////////////////////////////
    private void Awake(){
        var cam=GetComponent<Camera>();                   // Componenti
        musica=GetComponent<AudioSource>();
        Time.timeScale=1;                                      // Azzerato alla morte
        infoPartita[0].text=_punti.ToString();                // Punti
        infoPartita[3].text=_vite.ToString();                // Vite (10 o rimanenti)
        
        posMinX=player.position.x+cam.orthographicSize*cam.aspect-0.5f;         // 0.5 per inquadrare tutto l'oggetto
        posMaxX=posBandiera.position.x-cam.orthographicSize*cam.aspect+1.5f;    // 1.5 per inquadrare tutto l'oggetto
        StartCoroutine(TimerPartita());                 // Avvio timer
        posCamera=new Vector3(posMinX,9.2f,-9);
        transform.position=posCamera;}      // Posizione iniziale
    
//////////////////////////////////////////// INFO PER SCHERMI FINE /////////////////////////////////////////////////////
    public static int GetVite(){
        _punti=_puntiPrecedenti;
        _vite--;                           // Morte
        return _vite+1;}
    public static void RestoreVite(){
        _punti=_puntiPrecedenti=0;
        _nomeLivello="Livello1";
        _vite=Vite;}
    public static int GetPunti(){               // Funzioni di appoggio cambio scena
        var n=_punti;
        _punti=0;
        return n;}
    public static string GetLivello(){
        return  _nomeLivello;}
    
///////////////////////////////////////////////////// TIMER ////////////////////////////////////////////////////////////
    private IEnumerator TimerPartita(){
        while(tempo>0){
            tempo--;
            if(tempo==TempoVelocitàDoppia){
                StartCoroutine(PochiSecRimanenti());}                 // Cambia il suono
            infoPartita[2].text=tempo.ToString();
            yield return new WaitForSeconds(1);}             // Passa 1 sec
        
        StartCoroutine(Morte());}
    
//////////////////////////////////////////////////// 100 SECONDI ///////////////////////////////////////////////////////
    private IEnumerator PochiSecRimanenti(){
        musica.clip=pochiSecondi;
        musica.loop=false;
        musica.Play();

        while(musica.isPlaying){                // Aspetta
            yield return null;}
            
        musica.loop=true;
        musica.clip=musicaStd;              // Più veloce
        musica.pitch=1.5f;
        musica.Play();}

/////////////////////////////////////////////////////// VITTORIA ///////////////////////////////////////////////////////
    public IEnumerator Vittoria(){
        Time.timeScale=0;
        var h=player.position.y<hMax? player.position.y : hMax;
        
        musica.pitch=1;
        musica.Stop();
        musica.loop=false;

        if(h>hSogliaTop){
            AggiungiPunti(5000);}
        else if(h>hSogliaTop-1){
            AggiungiPunti(2000);}                 // Punti in base a dove tocco
        else if(h>hSogliaTop-2){
            AggiungiPunti(800);}
        else if(h>hSogliaTop-3){
            AggiungiPunti(400);}
        else{
            AggiungiPunti(100);}
        yield return new WaitForSecondsRealtime(0.5f);
        
        while(posBandiera.position.y<h){                           // La bandiera sale, mario scende
            yield return new WaitForSecondsRealtime(DeltaT);
            player.position=new Vector2(player.position.x,player.position.y-DeltaS);
            posBandiera.position=new Vector2(posBandiera.position.x,posBandiera.position.y+DeltaS);}
        
        while(player.position.y>terra){                                // Mario scende fino a terra
            yield return new WaitForSecondsRealtime(DeltaT);
            player.position=new Vector2(player.position.x,player.position.y-DeltaS);}
        
        musica.clip=musicaVittoria;          // Musichetta
        musica.Play();
        
        while(musica.isPlaying){
            yield return null;}
        
        while(tempo>0){                          // Punti=Tempo*50
            tempo--;
            AggiungiPunti(50);
            infoPartita[2].text=tempo.ToString();
            yield return new WaitForSecondsRealtime(DeltaT);}
        _puntiPrecedenti+=_punti;                  // Punti=somma
            
        switch(_nomeLivello){
            case "Livello1":
                _nomeLivello="Livello2";
                break;
            case "Livello2":               // Prossimo livello
                _nomeLivello="Esito";
                break;}

        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadScene(_nomeLivello);}

//////////////////////////////////////////////// MORTE /////////////////////////////////////////////////////////////////
    public IEnumerator Morte(){
        var h=player.position.y+5;
        
        posOggetto=player.position;
        musica.pitch=1;
        Time.timeScale=0;                                             // Non si muove più nulla
        musica.clip=morte;           // Suono morte
        musica.loop=false;            // Solo una volta
        musica.Play();

        yield return new WaitForSecondsRealtime(0.5f);
        if(posOggetto.y>=altezzaMinimaVisibile){
            while(posOggetto.y<h){
                yield return new WaitForSecondsRealtime(DeltaT);           // Sale
                posOggetto.y=player.position.y+DeltaS;
                player.position=posOggetto;}
            
            posOggetto.z=-1;                                     // Davanti a tutto
            while(posOggetto.y>altezzaMinimaVisibile){
                yield return new WaitForSecondsRealtime(DeltaT);               // Scende
                posOggetto.y=player.position.y-DeltaS;
                player.position=posOggetto;}}
        
        while(musica.isPlaying){
            yield return null;}                       // Finisce di suonare e cambia scena
        SceneManager.LoadScene("Morte");}
    
/////////////////////////////////////////////////// PAUSA //////////////////////////////////////////////////////////////
    public void SetPausa(){
        Time.timeScale=0;
        pausaPanel.gameObject.SetActive(true);}
    
/////////////////////////////////////////////////// PIU' PUNTI /////////////////////////////////////////////////////////
    public void AggiungiPunti(int p){
        _punti+=p;
        infoPartita[0].text=_punti.ToString();}               // Aggiunge punti
    public void AggiungiMoneta(){
        monete++;
        infoPartita[1].text=monete.ToString();              // Aggiunge moneta -> nuovi punti
        AggiungiPunti(200);}
    
/////////////////////////////////////////////////////// CUBO VUOTO /////////////////////////////////////////////////////
    public IEnumerator CuboVuoto(Transform obj){
        posOggetto=obj.position;
        scalaOggetto=obj.localScale;
        obj.position=new Vector2(obj.position.x,obj.position.y+DeltaCuboVuoto);      // Su e più grande per 0.5sec
        obj.localScale+=DeltaS*obj.localScale;
        yield return new WaitForSeconds(DeltaCuboVuoto);
        obj.position=posOggetto;
        obj.localScale=scalaOggetto;}
    
//////////////////////////////////////////////// OGGETTI NASCOSTI //////////////////////////////////////////////////////
    public bool RilasciaOggetto(string nomeCubo){
        cuboOggettoCollisione=cubiOggetto.FirstOrDefault(cubo=>cubo.name==nomeCubo && !cubo.GetRilasciato());
        if(!cuboOggettoCollisione){
            return false;}
        
        cuboOggettoCollisione.Rilascia();
        return cuboOggettoCollisione.PresenzaMoneta();}               // Mi dice se suonare o no
        
///////////////////////////////////////////////////////// UPDATE ///////////////////////////////////////////////////////
    private void Update(){
        if(player.position.x<posMinX || player.position.x>posMaxX){      // Non troppo indietro o avanti
            return;}
        
        posCamera.x=player.position.x;
        transform.position=posCamera;}}       // Distanza fissa su y e z