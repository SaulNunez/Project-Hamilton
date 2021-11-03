# A night in the house of Hunt

Un juego para reforzar el conocimiento de los temas de programación estructurada.

## Mécanicas del juego
Este juego esta inspirado en Among Us, se separan aleatoriamente los jugadores entre programadores e impostores.

### Programadores
El trabajo de los programadores es completar todos los ejercicios para ganar. También pueden ganar si encuentran y sacan a todos los impostores.

### Impostores
Evita que los programadores terminen sus tareas. Pueden hacerlo creando emergencias que obliguen a los programadores a tener que moverse al otro lado del mapa. O matar en el juego a todos los programadores. Resuelve acertijos para obtener modo fantasma, este acertijo no cuenta para que los programadores terminen sus tareas, pero dan 30 segundos para escapar de cualquier situación díficil de ser necesario. Evita ser encontrado, porque si los programadores sacan a todos los impostores, ganan los programadores. 

Este juego fue creado como proyecto de titulación para la carrera de Ingeniería en Software.

## Como ejecutarlo localmente
El juego usa una arquitectura cliente/servidor. El docente o alguien designado del grupo tiene que instalar el servidor en una computadora o servidor propio. Esta maquina tiene que tener instalado Docker y Docker Compose.

1. Clona este proyecto descargando el .zip del proyecto o usando `git clone`.
2. Abre una terminal en la carpeta del proyecto y orquestra los contenedores usando `docker-compose up`.
3. Si introduce `localhost:80` en el navegador, debería de cargarse el juego.
4. Usa `ipconfig` para Windows o `ifconfig` en Linux o MacOS para obtener tu dirección IP si todos estan jugador en la misma red local.

## Puzzles
Tiene acertijos para repasar distintos temas de programación estructurada como:
* If sencillo
* If/else
* Operador condicionales
* Ciclo For (Para)
* Ciclo While (Mientras)
* Ciclo Do While (Repetir)

Los puzzles estan creados con pseudocódigo con la sintaxis de PSeInt.

### Puzzles para reforzar creación de algoritmos
![Puzzle secuencia 1](https://user-images.githubusercontent.com/13012976/140199758-fc786a64-caef-4a97-8be9-f14136b61c77.png)
![Puzzle secuencia 2](https://user-images.githubusercontent.com/13012976/140199760-8e1fa496-ad76-472a-a4a6-00334004f3ee.png)
![Puzzle secuencia 3](https://user-images.githubusercontent.com/13012976/140199762-46f0ace3-5331-4c3f-9e9d-3d465c5acb9e.png)

### If/else (si/no)
![Puzzle If/else](https://user-images.githubusercontent.com/13012976/140199767-61923ef4-0331-44fe-a96c-f0a417e13752.png)
![Puzzle If](https://user-images.githubusercontent.com/13012976/140199778-c3184f3f-8917-47c5-a086-01b3728daef5.png)
![Puzzle If](https://user-images.githubusercontent.com/13012976/140199780-1a8404e4-e596-4e6a-a015-ded35999acad.png)
![Puzzle If](https://user-images.githubusercontent.com/13012976/140199782-f46b716e-ba10-4ab4-b942-1c54e0d7bc5e.png)

### Ciclos
#### Elegir ciclo que hace la acción necesaria
![Elegir ciclo correcto 1](https://user-images.githubusercontent.com/13012976/140199766-f824b2d2-ec6f-4a2b-91d4-f3e6e5889d5d.png)
![Elegir ciclo correcto 2](https://user-images.githubusercontent.com/13012976/140199783-a54f6b39-edac-4f25-9d2f-814567910c9b.png)
![Elegir ciclo correcto 3](https://user-images.githubusercontent.com/13012976/140214693-057c4e54-00c6-40f8-ab59-5d5463a5023c.png)

#### Completar los ciclos
![Completar For](https://user-images.githubusercontent.com/13012976/140199771-4eec29bc-cc26-4050-8c4d-f3a3021c21b6.png)
![Completar while](https://user-images.githubusercontent.com/13012976/140199781-df60beec-33b8-41f3-99ca-dc750f8ba7f7.png)

#### Contar vueltas
![Contar vueltas For](https://user-images.githubusercontent.com/13012976/140199775-8a7cb22f-f633-458e-bb33-5beaed575f7b.png)
