import javax.swing.*;
import java.awt.*;

/**
 * Se encarga de mostrar los diferentes mensajes necesarios para interactuar con el usuario.
 */
public class Interfaz extends JFrame {
    private static final String TITULO_VENTANA = "Bacteria 2-Daniel Arguedas";  
    private static final String ACERCA_DE = "Bacteria 2.\nTarea programada #3.\nRealizado por Daniel Arguedas.\n2011.";
    private static final String CREDITOS = "Bacteria 2.\nVersión Original: Simon Donkers.\nEsta versión fue realizada por Daniel Arguedas.\n2011.";
    private static final String FONDO = "BACTERIAs/BACK.jpg";
    private static final int ANCHO = 1000;
    private static final int ALTO = 800;
    private ImageIcon imagenFondo;
    private InterfazSimple interfazAuxiliar;
    private Menu menu;
    private Panel panelEstado;
    private Panel panelTablero;
    private JLabel puntosRojas;
    private JLabel puntosVerdes;
    private int filas;
    private int columnas;
    
    private Arbitro miArbitro;
    private Tablero elTablero;
    
    
    /**
     * Constructor
     */
    public Interfaz(Tablero tableroEnJuego,Arbitro elArbitro) {
       elTablero = tableroEnJuego;
       miArbitro = elArbitro;
       this.setLayout(new FlowLayout());
       interfazAuxiliar = new InterfazSimple();
       filas = elTablero.getFilas();
       columnas = elTablero.getColumnas();
       this.configurar();
       this.agregarComponentes();
    }

    public void configurar() {
        this.setTitle(TITULO_VENTANA);
        this.setSize(ANCHO, ALTO);
        this.setLocationRelativeTo(null);
        this.setLayout(new BorderLayout());
        this.setDefaultCloseOperation(EXIT_ON_CLOSE);
        this.setIconImage (new ImageIcon("VIRUS.png").getImage());
    }
    
    public void agregarComponentes() {
        this.agregarMenu(miArbitro);
        this.agregarPanelEstado();
        this.agregarPanelTablero(elTablero);
    }
    
    public void activar() {
        this.setVisible(true);
    }
    
    public void agregarMenu(Arbitro elArbitro) {
        menu = new Menu(elArbitro);
        this.setJMenuBar(menu);
    }
    
    public void agregarPanelTablero(Tablero elTablero) {
        Boton[][] botones = elTablero.getTablero();
        Boton elBoton;
        
        panelTablero = new Panel(new GridLayout(filas,columnas),FONDO);
        
        for(int laFila = 0; laFila < botones.length; laFila++) {
            for(int laColumna = 0; laColumna < botones[0].length; laColumna++) {
                elBoton = botones[laFila][laColumna];
                panelTablero.add(elBoton);
            }
        }
        
        this.add(panelTablero,BorderLayout.CENTER);
    }
    
    public void agregarPanelEstado() {
        panelEstado = new Panel(new FlowLayout(),null);
        puntosRojas = new JLabel("<html><font color = red>Rojas: 0</font></html>");
        puntosVerdes = new JLabel("<html><font color = green>Verdes: 0</font></html>");
        
        panelEstado.add(puntosVerdes);
        panelEstado.add(puntosRojas);
        
        this.add(panelEstado,BorderLayout.NORTH);
    }
    
    public void refrescarPanelEstado() {
        puntosVerdes.setText("<html><font color = green>VERDES: " + miArbitro.getPuntajeVerde() + "</font></html>");
        puntosRojas.setText("<html><font color = red>ROJAS: " + miArbitro.getPuntajeRojo() + "</font></html>");
    }
    
    public void decirAcercaDe() {
        interfazAuxiliar.mostrarDialogo(ACERCA_DE);
    }
    
    public void decirCreditos() {
        interfazAuxiliar.mostrarDialogo(CREDITOS);
    }
    
    public void decirGanador(String elGanador) {
        String frase = null;
        
        switch(elGanador) {
            case "C":
            frase = "Lo siento! Ha perdido! D=\nPulsa Programa>Reiniciar si quieres jugar de nuevo!";
            break;
            case "J":
            frase = "Felicidades!!! Ha ganado! =D \nPulsa Programa>Reiniciar si quieres jugar de nuevo!";
            break;
        }
        
        interfazAuxiliar.mostrarDialogo(frase);
    }
    
    public void decirInstrucciones() {
        interfazAuxiliar.decirInformacion();
    }
}