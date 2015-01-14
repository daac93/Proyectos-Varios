import java.util.ArrayList;

public class Conector
{
    private Tejido elTejido;
    
    /**
     * Constructor
     */
    public Conector(Tejido tejidoEnJuego)
    {
        elTejido = tejidoEnJuego;
    }

    /**
     * Busca todas las celulas conectadas con la celula admitida como parametro. Ademas revisa que no sean del mismo color que el especificado.
     * @param Celul laCelula: Celula a la que se le debe buscar las conectadas.
     * @param String elEstado: Estado que no se quiere tomar en cuenta como conectadas(mismo estado que laCelula).
     * @return ArrayList celulasConectadas: Lista de todas las celulas conectadas entre si.
     */
    public ArrayList<Celula> buscarConectadas(Celula laCelula,String elEstado)  {
        ArrayList<Celula> celulasConectadas = new ArrayList <Celula>();
        ArrayList<Celula> celulasVecinas = new ArrayList <Celula>(4);
        Celula celulas [] [];
        Celula celulaActual;
        int posicionLista = 0;
        boolean laContiene;
        boolean sonLaMisma;
        
        celulas = elTejido.getTejido();        
        celulasConectadas.add(laCelula);
        
        while(posicionLista < (celulasConectadas.size())) {
            celulaActual = celulasConectadas.get(posicionLista);
            celulasVecinas = elTejido.obtenerVecinos(celulaActual);
            for(Celula vecina : celulasVecinas) {
                Celula celulaQueVe;
                
                if((vecina != null) && (celulasConectadas.contains(vecina)) == false)  {
                    celulaQueVe = this.buscarApuntada(vecina);
                    if(celulaQueVe != null) {
                        laContiene = celulasConectadas.contains(celulaQueVe);
                        if(laContiene == true) {
                            if(vecina.getEstado() != elEstado) {
                                celulasConectadas.add(vecina);
                            }
                        } else {
                            celulaQueVe = this.buscarApuntada(celulaActual);
                            if(celulaQueVe != null) {
                                sonLaMisma = elTejido.compararCelulas(celulaQueVe,vecina);
                                if((sonLaMisma == true) && (vecina.getEstado() != elEstado)) {                       
                                    celulasConectadas.add(vecina);
                                }
                            } 
                        }
                    }else {
                        celulaQueVe = this.buscarApuntada(celulaActual);
                        if(celulaQueVe != null) {
                            sonLaMisma = elTejido.compararCelulas(celulaQueVe,vecina);
                            if((sonLaMisma == true) && (vecina.getEstado() != elEstado)) {                       
                                celulasConectadas.add(vecina);
                            }
                        } 
                    }
                }
                }
            posicionLista++;
            }
               
        return celulasConectadas;
    }
    
    /**
     * Determina a que celula esta apuntado la celula admitida como parametro.
     * @param laCelula: Celula a revisar.
     * @return celulaApuntada: Celula a la que apunta laCelula.
     */
    public Celula buscarApuntada(Celula laCelula) {
        int direccionActual = (laCelula.getDireccion());
        int filaActual = laCelula.getFila();
        int columnaActual = laCelula.getColumna();
        Celula lasCelulas[][];
        Celula celulaApuntada = null;
        
        lasCelulas = elTejido.getTejido();        
        
        switch (direccionActual) {
            case 0: columnaActual = (columnaActual + 1);
            break;
            case 1: filaActual = (filaActual + 1);
            break;
            case 2: columnaActual = (columnaActual - 1);
            break;
            case 3: filaActual = (filaActual - 1);
            break;
        }
            
        if((filaActual < elTejido.getFilas()) && (columnaActual < elTejido.getColumnas()) && (filaActual >= 0) && (columnaActual >= 0)){
            celulaApuntada = lasCelulas[filaActual][columnaActual];
        }
              
        return celulaApuntada;
    }
    
    /**
     * Busca la celula que podria ser contaminada,segun la cantidad de turnos/movimientos especificada, si se mueve la Celula que se admite como parametro
     * @param  laCelula: la Celula a revisar
     * @param  cantidadDeTurnos: cantidad de turnos a revisar
     * @return celulaSiguiente: Celula siguiente a laCelula
     */
    public Celula buscarSiguiente(Celula laCelula,int cantidadDeTurnos) {
        int direccionActual; 
        int direccionOriginal = laCelula.getDireccion();
        int filaActual = laCelula.getFila();
        int columnaActual = laCelula.getColumna();
        Celula lasCelulas[][];
        Celula celulaSiguiente = null;
        
        direccionActual = direccionOriginal + cantidadDeTurnos;
        lasCelulas = elTejido.getTejido();        

        switch(cantidadDeTurnos) {
            case 1:
            switch(direccionActual) {
                case 4: direccionActual = 0;
            }
            break;
            case 2:
            switch(direccionActual) {
                case 4: direccionActual = 0;
                break;
                case 5: direccionActual = 1;
                break;
            }
            break;
            case 3:
            switch(direccionActual) {
                case 4: direccionActual = 0;
                break;
                case 5: direccionActual = 1;
                break;
                case 6: direccionActual = 2;
                break;
            }
            break;
        }
        
        switch (direccionActual) {
            case 0: columnaActual = (columnaActual + 1);
            break;
            case 1: filaActual = (filaActual + 1);
            break;
            case 2: columnaActual = (columnaActual - 1);
            break;
            case 3: filaActual = (filaActual - 1);
            break;
        }
        
        if((filaActual < elTejido.getFilas()) && (columnaActual < elTejido.getColumnas()) && (filaActual >= 0) && (columnaActual >= 0)){
            celulaSiguiente = lasCelulas[filaActual][columnaActual];
        }
              
        return celulaSiguiente;
    }
}
