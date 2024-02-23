using DefontanaPrueba.Entidades;
using DefontanaPrueba.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DefontanaPrueba.Negocio
{
    internal class PruebaNegocio
    {
        public Respuestas traeRespuestas()
        {

            Respuestas respuestas = new Respuestas();
            List<Ventum> resultadoVentas = ConsultaVentas();

            respuestas.resp1 = TotalVentas(resultadoVentas);
            respuestas.resp2 = ConsultaDiaHoraVenta(resultadoVentas);
            respuestas.resp3 = ConsultaProductoConMasVentas(resultadoVentas);
            respuestas.resp4 = ConsultaLocalMasVentas(resultadoVentas);
            respuestas.resp5 = ConsultaProductoMasGanancia(resultadoVentas);
            respuestas.resp6 = ConsultaProductoMasVendido(resultadoVentas);


            return respuestas;

        }

        public List<Ventum> ConsultaVentas()
        {

            PruebaContext dbVenta = new PruebaContext();
            List<Ventum> listVenta = new List<Ventum>();
            List<Ventum> listUltimas30 = new List<Ventum>();
            var ventas = dbVenta.Venta.ToList();
            string fechaUltimaVenta = ventas.OrderByDescending(f => f.Fecha)
                                                   .Select(s => s.Fecha)
                                                   .FirstOrDefault().AddDays(-30).ToShortDateString();
            listUltimas30 = ventas.Where(x => x.Fecha >= Convert.ToDateTime(fechaUltimaVenta)).ToList();
            return listUltimas30;


        }

        public string TotalVentas(List<Ventum> totalVentas)
        {
            try
            {
                string sumaTotal = string.Empty;
                string cantidadVentas = string.Empty;
                string resultado = string.Empty;

                sumaTotal = totalVentas.Sum(s => s.Total).ToString("C", CultureInfo.CreateSpecificCulture("es-CL"));
                cantidadVentas = totalVentas.Count.ToString();
                resultado = "Monto Total: " + sumaTotal + ", Cantidad de ventas: " + cantidadVentas;
                return resultado;
            }
            catch
            {
                return "Problemas al cargas la respuesta";
            }

        }

        public string ConsultaDiaHoraVenta(List<Ventum> totalVentas)
        {
            try
            {
                string diaVenta = string.Empty;
                string horaVenta = string.Empty;
                int montoMax = 0;

                montoMax = totalVentas.OrderByDescending(x => x.Total)
                                        .Select(s => s.Total)
                                        .FirstOrDefault();
                var vardetalleVenta = totalVentas.Where(x => x.Total == montoMax);
                diaVenta = vardetalleVenta.First().Fecha.ToShortDateString();
                horaVenta = vardetalleVenta.First().Fecha.ToShortTimeString();



                return "Día Venta " + diaVenta + " Hora Venta " + horaVenta + " Total Venta " + montoMax.ToString("C", CultureInfo.CreateSpecificCulture("es-CL"));

            }
            catch
            {
                return "Problemas al cargar respuesta";
            }



        }

        public string ConsultaProductoConMasVentas(List<Ventum> totalVentas)

        {
            try
            {
                string nombreProducto = string.Empty;
                string valorVendido = string.Empty;
                PruebaContext db = new PruebaContext();
                List<VentaDetalle> listVentaDetalle = new List<VentaDetalle>();
                List<Producto> listProductos = new List<Producto>();

                listProductos = db.Productos.ToList();
                listVentaDetalle = db.VentaDetalles.ToList();

                var resultadoVentas = from VentaDet in listVentaDetalle
                                      join producto in listProductos on VentaDet.IdProducto equals producto.IdProducto
                                      join totalVenta in totalVentas on VentaDet.IdVenta equals totalVenta.IdVenta
                                      group VentaDet by new { VentaDet.IdProducto, producto.Nombre } into g
                                      orderby g.Sum(s => s.TotalLinea) descending
                                      select new { totalVentaProducto = g.Sum(s => s.TotalLinea), nombreProd = g.Key.Nombre };
                nombreProducto = resultadoVentas.Select(s => s.nombreProd).First();
                valorVendido = resultadoVentas.Select(s => s.totalVentaProducto).First().ToString("C", CultureInfo.CreateSpecificCulture("es-CL"));

                return "El producto con mayor total de ventas es " + nombreProducto + " con un total de " + valorVendido;
            }

            catch 
            {
                return "Problemas al cargar respuesta";
            }

        }

        public string ConsultaLocalMasVentas(List<Ventum> totalVentas)
        {
            try
            {
                string nombreLocal = string.Empty;
                string cantidadVendida = string.Empty;
                List<Local> listLocal = new List<Local>();
                PruebaContext db = new PruebaContext();
                listLocal = db.Locals.ToList();


                var resultado = from totalVenta in totalVentas
                                join locales in listLocal on totalVenta.IdLocal equals locales.IdLocal
                                group totalVenta by new { totalVenta.IdLocal, locales.Nombre } into g
                                orderby g.Sum(s => s.Total) descending
                                select new { totalVendido = g.Sum(s => s.Total), nombreLocal = g.Key.Nombre };

                nombreLocal = resultado.Select(s => s.nombreLocal).First();
                cantidadVendida = resultado.Select(s => s.totalVendido).First().ToString("C", CultureInfo.CreateSpecificCulture("es-CL"));

                return "El local con mas ventas es " + nombreLocal + " con un total de " + cantidadVendida;
            }
            catch
            {
                return "Problemas al cargar respuesta";
            }

        }


        public string ConsultaProductoMasGanancia(List<Ventum> totalVentas)
        {

            try
            {
                PruebaContext db = new PruebaContext();
                List<VentaDetalle> listVentaDetalle = new List<VentaDetalle>();
                List<Producto> listProductos = new List<Producto>();
                List<Marca> listMarca = new List<Marca>();
                string nombreMarca = string.Empty;
                string cantidadVendida = string.Empty;
                listVentaDetalle = db.VentaDetalles.ToList();
                listProductos = db.Productos.ToList();
                listMarca = db.Marcas.ToList();

                var resultado = from listDet in listVentaDetalle
                                join Prod in listProductos on listDet.IdProducto equals Prod.IdProducto
                                join Marcas in listMarca on Prod.IdMarca equals Marcas.IdMarca
                                join totalVen in totalVentas on listDet.IdVenta equals totalVen.IdVenta
                                group listDet by new { listDet.IdProducto, Marcas.Nombre } into g
                                orderby g.Sum(g => g.TotalLinea) descending
                                select new { cantidadVendida = g.Sum(s => s.TotalLinea), nombreMarca = g.Key.Nombre };
                nombreMarca = resultado.Select(s => s.nombreMarca).First();
                cantidadVendida = resultado.Select(s => s.cantidadVendida).First().ToString("C", CultureInfo.CreateSpecificCulture("es-CL"));


                return "La marca mas vendida es " + nombreMarca + " con un total de " + cantidadVendida;
            }
            catch
            {
                return "Problemas al cargar respuesta";
            }

        }

        public string ConsultaProductoMasVendido(List<Ventum> totalVentas)
        {
            try
            {
                PruebaContext db = new PruebaContext();
                List<VentaDetalle> listVentaDetalle = new List<VentaDetalle>();
                List<Producto> listProductos = new List<Producto>();
                List<Local> listLocales = new List<Local>();
                listVentaDetalle = db.VentaDetalles.ToList();
                listProductos = db.Productos.ToList();
                listLocales = db.Locals.ToList();
                string productoVendido = string.Empty;
                string localmasVendido = string.Empty;
                var resultado = from ventaDet in listVentaDetalle
                                join totalVenta in totalVentas on ventaDet.IdVenta equals totalVenta.IdVenta
                                group ventaDet by new { ventaDet.IdProducto, totalVenta.IdLocal } into g
                                orderby g.Sum(g => g.Cantidad) descending
                                select new { cantidadVendida = g.Sum(g => g.Cantidad), idProd = g.Key.IdProducto, idLocal = g.Key.IdLocal };

                var resultFinal = from det in resultado
                                  join prod in listProductos on det.idProd equals prod.IdProducto
                                  join locVenta in listLocales on det.idLocal equals locVenta.IdLocal
                                  orderby det.cantidadVendida descending
                                  select new { nombreProduc = prod.Nombre, locVenta.Nombre };

                productoVendido = resultFinal.Select(s => s.nombreProduc).First();
                localmasVendido = resultFinal.Select(s => s.Nombre).First();

                return "El producto mas vendido es " + productoVendido + " en el local de " + localmasVendido;
            }
            catch
            {
                return "Problemas al cargar respuesta";
            }
        }

    }
}
