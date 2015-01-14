#include <iostream>
#include <vector>

#include "BuscadorExhaustivo.h"
#include <fstream>
#include "windows.h"

using namespace std;


vector<int> melodia2;
vector<int> sigma;

template <typename T>
vector<T>& programacionDinamica(const vector<int>& melodia);
template <typename T>
vector<T>& algoritmoAvido(const vector<int>& melodia);

void leerMelodia(const char* archivoMelodia)    {
    int nota = 0;

    ifstream archivoEntrada(archivoMelodia, ios::in);

    if(!archivoEntrada) {
        cout << "No se puede abrir el archivo" << endl;
        exit(1);
    }   else    {
        while(archivoEntrada >> nota)   {
            melodia2.push_back(nota);
        }
    }

}

double calcularTiempo(LARGE_INTEGER *fin, LARGE_INTEGER *inicio)    {
    LARGE_INTEGER frecuencia;

    QueryPerformanceFrequency(&frecuencia);

    return (double) (fin->QuadPart - inicio->QuadPart) / (double) frecuencia.QuadPart;
}

int main () {
    leerMelodia("Melodia.txt");

    LARGE_INTEGER inicio, fin;
    double duracion = 0.0;

    BuscadorExhaustivo<int> b;
    QueryPerformanceCounter(&inicio);
    //sigma = b.busquedaExhaustiva(melodia2);
    sigma = b.programacionDinamica(melodia2);
    QueryPerformanceCounter(&fin);
	duracion = calcularTiempo(&fin,&inicio);
	printf("\nDuracion: %.16g milisegundos\n\n",duracion * 1000.0);

    return 0;
}
