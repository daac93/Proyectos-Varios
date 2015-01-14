import javax.swing.JButton;
import javax.swing.ImageIcon;

public class Boton extends JButton
{
    private ImageIcon[] estado;

    /**
     * Constructor
     */
    public Boton()
    {
        estado = new ImageIcon[12];
        estado[0] = new ImageIcon("BACTERIAs/GD.png");
        estado[1] = new ImageIcon("BACTERIAs/GAB.png");
        estado[2] = new ImageIcon("BACTERIAs/GI.png");
        estado[3] = new ImageIcon("BACTERIAs/GA.png");
        estado[4] = new ImageIcon("BACTERIAs/VD.png");
        estado[5] = new ImageIcon("BACTERIAs/VAB.png");
        estado[6] = new ImageIcon("BACTERIAs/VI.png");
        estado[7] = new ImageIcon("BACTERIAs/VA.png");
        estado[8] = new ImageIcon("BACTERIAs/RD.png");
        estado[9] = new ImageIcon("BACTERIAs/RAB.png");
        estado[10] = new ImageIcon("BACTERIAs/RI.png");
        estado[11] = new ImageIcon("BACTERIAs/RA.png");
        this.setOpaque(false);
        this.setBorder(null);
        this.setBorderPainted(false);
        this.setContentAreaFilled(false);
    }
    
    public void setEstadoImagen(int laDireccion, String elEstado) {
        switch(elEstado) {
            case "N":
                switch(laDireccion) {
                    case 0: this.setIcon(estado[0]);
                    break;
                    case 1: this.setIcon(estado[1]);
                    break;
                    case 2: this.setIcon(estado[2]);
                    break;
                    case 3: this.setIcon(estado[3]);
                    break;
                }
            break;
            case "V":
                switch(laDireccion) {
                    case 0: this.setIcon(estado[4]);
                    break;
                    case 1: this.setIcon(estado[5]);
                    break;
                    case 2: this.setIcon(estado[6]);
                    break;
                    case 3: this.setIcon(estado[7]);
                    break;
                }
            break;
            case "R":
                switch(laDireccion) {
                    case 0: this.setIcon(estado[8]);
                    break;
                    case 1: this.setIcon(estado[9]);
                    break;
                    case 2: this.setIcon(estado[10]);
                    break;
                    case 3: this.setIcon(estado[11]);
                    break;
                }
            break;
        }
    }
}
