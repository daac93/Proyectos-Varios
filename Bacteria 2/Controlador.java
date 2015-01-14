import java.awt.event.*;

public class Controlador
{
    private Arbitro arbitro;
    private static final int FILAS = 15;
    private static final int COLUMNAS = 20;

    /**
     * Constructor
     */
    public Controlador()
    {
        arbitro = new Arbitro(FILAS,COLUMNAS,this);
    }

    public void iniciar() {
        arbitro.iniciarJuego();
    }
}
