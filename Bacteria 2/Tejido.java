import java.util.Random;
import java.util.ArrayList;

public class Tejido
{
    private Celula [][] tejido;
    private ArrayList <Celula> verdes;
    private ArrayList <Celula> rojas;
    private final int FILAS;
    private final int COLUMNAS;
    private final int ELEMENTOS_TOTALES;
    
    /**
     * Constructor
     */
    public Tejido(int cantidadFilas, int cantidadColumnas) {
        FILAS = cantidadFilas;
        COLUMNAS = cantidadColumnas;
        ELEMENTOS_TOTALES = (FILAS*COLUMNAS);
        verdes = new ArrayList<Celula>(ELEMENTOS_TOTALES);
        rojas = new ArrayList<Celula>(ELEMENTOS_TOTALES);
        this.construir();
        this.organizarCelulas();
    }
    
    public int getFilas() {
        return FILAS;
    }
    
    public int getColumnas() {
        return COLUMNAS;
    }
    
    public int getTotales() {
        return ELEMENTOS_TOTALES;
    }
    
    public Celula [][] getTejido() {
        return tejido;
    }
    
    public ArrayList<Celula> getVerdes() {
        return verdes;
    }
    
    public ArrayList<Celula> getRojas() {
        return rojas;
    }
    
    public void setTejido(Celula laCelula) {
        tejido[laCelula.getFila()][laCelula.getColumna()] = laCelula;
    }
    
    public void decirDatos() {
        int posFila = 0;
        int posColumna = 0;
        int numeroElementos = ELEMENTOS_TOTALES;
        int laDireccion = 0;
        String estado = "";
        String datos = "<html>";
        
        while (numeroElementos != 0) {
            
            while(posFila != (FILAS)) {
                
                while(posColumna != (COLUMNAS)) {
                    Celula laCelula;
                    laCelula = tejido[posFila][posColumna];
                    estado = laCelula.getEstado();
                    switch (estado) {
                        case "V": datos += "<font color=green>";
                        break;
                        case "R": datos += "<font color=red>";
                        break;                        
                    }
                    laDireccion= laCelula.getDireccion();
                    switch (laDireccion) {
                        case 0: datos += "\u2192_";
                        break;
                        case 1: datos += "\u2193_";
                        break;
                        case 2: datos += "\u2190_";
                        break;
                        case 3: datos += "\u2191_";
                        break;
                    }
                    if (estado != "N") {
                        datos += "</font>";
                    }
                    posColumna++;
                    numeroElementos--;
                }
                
                datos +="<br>";
                posColumna = 0;
                posFila++;
            }

        }
        
        datos += "</html>";
    }
    
    public void construir() {
        Random generador = new Random();
        int cantidadElementos = ELEMENTOS_TOTALES;
        int posFila = 0;
        int posColumna = 0;
        int direccionCelula = 0;
        Celula miCelula;
        
        tejido = new Celula [FILAS] [COLUMNAS];
        while (cantidadElementos != 0) {
            
            while(posFila != (FILAS)) {
                
                while(posColumna != (COLUMNAS)) {
                    direccionCelula = generador.nextInt(4);
                    tejido[posFila][posColumna] = new Celula(direccionCelula,posFila, posColumna);
                    posColumna++;
                    cantidadElementos--;
                }
                posColumna = 0;
                posFila++;
                
            }

        }
        
        miCelula = tejido[0][0];
        miCelula.setEstado("V");
        tejido [0][0] = miCelula;
        
        miCelula = tejido[(FILAS - 1)][(COLUMNAS - 1)];
        miCelula.setEstado("R");
        tejido [(FILAS - 1)][(COLUMNAS - 1)] = miCelula;
        
    }

    public void organizarCelulas() {
        String elEstado;
        Celula celulaActual;
        
        verdes.clear();
        rojas.clear();
        for(int laFila = 0; laFila<FILAS;laFila++) {
            for(int laColumna = 0; laColumna<COLUMNAS; laColumna++) {
                celulaActual = tejido [laFila][laColumna];
                elEstado = celulaActual.getEstado(); 

                switch(elEstado) {
                    case "V" : verdes.add(celulaActual);
                    break;
                    case "R" : rojas.add(celulaActual);
                    break;
                }

            }
        
        }
    }
    
        public ArrayList<Celula> obtenerVecinos(Celula celulaSeleccionada) {
        ArrayList <Celula> vecinos = new ArrayList<Celula>();;
        Celula celulaVecina;
        int laFila = celulaSeleccionada.getFila();
        int laColumna = celulaSeleccionada.getColumna();
        
        if((laColumna + 1) < COLUMNAS) {
            celulaVecina = tejido[laFila][laColumna + 1];
            vecinos.add(celulaVecina);
        }
        if((laColumna -1) >= 0) {
            celulaVecina = tejido[laFila][laColumna -1 ];
            vecinos.add(celulaVecina);
        }
        if((laFila + 1) < FILAS) {
            celulaVecina = tejido[laFila +1 ][laColumna];
            vecinos.add(celulaVecina);
        }
        if((laFila - 1) >= 0) {
            celulaVecina = tejido[laFila - 1 ][laColumna];
            vecinos.add(celulaVecina);
        }
        
        return vecinos;
    }
    
    public boolean compararCelulas(Celula unaCelula, Celula otraCelula) {
        boolean sonIguales = false;
        int filaUna = unaCelula.getFila();
        int filaOtra = otraCelula.getFila();
        int columnaUna = unaCelula.getColumna();
        int columnaOtra = otraCelula.getColumna();
        
        if ((filaUna == filaOtra) && (columnaUna == columnaOtra)) {
            sonIguales = true;
        }
        
        return sonIguales;
    }
    
    public void reiniciarTejido() {
        verdes.clear();
        rojas.clear();
        this.construir();
        this.organizarCelulas();
    }
}