
public class Celula {
    private int fila;
    private int columna;
    private int direccion;
    private String estado;
    
    /**
     * Constructor
     */
    public Celula(int laDireccion,int laFila, int laColumna)
    {
        this.setDireccion(laDireccion);
        this.setFila(laFila);
        this.setColumna(laColumna);
        estado = "N";       
    }

    public void setFila(int laFila) {
        fila = laFila;
    }
    
    public void setColumna(int laColumna) {
        columna = laColumna;
    }
    
    public void setDireccion(int laDireccion) {
        direccion = laDireccion;
        if(direccion == 4 || direccion == -1) {
            direccion = 0;
        }
    }
    
    public void setEstado(String elestado) {
        estado = elestado;
    }
    
    public int getFila() {
        return fila;
    }
    
    public int getColumna() {
        return columna;
    }
    
    public int getDireccion() {
        return direccion;
    }
    
    public String getEstado() {
        return estado;
    }
}
