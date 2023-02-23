# IAV - Base para la Práctica 1

## Autores
- Pablo Arredondo Nowak (PabloArrNowak)
- Mario Miguel Cuartero (mamigu05)

## Propuesta
A partir de la base proporcionada, se deben diseñar e implementar soluciones para los siguientes comportamientos, indicados en https://narratech.com/es/inteligencia-artificial-para-videojuegos/percepcion-y-movimiento/plaga-de-ratas/ :

Avatar del Jugador (**Flautista**): Se mueve por el mapa controlado por ratón y puede tocar la flauta con click derecho. El avatar del jugador es perseguido por el perro. Cuando el jugador toca la flauta, las ratas que se encuentren a una distancia marcada por un radio determinado, dejarán su movimiento desordenado y empezarán a seguirle. 

**Ratas**: Cuando la flauta no suena, deambulan por su cuenta de forma aleatoria y errática a una velocidad determinada y cambiando de dirección cada cierto tiempo. Una vez empieza a sonar, las ratas que se encuentren en proximidad al flautista, empiezan a seguirle. Cuando lleguen al radio interior, desacelerarán su velocidad hasta 0. Además, se producirá una separación entre ellas marcadas por un coeficiente de distancia.

**Perro**: Persigue al flautista con predicción, hasta que se le acercan 2 o más **ratas**, que lo hacen huir.

## Punto de partida
Se parte de un proyecto base de Unity proporcionado por el profesor aquí:
https://github.com/Narratech/IAV-P1

En la escena se encuentran:
- **Cámara**, que contiene un script que hace seguir al jugador.
- **Avatar**, al que puedes controlar por teclado haciendo que pueda rotar y moverse. Además también contiene el script "TocarFlauta" que le permite tocar la flauta.
- **Perro**, prefab que sólo tiene los componentes básicos (transform, rigidbody...)
- **Generador de ratas**, que responde al input del jugador por teclado y las genera en un mismo punto fijo, haciendo que se apilen una sobre otra. Estas ratas sólo tienen los componentes básicos (transform, rigidbody...)
- **Gestor de juego**, que contiene el script "GestorJuego" que permite informar al jugador, spawnear y despawnear ratas, volver a recargar la escena, cambiar la vista de la cámara...
- **Canvas**, que una vez ha comenzado el juego, muestra por pantalla información sobre el número de ratas que se encuentran en la escena; FPS actuales; cómo generar y eliminar ratas; cómo activar y desactivar obstáculos; y en general, información del input del teclado.
- **Obstaculos y escenario**, que está formado por una serie de casas y árboles y un pozo.

Se incluyen los siguientes scripts/clases:

- **Merodear**, que no está implementada, pero hará deambular a las ratas.

- **ControlJugador**, que hereda de la clase "ComportamientoAgente" y que, en principio, controlaba el movimiento y la dirección del avatar del jugador con el teclado, pero que ahora está implementado el control con ratón: se crea un rayo desde la cámara en la posición del ratón hacia el mundo, que solo colisiona con el suelo, y se acelera al avatar del jugador hacia el punto de colisión.

- **Llegada**, que hereda de la clase "ComportamientoAgente" y que hace que el agente que lo tenga siga a un agente asignado como objetivo.

- **Huir**, que no está implementado, pero hará huir al perro cuando haya más de dos ratas cerca de él.

- **Separacion**, que no está implementado, pero hará que las ratas mantengan su dirección pero con separación entre unas y otras.

- **TocarFlauta**, que activa varios sistemas de partículas al pulsar una tecla determinada y permite que las ratas activen y desactiven ciertos comportamientos que hacen que sigan o no al avatar del jugador.

- **Agente**, que es usado por todos los personajes y permite mover a los personajes en base a las direcciones que tiene.

- **ComportamientoAgente**, que es la clase de la cual heredan varios scripts de los personajes y que declaran ciertas variables y devuelven direcciones.

- **Direccion**, que es una clase que representa la dirección mediante aceleraciones.

- **GestorJuego**, que es una clase que gestiona cada ámbito del juego como: la información que sale en pantalla; el spawn y despawn de ratas; la acción de volver a empezar la escena; cambio de vista de cámara...


## Diseño de la solución

Lo que vamos a realizar para resolver esta práctica es...

El pseudocódigo del algoritmo de llegada utilizado es:
```
class Arrive:
    character: Kinematic
    target: Kinematic

    maxAcceleration: float
    maxSpeed: float

    # The radius for arriving at the target.
    targetRadius: float

    # The radius for beginning to slow down.
    slowRadius: float

    # The time over which to achieve target speed.
    timeToTarget: float = 0.1

    function getSteering() -> SteeringOutput:
        result = new SteeringOutput()

        # Get the direction to the target.
        direction = target.position - character.position
        distance = direction.length()

        # Check if we are there, return no steering.
        if distance < targetRadius:
            return null

        # If we are outside the slowRadius, then move at max speed.
        if distance > slowRadius:
            targetSpeed = maxSpeed
        # Otherwise calculate a scaled speed.
        else:
            targetSpeed = maxSpeed * distance / slowRadius

        # The target velocity combines speed and direction.
         targetVelocity = direction
        targetVelocity.normalize()
        targetVelocity *= targetSpeed

        # Acceleration tries to get to the target velocity.
        result.linear = targetVelocity - character.velocity
        result.linear /= timeToTarget

        # Check if the acceleration is too fast.
        if result.linear.length() > maxAcceleration:
            result.linear.normalize()
            result.linear *= maxAcceleration

        result.angular = 0
        return result
```
El script **"Llegada"** permite aumentar la aceleración lineal de un agente en dirección a otro según la distancia entre ambos.


