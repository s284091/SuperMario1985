using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class GestorePartita:MonoBehaviour{
    private int monete,tempo=150;
    private static int _vite=Vite,_punti,_puntiPrecedenti;
    private AudioSource musica;
    private float posMinX,posMaxX;
    private Vector3 posCamera,posObj;
    private const float DeltaT=0.0005f,DeltaS=0.2f,DeltaCuboVuoto=0.1f,DeltaAscia=1.5f;
    private const int TempoVelocitàDoppia=50,Vite=10,AltezzaMinimaVisibile=3,DeltaPonte=2;
    private static string _nomeLivello="Livello1";
    [SerializeField] private float hMax;
    [SerializeField] private int terra,hSogliaTop;
    [SerializeField] private SpriteRenderer player;
    [SerializeField] private Transform posBandiera,pausaPanel;
    [SerializeField] private Bowser bowser;
    [SerializeField] private Transform[] cubiDaMettere,ponte;
    [SerializeField] private TMP_Text[] infoPartita;
    [SerializeField] private AudioClip morte,pochiSecondi,musicaStd,musicaVittoria,cadeBowser;
    
////////////////////////////////////////////////// AWAKE ///////////////////////////////////////////////////////////////
    private void Awake(){
        var cam=GetComponent<Camera>();                   // Componenti
        
        Time.timeScale=1;                                      // Azzerato alla morte
        musica=GetComponents<AudioSource>()[1];
        infoPartita[0].text=_punti.ToString();                // Punti
        infoPartita[3].text=_vite.ToString();                // Vite (10 o rimanenti)
        
        posMinX=player.gameObject.transform.position.x+cam.orthographicSize*cam.aspect-0.5f;
        posMaxX=posBandiera.position.x-cam.orthographicSize*cam.aspect+1.5f;  // 0.5/1.5 per inquadrare tutto l'oggetto
        posCamera=new Vector3(posMinX,9.2f,-9);
        
        StartCoroutine(TimerPartita());                 // Avvio timer
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
        return _nomeLivello;}
    
///////////////////////////////////////////////////// TIMER ////////////////////////////////////////////////////////////
    private IEnumerator TimerPartita(){
        while(tempo>0){
            yield return new WaitForSeconds(1);            // Passa 1 sec
            tempo--;
            if(tempo==TempoVelocitàDoppia){
                StartCoroutine(PochiSecRimanenti());}                 // Cambia il suono
            infoPartita[2].text=tempo.ToString();}
        
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
        var h=player.gameObject.transform.position.y<hMax? player.gameObject.transform.position.y : hMax;
        
        player.enabled=true;                       // Stop
        Time.timeScale=0;
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
        
        while(posBandiera.position.y<h){
            yield return new WaitForSecondsRealtime(DeltaT);
            posObj=player.gameObject.transform.position;
            posObj.y-=DeltaS;
            player.gameObject.transform.position=posObj;              // La bandiera sale, mario scende
            posObj=posBandiera.position;
            posObj.y+=DeltaS;
            posBandiera.position=posObj;}
        
        while(player.gameObject.transform.position.y>terra+player.gameObject.transform.localScale.y/2-DeltaS){
            yield return new WaitForSecondsRealtime(DeltaT);
            posObj=player.gameObject.transform.position;
            posObj.y-=DeltaS;                                                         // Mario scende fino a terra
            player.gameObject.transform.position=posObj;}
        
        musica.clip=musicaVittoria;          // Musichetta
        musica.Play();
        while(musica.isPlaying){
            yield return null;}
        
        while(tempo>0){                          // Punti=Tempo*50
            tempo--;
            AggiungiPunti(50);
            infoPartita[2].text=tempo.ToString();
            yield return new WaitForSecondsRealtime(DeltaT);}
        _puntiPrecedenti=_punti;                  // Punti
         
        _nomeLivello=_nomeLivello=="Livello1"? "Livello2" : "Livello3";            // Prossimo elemento
        yield return new WaitForSecondsRealtime(1);
        SceneManager.LoadScene(_nomeLivello);}
    
    public IEnumerator VittoriaFinale(){                    // Vittoria ultimo livello
        var posRotazione=new Vector2(posBandiera.position.x,posBandiera.position.y-DeltaAscia);
        var n=0;
        
        player.enabled=true;                       // Stop
        Time.timeScale=0;
        musica.pitch=1;                         // Blocca tutto
        musica.Stop();
        musica.loop=false;
        
        while(player.gameObject.transform.position.y>terra+player.gameObject.transform.localScale.y/2-DeltaS){
            yield return new WaitForSecondsRealtime(DeltaT);
            yield return new WaitForSecondsRealtime(DeltaT);
            posObj=player.gameObject.transform.position;
            posObj.y-=DeltaS;
            player.gameObject.transform.position=posObj;}              // Mario scende fino a terra
        
        while(bowser.transform.position.y>terra+1){                   // Bowser scende fino a terra
            yield return new WaitForSecondsRealtime(DeltaT);
            posObj=bowser.transform.position;
            posObj.y-=DeltaS;
            bowser.transform.position=posObj;}
        
        posBandiera.RotateAround(posRotazione,Vector3.forward,90);       // Cade l'ascia
        AggiungiPunti(10000);                // Punti vittoria
        
        while(n<ponte.Length){
            n++;
            yield return new WaitForSecondsRealtime(DeltaS);           // Crolla il ponte
            for(var i=0;i<n;i++){
                posObj=ponte[i].position;
                posObj.y-=DeltaPonte;
                ponte[i].position=posObj;}}
        
        musica.clip=cadeBowser;                                    // Musichetta morte Bowser
        musica.Play();
        
        while(bowser.transform.position.y>0){
            yield return new WaitForSecondsRealtime(DeltaT);       // Bowser cade
            posObj=bowser.transform.position;
            posObj.y-=DeltaS;
            bowser.transform.position=posObj;}
        
        musica.clip=musicaVittoria;          // Musichetta
        musica.Play();
        while(musica.isPlaying){
            yield return null;}
        
        while(tempo>0){                        // Punti tempo
            tempo--;
            AggiungiPunti(50);
            infoPartita[2].text=tempo.ToString();
            yield return new WaitForSecondsRealtime(DeltaT);}
        
        SceneManager.LoadScene("Esito");}                 // Scena finale

//////////////////////////////////////////////// MORTE /////////////////////////////////////////////////////////////////
    public IEnumerator Morte(){
        var posOggetto=player.gameObject.transform.position;
        var h=posOggetto.y+5;
        
        player.enabled=true;                       // Stop
        musica.pitch=1;
        Time.timeScale=0;                                             // Non si muove più nulla
        musica.clip=morte;           // Suono morte
        musica.loop=false;            // Solo una volta
        musica.Play();

        yield return new WaitForSecondsRealtime(0.5f);
        if(posOggetto.y>=AltezzaMinimaVisibile){
            while(posOggetto.y<h){
                yield return new WaitForSecondsRealtime(DeltaT);           // Sale
                posOggetto.y=player.gameObject.transform.position.y+DeltaS;
                player.gameObject.transform.position=posOggetto;}
            
            posOggetto.z=-1;                                     // Davanti a tutto
            while(posOggetto.y>AltezzaMinimaVisibile){
                yield return new WaitForSecondsRealtime(DeltaT);               // Scende
                posOggetto.y=player.gameObject.transform.position.y-DeltaS;
                player.gameObject.transform.position=posOggetto;}}
        
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
        var posOggetto=obj.position;
        var scalaOggetto=obj.localScale;
        
        obj.position=new Vector2(obj.position.x,obj.position.y+DeltaCuboVuoto);      // Su e più grande per 0.5sec
        obj.localScale+=DeltaS*obj.localScale;
        yield return new WaitForSeconds(DeltaCuboVuoto);
        obj.position=posOggetto;
        obj.localScale=scalaOggetto;}
    
///////////////////////////////////////////////////// INIZIO SCONTRO ///////////////////////////////////////////////////
    public void InizioScontro(){
        foreach(var cubo in cubiDaMettere){                 // Chiude l'ingresso
            cubo.gameObject.SetActive(true);}
        
        bowser.SparaFuoco();}                // Avvia il fuoco all'inizio

///////////////////////////////////////////////////////// UPDATE ///////////////////////////////////////////////////////
    private void Update(){
        if(player.gameObject.transform.position.x<posMinX || player.gameObject.transform.position.x>posMaxX){
            return;}                           // Non troppo indietro o avanti
            
        posCamera.x=player.gameObject.transform.position.x;
        transform.position=posCamera;}}       // Distanza fissa su y e z