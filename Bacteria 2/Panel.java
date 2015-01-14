import javax.swing.*;
import java.awt.*;
import javax.swing.border.*;

class Panel extends JPanel{
    private ImageIcon imagenFondo;

    public Panel(LayoutManager elLayout, String elNombreArchivoImagenFondo) {
        this.setBorder(BorderFactory.createEtchedBorder(EtchedBorder.RAISED));
        this.setLayout(elLayout);
        this.setImagenFondo(elNombreArchivoImagenFondo);
    }

    public void paintComponent(Graphics grafico) {
        pintarFondo(grafico);
        super.paintComponent(grafico);
    }
    
    private void pintarFondo(Graphics grafico)  {
        if (imagenFondo != null) {
            Dimension tamanoPanel = this.getSize();
            grafico.drawImage(imagenFondo.getImage(), 0, 0, tamanoPanel.width, tamanoPanel.height, null);
            this.setOpaque(false);
        }
    }

    private void setImagenFondo(String elNombreArchivoImagenFondo) {
        if (elNombreArchivoImagenFondo == null) {
            imagenFondo = null;
        }else{
            imagenFondo = new ImageIcon(elNombreArchivoImagenFondo);
        }
    }
}