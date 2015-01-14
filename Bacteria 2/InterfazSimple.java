import javax.swing.JOptionPane;

/**
 * Se encarga de mostrar los diferentes mensajes necesarios para interactuar con el usuario.
 */
public class InterfazSimple extends JOptionPane {
    private static final String TITULO_VENTANA = "Bacteria 2-Daniel Arguedas";  

    /**
     * Constructor
     */
    public InterfazSimple()
    {

    }

    /**
     * Saluda al usuario
     */
    public void saludar() {
        this.showMessageDialog(null,"",TITULO_VENTANA,INFORMATION_MESSAGE);
    }

    /**
     * Muestra un dialogo
     * @param elDialogo = Dialogo que quiere ser mostrado
     */
    public void mostrarDialogo(String elDialogo) {
        this.showMessageDialog(null,elDialogo,TITULO_VENTANA,INFORMATION_MESSAGE);
    }

    public String pedirHilera () {
        String hilera = "";
        hilera = this.showInputDialog(null,"Digite una hilera");
        return hilera;
    }

    public boolean pedirConfirmacion () {
        boolean respuesta=true;
        int numRespuesta;

        numRespuesta = this.showConfirmDialog(null, "Seguro?", "ALERTA", YES_NO_OPTION);

        if (numRespuesta == 1) {
            respuesta = false;
        }

        return respuesta;
    }

    public void decirInformacion() {
        String informacion;
        
        informacion = "Bienvenido a Bacteria 2!\n\nSobre el juego:\nEl juego es simple, al presionar una de las fichas verdes,esta se gira en sentido\nde las manecillas del reloj y contamina a todas las que esten conectadas a ella.\nPor su parte la computadora piensa su jugada y rota la ficha que mas le convenga.\nEl objetivo del juego es eliminar todas las fichas rojas deltablero,\nsi usted logra esto el juego habr� terminado y usted habr� ganado.  Sin embargo;\nsi todas las fichas verdes son eliminadas, usted habr� perdido pero\nno se desanime! En el men� Programa podr� encontrar la opci�n Reiniciar, la cu�l\nle dar� una nueva oportunidad para jugar.\n\nSuerte!";
        
        this.mostrarDialogo(informacion);
    }
}