El pseudocódigo del algoritmo de separación utilizado es:
```
class Separation:
    # Holds the kinematic data for the character
    character

    # Holds a list of potential targets
    targets

    # Holds the threshold to take action
    threshold

    # Holds the constant coefficient of decay for the
    # inverse square law force
    decayCoefficient

    # Holds the maximum acceleration of the character
    maxAcceleration

    # See the Implementation Notes for why we have two
    # getSteering methods
    def getSteering():

        # The steering variable holds the output
        steering = new Steering

        # Loop through each target
        for target in targets:

            # Check if the target is close
            direction = target.position - character.position
            distance = direction.length()
            if distance < threshold:

                # Calculate the strength of repulsion
                strength = min(decayCoefficient / (distance * distance),
                maxAcceleration)

                # Add the acceleration
                direction.normalize()
                steering.linear += strength * direction

         # We’ve gone through all targets, return the result
         return steering
```
El script **"Separación"** consigue que a una cierta distancia aplique un coeficiente de distancia que separa a unos agentes determinados.


El pseudocódigo del algoritmo de movimiento de persecución y huida es:
```
class Pursue (Seek):

 # Holds the maximum prediction time
 maxPrediction

 # OVERRIDES the target data in seek (in other words
 # this class has two bits of data called target:
 # Seek.target is the superclass target which
 # will be automatically calculated and shouldn’t
 # be set, and Pursue.target is the target we’re
 # pursuing).
 target

 # ... Other data is derived from the superclass ...

 def getSteering():

     # 1. Calculate the target to delegate to seek

     # Work out the distance to target
     direction = target.position - character.position
     distance = direction.length()

     # Work out our current speed
     speed = character.velocity.length()

     # Check if speed is too small to give a reasonable
     # prediction time
     if speed <= distance / maxPrediction:
     prediction = maxPrediction

     # Otherwise calculate the prediction time
     else:
     prediction = distance / speed

     # Put the target together
     Seek.target = explicitTarget
     Seek.target.position += target.velocity * prediction

     # 2. Delegate to seek
     return Seek.getSteering()
```
El script **"Persecución"** es igual al script "Llegada", pero con la diferencia de que multiplica la velocidad del agente como objetivo, sin cambiar la velocidad del agente.

El script **"Huir"** es igual al script "Persecución", pero en vez de seguir al agente asigando como objetivo, huye de él.ç

## Pruebas y métricas

- [Vídeo con la batería de pruebas](https://youtu.be/xxxxx)

## Ampliaciones

Se han realizado las siguientes ampliaciones

- Control del jugador por ratón haciendo uso de "raycast".
- Botón en la interfaz para añadir y destruir ratas.

## Producción

Las tareas se han realizado y el esfuerzo ha sido repartido entre los autores.

| Estado | Tarea | Fecha |
| :----: | :---: | :---: |
| ✔  | ReadMe: Preparación | 02-02-2023 |
| ✔  | Característica A: Controles ratón | 09-02-2023 |
| ✔  | Característica A: Tocar flauta | 09-02-2023 |
| ✔  | Característica D: Merodear | 09-02-2023 |
| ✔  | Característica E: Llegada V1 | 09-02-2023 |
| ✔  | Característica E: Llegada V2 | 09-02-2023 |
| ✔  | ReadMe: Scripts | 09-02-2023 |
| ✔  | Característica E: Separación | 16-02-2023 |
| ✔  | Característica E: Llegada refactorización | 16-02-2023 |
| ✔  | ReadMe: Pseudocódigo | 16-02-2023 |
| ✔  | Característica B: Persecución | 16-02-2023 |
| ✔  | Característica C: Huir | 16-02-2023 |
| ✔  | ReadMe: Update | 16-02-2023 |
| ✔  | Característica A: Botones + - ratas | 23-02-2023 |
| ✔  | Código: comentarios | 23-02-2023 |
| ✔  | ReadMe: FIN | 23-02-2023 |
| ✔  | Ejecutable y vídeo | 23-02-2023 |

## Referencias

Los recursos de terceros utilizados son de uso público.

- *AI for Games*, Ian Millington.
    - 3.3.8 "Pursue and Evade", 68.
    - 3.3.13 "Separation", 82.
- [Kaykit Medieval Builder Pack](https://kaylousberg.itch.io/kaykit-medieval-builder-pack)
- [Kaykit Dungeon](https://kaylousberg.itch.io/kaykit-dungeon)
- [Kaykit Animations](https://kaylousberg.itch.io/kaykit-animations)
