# Interfaz de monitoreo energetico

Nuestra actualidad aun siendo tan avanzada existe un gran deficit en cuanto a regulacion de consumo energetico y medicion de consumo energetico. La interfaz desarrollada en el lenguaje C# la cual cuenta con la capacidad de leer datos enviados desde una placa de desarrollo y testeada con el sensor o medidor PZEM-004T el cual tiene la función de leer diferentes datos de una conexion electrica AC (corriente alterana) tomando datos como:

- Voltaje
- Consumo
- Factor de potencia
- Potencia
- Frecuencia

Podemos hacer un muy buen analisis de resultados y ayudarnos de reglas de regulación.

## Sobre el programa
El desarrollo de esta aplicacion fue realizada en Visual Studio con el lenguaje C# y algun que otro paquete, como datos importantes a tener en cuenta es que la aplicacion esta en fase de pruebas y desarrollo, no cuenta con una version estable y funcional en todos los sentidos, como objetivo principal es poder crear una **Red de monitoreo inteligente | RMI ** el cual debe ser capaz de estar conectado con un servidor remoto poder monitorear los datos tanto en la aplicacion de escritorio como en una aplicacion web, por ende el proyecto tomara un rumbo IoT(Internet de las cosas), siendo una gran tecnologia y debe ser aprovechada.

## Explicaciones Principales
La interfaz cuenta con un estilo muy minimalista y lo mas limpio posible, se conecta atraves del puerto serial al dispositivo o microcontrolador, cuenta con un espacio de 3 barras de progreso circulares las cuales nos muestran ciertos datos que no se ve la necesidad de graficarlos pero si de tenerlos en cuenta,  los datos que se grafican son el factor de potencia y el consumo, los cuales  son de gran ayuda para ender mejor el proceso pos-lectura. 
