import javax.swing.JMenuBar;
import javax.swing.JMenu;
import javax.swing.JMenuItem;
import java.awt.event.*;

public class Menu extends JMenuBar {
    private JMenu[] menues;
    private static final String[][] MENU = {{"Programa", "Reiniciar", "Salir"},{"Ayuda","Instrucciones", "Acerca de...", null, "Créditos"}};
  
    public Menu(ActionListener elEscuchador) {
        menues = new JMenu[MENU.length];

        for (int indiceMenu=0;indiceMenu<MENU.length; indiceMenu++) {
            menues[indiceMenu] = new JMenu(MENU[indiceMenu][0]);
            this.add(menues[indiceMenu]);

            for (int indiceItem=1; indiceItem<MENU[indiceMenu].length; indiceItem++) {
                String etiquetaItem = MENU[indiceMenu][indiceItem];
                if (etiquetaItem == null) {
                    menues[indiceMenu].addSeparator();
                }else{
                    JMenuItem item = new JMenuItem(etiquetaItem);
                    item.addActionListener(elEscuchador);
                    menues[indiceMenu].add(item);
                }
            }
        }
    }
  
}