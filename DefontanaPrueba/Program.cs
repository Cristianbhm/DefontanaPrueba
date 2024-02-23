
using DefontanaPrueba.Entidades;
using DefontanaPrueba.Models;
using DefontanaPrueba.Negocio;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DefontanaPrueba
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Respuestas respuestas = new Respuestas();
            PruebaNegocio neg = new PruebaNegocio();
            respuestas = neg.traeRespuestas();


            Console.WriteLine("Pregunta 1: El total de ventas de los últimos 30 días (monto total y cantidad total de ventas).");
            Console.WriteLine("Respuesta: " + respuestas.resp1.ToString());
            Console.WriteLine("\r\n");


            Console.WriteLine("Pregunta 2: El día y hora en que se realizó la venta con el monto más alto (y cuál es aquel monto).");
            Console.WriteLine("Respuesta : " + respuestas.resp2.ToString());
            Console.WriteLine("\r\n");


            Console.WriteLine("Pregunta 3: Indicar cuál es el producto con mayor monto total de ventas.");
            Console.WriteLine("Respuesta : " + respuestas.resp3.ToString());
            Console.WriteLine("\r\n");

            Console.WriteLine("Pregunta 4: Indicar el local con mayor monto de ventas.");
            Console.WriteLine("Respuesta : " + respuestas.resp4.ToString());
            Console.WriteLine("\r\n");

            Console.WriteLine("Pregunta 5: ¿Cuál es la marca con mayor margen de ganancias?.");
            Console.WriteLine("Respuesta : " + respuestas.resp5.ToString());
            Console.WriteLine("\r\n");

            Console.WriteLine("Pregunta 6:--¿Cómo obtendrías cuál es el producto que más se vende en cada local?.");
            Console.WriteLine("Respuesta : " + respuestas.resp6.ToString());
            Console.WriteLine("\r\n");

        }



    }

}


     