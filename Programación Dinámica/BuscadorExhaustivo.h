#ifndef BUSCADOREXHAUSTIVO_H
#define BUSCADOREXHAUSTIVO_H

#include <vector>
#include <iostream>
#include <cstdlib>

using namespace std;

template <class T>
class BuscadorExhaustivo {
public:
    BuscadorExhaustivo();
    ~BuscadorExhaustivo();

    vector<T>& busquedaExhaustiva(const vector<int>&);
    vector<T>& programacionDinamica(const vector<int>&);
    vector<T>& programacionDinamica2(const vector<int>&);

protected:
private:
    vector<int> sigma;
    vector<int> notas;
    int cantidadNotas;
    int pases[5];
    vector<int> teclado;
    int intervalos [5][5];

    int ** matrizDinamica;
    int ** sigmaDinamica;

    void llenarIntervalos();
    void completarDatos();
    void mapearTeclado();
    void completarSigma();
    bool validarMovimiento(int,int,int);
    int fase(int,int,int);
    int esNegra(int);
    int sacarSigma();

    void crearMatrices();

    void imprimirMatriz(int **);
};
template <class T>
BuscadorExhaustivo<T>::BuscadorExhaustivo() {
    mapearTeclado();
    completarDatos();
}

template <class T>
BuscadorExhaustivo<T>::~BuscadorExhaustivo() {
}


template <class T>
int BuscadorExhaustivo<T>::esNegra(int laTecla)    {
    return (teclado[laTecla-21]);
}


template <class T>
void BuscadorExhaustivo<T>::mapearTeclado()    {
    teclado.resize(88,0);
    teclado[1] = 1;
    int u = 4;

    for(int i = 0; i < 7; i++)   {
        teclado[u] = 1;
        u+=2;
        teclado[u] = 1;
        u+=3;
        teclado[u] = 1;
        u+=2;
        teclado[u] = 1;
        u+=2;
        teclado[u] = 1;
        u+=3;
    }
}


template <class T>
void BuscadorExhaustivo<T>::completarDatos()   {
    pases[0] = pases[4] = 0;
    pases[1] = 7;
    pases[2] = 6;
    pases[3] = 4;

    for(int i = 0; i < 5; i++)   {
        for(int j = 0; j < 5; j++)   {
            intervalos[i][j] = 0;
        }
    }

    intervalos[0][1] = intervalos[1][0] = 6;
    intervalos[0][2] = intervalos[2][0] = 9;
    intervalos[0][3] = intervalos[3][0] = 10;
    intervalos[0][4] = intervalos[4][0] = 12;
    intervalos[1][2] = intervalos[2][1] = 5;
    intervalos[1][3] = intervalos[3][1] = 7;
    intervalos[1][4] = intervalos[4][1] = 9;
    intervalos[2][3] = intervalos[3][2] = 3;
    intervalos[2][4] = intervalos[4][2] = 5;
    intervalos[3][4] = intervalos[4][3] = 4;
}


template <class T>
int BuscadorExhaustivo<T>::fase(int numeroNota,int dedoAnterior,int pasesAcumulados)   {
    int pasesMinimos = 99999;
    vector<T> sigma2;
    sigma2.resize(cantidadNotas,9);
    int pases = 0;
    int dedoOptimo = 9;
    int notaActual = 0;
    int notaAnterior = -1;

    if(numeroNota == cantidadNotas)  {
        return pasesAcumulados;
    }   else    {
        if(numeroNota != 0) {
            notaActual = (notas)[numeroNota];
            notaAnterior = (notas)[numeroNota - 1];
        }
        for(int dedo = 0; dedo < 5; dedo++) {
            if(notaActual != -1)    {   //Reviso que no vaya a tocar la misma nota con el mismo dedo
                if(notaAnterior == notaActual && dedo == dedoAnterior)  {
                } else if(validarMovimiento(numeroNota, dedoAnterior, dedo)) {           //Veo si el mov es valido
                    //cout << "dedo: " << dedo+1 <<" nota: " << numeroNota + 1 << " pases: " << pases << endl;
                    if(dedoAnterior == 0 && notaAnterior > notaActual)    {     //Reviso si es un pase de pulgar hacia abajo
                        //cout << "pase de pulgar hacia abajo con dedo: " << dedo+1 << " hacia la nota: " << numeroNota+1 << endl;
                        pases = fase(numeroNota + 1,dedo,pasesAcumulados+1);
                    }   else if(notaAnterior < notaActual && dedo == 0 && (dedoAnterior > 0 && dedoAnterior < 4)) { //Pase de pulgar hacia arriba
                        //cout << "pase de pulgar hacia arriba con dedo: " << dedo+1 << " hacia la nota: " << numeroNota+1 << endl;
                        pases = fase(numeroNota + 1,dedo,pasesAcumulados+1);
                    }   else    {   //no hay pase de pulgar
                        pases = fase(numeroNota + 1,dedo,pasesAcumulados);
                    }
                    if(pases <= pasesMinimos)    {   //Comparo cant de pases
                        pasesMinimos = pases;
                        sigma2 = sigma;
                        dedoOptimo = dedo;
                        //cout << "guardo en sigma[" << numeroNota <<"] el dedo: " << dedo+1 << endl;
                        sigma[numeroNota] = dedo;
                    }
                }
            }
        }
        sigma = sigma2;
        sigma[numeroNota] = dedoOptimo;
    }
    return pasesMinimos;
}

