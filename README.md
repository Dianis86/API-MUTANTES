# API Mutantes

![](/Imagenes/apix128.png)

**NuGet Package Utilizados: **

- [FireSharp V.2.0.4](https://github.com/ziyasal/FireSharp "FireSharp V.2.0.4")
- [FireSharp.Serialization.JsonNet V.1.1.0](https://github.com/ziyasal/FireSharp "FireSharp.Serialization.JsonNet V.1.1.0")
- [Swashbuckle.AspNetCore (Swagger) V.5.6.3](https://github.com/domaindrivendev/Swashbuckle.AspNetCore "Swashbuckle.AspNetCore (Swagger) V.5.6.3")

**Versión .NET Framework: **

.Net Core 3.1

**Versión Visual Studio:**

Microsoft Visual Studio Community 2019
Version 16.11.12

**Base de Datos:**

Base de datos no relacional, basada en colecciones y documentos.
Firebase - Módulo:  Realtime DataBase

**Repositorio GitHub:**

https://github.com/Dianis86/API-MUTANTES


#### Descripción del Proyecto:

API REST que recibe un array de Strings que representa una secuencia de ADN, realiza validacilón de que la cadena sea válida y revisa si es ADN humano ó mutante. Luego envía los valores a una base de datos en la cual se tiene dos colecciones, una de ADNs mutantes y otra de ADNs humanos. Para luego sacar estadísticas basadas en los datos ingresados.

Cada cadena de string representa una fila de una tabla de NxN. 
En base a ello se recorre la matriz de forma vertical, horizontal y diagonal.
Para las diagonales se realiza el recorrido de la siguiente manera:

![](/Imagenes/Recorrido1.png)
Se recorre de (izquierda a derecha) y de (abajo hacia arriba), teniendo como resultado:

![](/Imagenes/Resultante_Recorrido1.png)

![](/Imagenes/Recorrido2.png)
Se recorre de (izquierda a derecha) y de (abajo hacia arriba), teniendo:

![](/Imagenes/Resultante_Recorrido2.png)

**Ejemplo de Secuencia**

String[] dna = {"ATGCGA", "CAGTGC", "TTATGT", "AGAAGG", "CCCCTA", "TCACTG"}

En esta API se tienen los siguientes servicios:

-  **/mutant **   sirve para validar si la secuencia de ADN es Mutante ó Humano
-  ** /stats**  devuelve un Json con las estadísticas de las verificaciones de ADN
