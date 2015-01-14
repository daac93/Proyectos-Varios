import java.util.ArrayList;

public class JugadorArtificial {
    private Tejido miTejido;
    private Conector conectorArtificial;
    private ArrayList <Celula> elTejidoRojo;
    private Celula [][] lasCelulas;
    
    /**
     * Constructor
     */
    public JugadorArtificial(Tejido elTejido,Conector elConector)  {
        miTejido = elTejido;
        conectorArtificial = elConector;
        elTejidoRojo = elTejido.getRojas();
        lasCelulas = miTejido.getTejido();
    }
    
    /**
     * Busca la mejor Jugada para la computadora.
     * @return Celula mejorCelula: la celula que determina la mejor jugada.
     */
    public Celula buscarJugada() {
        Celula mejorCelula = null;
        Celula celulaActual = null;
        Celula siguienteCelula;
        ArrayList <Celula> laJugada = new ArrayList<Celula>();
        ArrayList <Celula> posibleJugada = null;
        int posicionLista = 0;
        int filaActual = 0;
        int direccion = 0;
        int columnaActual = 0;
        int cantidadRojos = 0;       

        elTejidoRojo = miTejido.getRojas();
        laJugada.clear();
        cantidadRojos = elTejidoRojo.size();    
        
        while (posicionLista < cantidadRojos) {
            celulaActual = elTejidoRojo.get(posicionLista);
            filaActual = celulaActual.getFila();
            columnaActual = celulaActual.getColumna();
            siguienteCelula = conectorArtificial.buscarSiguiente(celulaActual,1);
            
            if((siguienteCelula != null) && (siguienteCelula.getEstado() != "R")) {
                posibleJugada = conectorArtificial.buscarConectadas(siguienteCelula,"R");
                
                if(laJugada.size() < posibleJugada.size()) {
                    mejorCelula = celulaActual;
                    laJugada = posibleJugada;
                }
            }
            
            posicionLista++;
        }
       
        //Ver dos turnos al futuro
        posicionLista = 0;
        while ((posicionLista < cantidadRojos) && (mejorCelula == null)) {
            celulaActual = elTejidoRojo.get(posicionLista);
            filaActual = celulaActual.getFila();
            columnaActual = celulaActual.getColumna();
                
            siguienteCelula = conectorArtificial.buscarSiguiente(celulaActual,2);
            if((siguienteCelula != null) && (siguienteCelula.getEstado() != "R")) {
                mejorCelula = celulaActual;
            }
            posicionLista++;
        }
       
        //Ver tres turnos al futuro
        posicionLista = 0;
        while ((posicionLista < cantidadRojos) && (mejorCelula == null)) {
            celulaActual = elTejidoRojo.get(posicionLista);
            filaActual = celulaActual.getFila();
            columnaActual = celulaActual.getColumna();
                
            siguienteCelula = conectorArtificial.buscarSiguiente(celulaActual,3);
            if((siguienteCelula != null) && (siguienteCelula.getEstado() != "R")) {
                mejorCelula = celulaActual;
            }
            posicionLista++;
        }
        
        return mejorCelula;
    }
    
    /**
     * 
     */
    public ArrayList<Celula> getRojas() {
        return elTejidoRojo;
    }
}