template <class T>
bool BuscadorExhaustivo<T>::validarMovimiento(int posNota,int dedoAnterior, int dedoActual)    {
    int notaActual = notas[posNota];
    int notaAnterior = 0;
    bool valido = false;
    if(posNota != 0)    {
        notaAnterior = notas[posNota-1];
    }
    int dif = abs(notaActual-notaAnterior);

    if (dedoAnterior != dedoActual) {
        if (posNota == 0) {
            if (dedoActual == 0 && !esNegra(notaActual)) {
                valido = true;
            } else if(dedoActual != 0) {
                valido = true;
            }
        } else if (notaAnterior == notaActual) {

            if ( (dedoAnterior > dedoActual) && (dedoActual != 0) ) {
                valido = true;
            } else if ( (dedoAnterior == 0) && (dedoActual > 0) ) {
                valido = true;
            } else if ( dedoActual == 0 ) {
                valido = true;
            }
        } else if ( notaAnterior < notaActual ) { //secuencia ascendente
            if ( dedoActual != 0 ) {
                if ( dif <= intervalos[dedoAnterior][dedoActual] && dedoActual > dedoAnterior) {
                    valido = true;
                }
            } else { //else pulgar
                if ( !esNegra(notaActual) && (dif <= pases[dedoAnterior]) ) {
                    valido = true;
                }
            }
        } else { //secuencia descendente
            if ( (dedoAnterior != 0) ) {
                if ( dif <= intervalos[dedoAnterior][dedoActual] && dedoActual < dedoAnterior) {
                    if (dedoActual == 0 && !esNegra(notaActual)) {
                        valido = true;
                    }   else if(dedoActual != 0) {
                        valido = true;
                    }
                }
            }  else {//else pulgar
                if ( dif <= pases[dedoActual] ) {//pase pulgar descendente
                    valido = true;
                }
            }
        }
    }
    return valido;
}

template <class T>
void BuscadorExhaustivo<T>::completarSigma()    {
    for(int i = 0; i < cantidadNotas; i++)  {
        sigma[i] += 1;
    }
}

template <class T>
vector<T>& BuscadorExhaustivo<T>::busquedaExhaustiva(const vector<int>& melodia)    {
    int pasesTotales = 0;
    notas = melodia;
    cantidadNotas = notas.size();
    sigma.resize(cantidadNotas,9);


    pasesTotales = this->fase(0,-1,0);

    if(pasesTotales != 99999)   {
        this->completarSigma();
        for(int o = 0; o<cantidadNotas; o++)  {
            cout << notas[o] << " - "<< sigma[o] << endl;
        }

        cout << endl << "Pases Totales: " << pasesTotales << endl;
    }   else    {
        cout << "No hay solucion valida." << endl;
    }
    return sigma;
}

template <class T>
void BuscadorExhaustivo<T>::crearMatrices() {
    matrizDinamica = new int*[cantidadNotas+1];
    sigmaDinamica = new int*[cantidadNotas+1];
    for(int i = 0; i <= cantidadNotas; i++)   {
        matrizDinamica[i] = new int [5];
        sigmaDinamica[i] = new int[5];
    }

    for(int i = 0; i <= cantidadNotas; i++)   {
        matrizDinamica[i] = new int [5];
        sigmaDinamica[i] = new int[5];
        for(int j = 0; j < 5; j++)  {
            matrizDinamica[i][j] = 0;
            sigmaDinamica[i][j] = 0;
        }
    }
}

