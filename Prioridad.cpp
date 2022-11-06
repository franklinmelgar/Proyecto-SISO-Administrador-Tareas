#include <iostream>
#include <stdlib.h>
using namespace std;

struct nodo
{
    char dato;
    int priori;       
    struct nodo *sgte;
};


struct cola
{
    nodo *delante;
    nodo *atras  ;
};


struct nodo *crearNodo( char x, int pr)
{
    struct nodo *nuevoNodo = new(struct nodo);
    nuevoNodo->dato = x;
    nuevoNodo->priori = pr;
    return nuevoNodo;
};


void encolar( struct cola &q, char valor, int priori )
{
     
     struct nodo *aux = crearNodo(valor, priori);
     aux->sgte = NULL;
     
     if( q.delante == NULL)
         q.delante = aux;   
     else
         (q.atras)->sgte = aux;

     q.atras = aux;        
}


void muestraCola( struct cola q )
{
     struct nodo *aux;
     
     aux = q.delante;
     
     cout << " Proceso  Prioridad " << endl;
     cout << " ------------------- " << endl;
          
     while( aux != NULL )
     {
            cout<<"    "<< aux->dato << "     |   " << aux->priori << endl;
            aux = aux->sgte;
     }    
}


void ordenarPrioridad( struct cola &q )
{
     struct nodo *aux1, *aux2;
     int p_aux;
     char c_aux;
     
     aux1 = q.delante;
     
     while( aux1->sgte != NULL)
     {
            aux2 = aux1->sgte;
            
            while( aux2 != NULL)
            {
                   if( aux1->priori > aux2->priori )
                   {
                       p_aux = aux1->priori;
                       c_aux = aux1->dato;
                       
                       aux1->priori = aux2->priori;
                       aux1->dato   = aux2->dato;
                       
                       aux2->priori = p_aux;
                       aux2->dato   = c_aux;
                   }
                   
                   aux2 = aux2->sgte;
            }
            
            aux1 = aux1->sgte;
     }
}

void insertar( struct cola &q, char c, int pr )
{
   
     encolar( q, c, pr );
     
    
     ordenarPrioridad( q ); 
}


void menu()
{
    cout<<"\n\t Algoritmo de Prioridad \n\n";
    cout<<" 1. Insertar proceso                           "<<endl;
    cout<<" 2. Mostrar resultados                           "<<endl;
    cout<<" 3. Salir                             "<<endl;

    cout<<"\n Ingrese opcion deseada: ";
}


int main()
{
    struct cola q;
    
    q.delante = NULL;
    q.atras   = NULL;

    char c ;    
    int pr;      
    int op;      
    int x ;      
    
    do
    {
        menu();  cin>> op;

        switch(op)
        {
            case 1:

                 cout<< "\n Ingrese caracter: ";
                 cin>> c;
                 
                 cout<< "\n Ingrese prioridad: ";   
                 cin>> pr;
                 
                 insertar( q, c, pr );
                 
                 cout<<"\n\n\t\tProceso '" << c << "' agregado\n\n";
            break; 
                  
            case 2:

                 cout << "\n\n Resultados de Prioridad\n\n";
                 
                 if(q.delante!=NULL)
                     muestraCola( q );
                 else   
                    cout<<"\n\n\tSin procesos..."<<endl;
            break;
            
            default:
                    cout<<"\n\tOpcion incorrecta"<<endl;
                    system("pause");
                    exit(0);
         }

        cout<<endl<<endl;
        system("pause");  system("cls");

    }while(op!=3);
    
    return 0;
}
