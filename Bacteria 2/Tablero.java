import java.util.ArrayList;

public class Tablero  {
    private Tejido elTejido;
    private Arbitro miArbitro;
    private Boton[][] losBotones;
    private int numeroColumnas;
    private int numeroFilas;
    private final String SEPARADOR;
    
    /**
     * Constructor
     */
    public Tablero(Tejido nuestroTejido,Arbitro elArbitro,String elSeparador)
    {
        elTejido = nuestroTejido;
        miArbitro = elArbitro;
        numeroColumnas = (elTejido.getTejido()[0]).length;
        numeroFilas = (elTejido.getTejido()).length;
        SEPARADOR = elSeparador;
        this.construirTablero();
        this.actualizarTablero();
    }
    
    /**
     * Construye la matriz de objetos de tipo boton segun el tamaño especificado
     */
    public void construirTablero() {
        losBotones = new Boton[numeroFilas][numeroColumnas];
        
        for(int laFila = 0;laFila < numeroFilas; laFila++) {
            for(int laColumna = 0; laColumna < numeroColumnas; laColumna++) {
                losBotones[laFila][laColumna] = new Boton();
                losBotones[laFila][laColumna].setActionCommand(Integer.toString(laFila) + SEPARADOR + Integer.toString(laColumna));
            }
        }
    }
    
    public void actualizarTablero() {
        Celula[][] celulas = elTejido.getTejido();
        for(int laFila = 0;laFila < numeroFilas; laFila++) {
            for(int laColumna = 0; laColumna < numeroColumnas; laColumna++) {
                Celula laCelula = celulas[laFila][laColumna];
                int laDireccion = laCelula.getDireccion();
                String elEstado = laCelula.getEstado();
                
                Boton elBoton = losBotones[laFila][laColumna];
                elBoton.setEstadoImagen(laDireccion, elEstado);
                if((elEstado == "V")) {
                    elBoton.addActionListener(miArbitro);
                    elBoton.setEnabled (true);
                    elBoton.setOpaque(false);
                }else if(elEstado == "N") {
                    elBoton.setEnabled(false);
                    elBoton.setOpaque(false);
                }else if(elEstado == "R") {
                    elBoton.setEnabled(true);
                    elBoton.removeActionListener(miArbitro);
                    elBoton.setOpaque(false);
                }
                losBotones[laFila][laColumna] = elBoton;
            }
        }
    }
    
    public Boton[][] getTablero() {
        return losBotones;
    }
    
    public int getFilas() {
        return numeroFilas;
    }
    
    public int getColumnas() {
        return numeroColumnas;
    }
}