template <class T>
vector<T>& BuscadorExhaustivo<T>::programacionDinamica(const vector<int>& melodia) {
    int notaActual = 0;
    int notaAnterior = 0;

    int dedoOptimo = 15;
    int pasesActuales = 99999;
    int pasesMinimosActuales = 99999;
    int pasesAnteriores = 99999;
    int valido = 0;
    int encontro = 0;
    notas = melodia;
    cantidadNotas = notas.size();
    sigma.resize(cantidadNotas,9);

    crearMatrices();

    //Caso Base: nota ficticia, 0 pases, se inicializa en crear Matrices

    //Recursivo
    for(int numeroNota = 1; numeroNota < cantidadNotas; numeroNota++)   {
        notaActual = notas[numeroNota];
        notaAnterior = notas[numeroNota - 1];

        for(int dedo = 0; dedo < 5; dedo++) {
            for(int dedoAnterior = 0; dedoAnterior < 5; dedoAnterior++) {
                pasesAnteriores = matrizDinamica[numeroNota - 1][dedoAnterior];
                valido = validarMovimiento(numeroNota,dedoAnterior,dedo);
                if(pasesAnteriores != 99999 && valido)    {
                    if(dedoAnterior == 0 && notaAnterior > notaActual)    {     //Reviso si es un pase de pulgar hacia abajo
                        pasesActuales = matrizDinamica[numeroNota -1][dedoAnterior] + 1;
                    } else if(notaAnterior < notaActual&& dedo == 0 && (dedoAnterior > 0 && dedoAnterior < 4)) { //Pase de pulgar hacia arriba
                        pasesActuales = matrizDinamica[numeroNota -1][dedoAnterior] + 1;
                    } else { //no hay pase de pulgar
                        pasesActuales = matrizDinamica[numeroNota -1][dedoAnterior];
                    }
                } else if(!encontro) {
                    sigmaDinamica[numeroNota][dedo] = -1;
                }
                if(valido && pasesAnteriores != 99999 && pasesActuales <= pasesMinimosActuales) {
                    pasesMinimosActuales = pasesActuales;
                    dedoOptimo = dedoAnterior;
                    sigmaDinamica[numeroNota][dedo] = dedoOptimo;
                    encontro = 1;
                }
            }
            matrizDinamica[numeroNota][dedo] = pasesMinimosActuales;
            if(!encontro)   {
                matrizDinamica[numeroNota][dedo] = 99999;
            }
            encontro = 0;
            pasesMinimosActuales = 99999;
        }
    }

    //imprimirMatriz(matrizDinamica);
    //imprimirMatriz(sigmaDinamica);
    pasesActuales = sacarSigma();
    if(pasesActuales != -1) {
        completarSigma();
        for(int numeroNota = 0; numeroNota < cantidadNotas; numeroNota++)   {
            cout << notas[numeroNota] << " - " << sigma[numeroNota] << endl;
        }

        cout << endl << "Pases Totales: " << pasesActuales << endl;
    }   else    {
        sigma.clear();
        cout << "No hay solucion valida." << endl;
    }

    return sigma;
}

template <class T>
int BuscadorExhaustivo<T>::sacarSigma()   {
    int mejorOpcion = 0;
    int pasesOptimos = 999;

    for(int i = 0; i< 5; i++)   {
        if(matrizDinamica[cantidadNotas-1][i] <= pasesOptimos)    {
            mejorOpcion = i;
            pasesOptimos = matrizDinamica[cantidadNotas-1][i];
        }
    }

    sigma[cantidadNotas - 1] = mejorOpcion;
    if(pasesOptimos != 999)   {
        for(int numeroNota = cantidadNotas - 1; numeroNota >= 0; numeroNota--) {
            mejorOpcion = sigma[numeroNota - 1] = sigmaDinamica[numeroNota][mejorOpcion];
        }
    }   else    {
        pasesOptimos = -1;
    }


    return pasesOptimos;
}


template<class T>
void BuscadorExhaustivo<T>::imprimirMatriz(int ** laMatriz) {
    for(int i = 0; i<cantidadNotas; i++)  {
        if(i<cantidadNotas) {
            cout << notas[i] << " / ";
        }

        for(int j = 0; j<5; j++)  {
            cout << laMatriz[i][j] << " ";
        }
        cout << endl;
    }
    cout << endl;
}
#endif // BUSCADOREXHAUSTIVO_H
