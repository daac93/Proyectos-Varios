import java.util.ArrayList;
import java.awt.event.*;

/**
 * Clase encargada de llevar el control del juego
 */
public class Arbitro implements ActionListener  {
    private static final String SEPARADOR = "/";
    private Tejido miTejido;
    private Interfaz interfaz;
    private Conector conector;
    private Tablero miTablero;
    private Controlador elControlador;
    private JugadorArtificial computadora;
    private Celula laCelula;
    private boolean ganador = false;
    private boolean repetir = true;
    private String ganadorTexto = "";
    
    /**
     * Constructor
     */
    public Arbitro(int lasFilas,int lasColumnas, Controlador nuestroControlador)
    {
        elControlador = nuestroControlador;
        miTejido = new Tejido(lasFilas,lasColumnas);
        conector = new Conector(miTejido);
        miTablero = new Tablero(miTejido,this,SEPARADOR);
        computadora = new JugadorArtificial(miTejido,conector);
        interfaz = new Interfaz(miTablero,this);       
    }

    /**
     * 
     */
    public void actionPerformed(ActionEvent evento) {
        String mensajeAccion = evento.getActionCommand();
        int posicionSeparador = mensajeAccion.indexOf(SEPARADOR);
        if(posicionSeparador != -1) {
            int filaAccion = Integer.parseInt(mensajeAccion.substring(0, posicionSeparador));
            int columnaAccion = Integer.parseInt(mensajeAccion.substring(posicionSeparador + 1, mensajeAccion.length()));
        
            laCelula = (miTejido.getTejido())[filaAccion][columnaAccion];
            while(laCelula.getEstado() == "R") {
                laCelula = null;
            }
        }else if(evento.getActionCommand().equals("Reiniciar")) {
            ganador = false;
            miTejido.reiniciarTejido();
            this.inicializarJuego();
            miTablero.actualizarTablero();
        }else if (mensajeAccion.equals("Acerca de...")) {
            interfaz.decirAcercaDe();
        }else if (mensajeAccion.equals("Créditos")) {
            interfaz.decirCreditos();
        }else if (mensajeAccion.equals("Salir"))  {
            repetir = false;
            System.exit(0);
        }else if(mensajeAccion.equals("Instrucciones")){
            interfaz.decirInstrucciones();
        }
    }
    
    /**
     * Rota la celula especificada
     * @param celulaARotar: Celula que se va a rotar
     */
    public void rotar(Celula celulaARotar) {
        int direccion;
              
        direccion = celulaARotar.getDireccion();
        celulaARotar.setDireccion(direccion + 1);
        direccion = celulaARotar.getDireccion();
        if (direccion == 4) {
            celulaARotar.setDireccion(0);
        }
        miTejido.setTejido(celulaARotar);
    }
    
    /**
     * Revisa si alguno de los 2 jugadores ha ganado
     * @return ganador: booleano que indica si alguien ha ganado
     */
    public boolean revisarGanador() {
        boolean ganador = false;
        boolean lleno = false;
        ArrayList<Celula> lasVerdes = miTejido.getVerdes();
        ArrayList<Celula> lasRojas = miTejido.getRojas();
        int cantidad = 0;
        
        cantidad = lasVerdes.size();
        if (cantidad == 0) {
            ganador = true;
            ganadorTexto = "C";
        }else {
            cantidad = lasRojas.size();
            if (cantidad == 0) {
                ganador = true;
                ganadorTexto = "J";
            }
        }
        
        return ganador;
    }
    
    /**
     * Contamina a las Celulas conectadas a la Celula de "x" color.
     * @param celulasAContaminar: Celulas conectadas que se van a contaminar.
     * @param elColor: Especifica de que color/estado se van a contaminar las Celulas.
     */
    public void contaminar(ArrayList<Celula> celulasAContaminar, String elColor) {
        int cantidad = 0;
        int posLista = 0;
        int filaCelula = 0;
        int columnaCelula = 0;
        Celula celulaAContaminar;
        
        cantidad = celulasAContaminar.size();
        while(posLista < cantidad) {
            celulaAContaminar = celulasAContaminar.get(posLista);
            celulaAContaminar.setEstado(elColor);
            miTejido.setTejido(celulaAContaminar);
            posLista++;
        }
    }
    
    /**
     * Inicializa el juego. Contamina las celulas conectadas a la celula inicial de cada color
     */
    public void inicializarJuego() {
        Celula miCelula;
        ArrayList<Celula> celulas = new ArrayList<Celula>();
        
        celulas = miTejido.getVerdes();
        if(celulas.size() > 0) {
            miCelula = celulas.get(0);
            this.contaminar(conector.buscarConectadas(miCelula,"V"),"V");
        }
        
        celulas.clear();
        miTejido.organizarCelulas();
        
        celulas = miTejido.getRojas();
        if(celulas.size() > 0) {    
            miCelula = celulas.get(0);
            this.contaminar(conector.buscarConectadas(miCelula,"R"),"R");
        }
    }
    
    /**
     * Comienza y lleva el control del juego hasta que alguien gane.
     */
    public void iniciarJuego() {
        int numeroTurno = 0;
        Celula celulaAMover = null;
        ArrayList<Celula> celulasAContaminar = new ArrayList<Celula>(miTejido.getTotales());
        
        this.inicializarJuego();
        interfaz.activar();
        this.esperar(1);
        miTablero.actualizarTablero();
        while (repetir == true) {
            numeroTurno = 0;
            this.esperar(1);
            interfaz.refrescarPanelEstado();
            while(ganador == false) {
                celulaAMover = null;
                laCelula = null;
                miTejido.organizarCelulas();
                ganador = this.revisarGanador();
                interfaz.refrescarPanelEstado();
                if(((numeroTurno%2) == 0) && ganador == false) {
                    while(celulaAMover == null) {
                        if(laCelula == null) {
                            this.esperar(1);
                        }
                        celulaAMover = laCelula;
                    }
                    this.rotar(laCelula);
                    celulasAContaminar = conector.buscarConectadas(celulaAMover,"V");
                    this.contaminar(celulasAContaminar,"V");
                } else if(ganador == false){
                    this.esperar(2);
                    celulaAMover = computadora.buscarJugada();
                    if(celulaAMover != null) {
                        this.rotar(celulaAMover);
                        celulasAContaminar = conector.buscarConectadas(celulaAMover,"R");
                        this.contaminar(celulasAContaminar,"R");
                    }
                }
                miTejido.organizarCelulas();
                miTablero.actualizarTablero();
                interfaz.refrescarPanelEstado();
                ganador = this.revisarGanador();
                numeroTurno++;
                if(ganador == true) {
                    this.esperar(1);
                    interfaz.decirGanador(ganadorTexto);
                }
            }
        }
    }
    
    private void esperar(int segundos){
        try{
            Thread.currentThread().sleep(segundos * 1000);
        }catch(Exception e){
        }
    }
    
    public int getPuntajeRojo() {
        int puntosRojos;
        
        puntosRojos = (miTejido.getRojas()).size();
        
        return puntosRojos;
    }
    
    public int getPuntajeVerde() {
        int puntosVerdes;
        
        puntosVerdes = (miTejido.getVerdes()).size();
        
        return puntosVerdes;
    }
}
