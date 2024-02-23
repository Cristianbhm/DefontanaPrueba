USE Prueba;

--El total de ventas de los últimos 30 días (monto total y cantidad total de ventas).
SELECT

	sum(v.Total) as TotalVentas,
	count(v.ID_Venta) as CantidadVentas
FROM 
	Venta as v 
WHERE
	Fecha >= DATEADD(DAY,-30,(
							     SELECT TOP 1 ve.Fecha
								 FROM 
									Venta as ve
								 ORDER BY
									ve.Fecha 
								 DESC
						     )
					) ;

--El día y hora en que se realizó la venta con el monto más alto (y cuál es aquel monto).
SELECT 
	Convert(VARCHAR(10), v.Fecha, 105) AS DiaVenta, 
	Convert(VARCHAR(5), v.Fecha, 108)  AS HoraVenta, 
	v.Total 
FROM  
	Venta AS v 
WHERE 
	v.total = (
				SELECT 
					MAX(ve.Total) AS Total 
				FROM 
					Venta AS ve
				WHERE
					Fecha >= DATEADD(DAY,-30,(
							     SELECT TOP 1 ve.Fecha
								 FROM 
									Venta as ve
								 ORDER BY
									ve.Fecha 
								 DESC
						     )
					) 
); 

--Indicar cuál es el producto con mayor monto total de ventas. 

	SELECT SUM(det.TotalLinea) AS totalVenta,
			p.Nombre AS nombreProd
	FROM 
		VentaDetalle AS det
	JOIN Producto AS p ON det.ID_Producto = p.ID_Producto
	JOIN venta AS ven ON det.ID_Venta = ven.ID_Venta
	WHERE ven.Fecha >=  DATEADD(DAY,-30,(
							     SELECT TOP 1 ve.Fecha
								 FROM 
									Venta as ve
								 ORDER BY
									ve.Fecha 
								 DESC
						     ))
	GROUP BY 
		det.ID_Producto, p.Nombre
	ORDER BY 
		totalVenta 
	DESC


--Indicar el local con mayor monto de ventas.

SELECT sum(v.Total) AS totalVendido, l.Nombre 
FROM Venta AS v 
left join Local AS l ON v.ID_Local = l.ID_Local
WHERE v.Fecha >= DATEADD(DAY,-30,(
							     SELECT TOP 1 ve.Fecha
								 FROM 
									Venta as ve
								 ORDER BY
									ve.Fecha 
								 DESC
						     ))
GROUP BY v.ID_local, l.Nombre
order by totalVendido desc

--¿Cuál es la marca con mayor margen de ganancias?

SELECT	
		sum(det.TotalLinea) AS cantidadVenta,
		mar.Nombre 
FROM 
		VentaDetalle AS det 
LEFT JOIN Producto AS prod ON det.ID_Producto = prod.ID_Producto
LEFT JOIN Marca AS mar ON prod.ID_Marca = mar.ID_Marca
LEFT JOIN Venta AS v ON det.ID_Venta = v.ID_Venta
WHERE v.Fecha >= DATEADD(DAY,-30,(
							     SELECT TOP 1 ve.Fecha
								 FROM 
									Venta as ve
								 ORDER BY
									ve.Fecha 
								 DESC
						     ))
GROUP BY 
	det.ID_Producto, mar.Nombre
ORDER BY
	cantidadVenta
DESC

--¿Cómo obtendrías cuál es el producto que más se vende en cada local?

 SELECT TOP 1
	SUM(det.Cantidad) cantidadVendida,
	det.ID_Producto AS ProductoVendido, 
	ven.ID_Local FROM VentaDetalle AS det
 JOIN venta AS ven ON ven.ID_Venta = det.ID_Venta
 WHERE ven.Fecha >= DATEADD(DAY,-30,(
							     SELECT TOP 1 ve.Fecha
								 FROM 
									Venta as ve
								 ORDER BY
									ve.Fecha 
								 DESC
						     ))
 GROUP BY det.ID_Producto, ven.ID_Local
 ORDER BY cantidadVendida  DESC
 

