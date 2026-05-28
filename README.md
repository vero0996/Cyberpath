# Cyberpath
Explicacion del funcionamiento del videojuego

# Enemigos
Los enemigos siguen un camino definido por waypoints predefinido que el spawner le asigna al crear cada enemigo. los enemigos tienen 2 scripts:

Virus
- Aqui se define como los enemigos obtienen la ubicacion de los waypoints vinculados a path. Si un enemigo detecta una defensa, este se sale del camino y va directo a la defensa para atacarla hasta que esta es destruida. Una vez destruida la defensa los enemigos vuelven al camino buscando el waypoint mas cercano a ellos.
- En los atributos del enemigo se puede asignar su velocidad de movimiento, el daño ocasionado al jugador al llegar al final del camino y el rango de deteccion de defensas.
Damage
-En este script se define tanto como el enemigo daña las defensas y con que frecuencia puede atacar, como el sistema de vida del enemigo y como este puede ser dañado por las defensas, y cuanto dineero recibe el jugador por derrotarlo.
- Se le puede asignar los atributos de daño a defensas, frecuencia de ataque, puntos de vida del enemigo y cuantas monedas le da al jugador.

## Jugador
El jugador es la manera en la que el usuario puede interactuar con el juego, en su codigo se define su sistema de vida, su velocidad y si esta vivo o no. 
Este puede colocar defensas en las zonas verdes(zonas de construccion) presionando la E si esta lo suficientemente cerca a la zona. 

En sus atributos se puede asignar su velocidad, vida maxima, y se puede observar su vida actual y si esta vivo o no.

## Spawner
El spawner es la manera en la que aparecen los enemigos y se asignan las waves. Actualemente funciona de la siguiente manera:
- Se le debe asignar el objeto Path para poder asignarle el camino de Waypoints a los enemigos.
- Se asigna el numero de waves que se quiera
- Se puede asignar el numero de enemigos unicos que pueden aparecer por wave
- Por enemigo se puede definir la cantidad de enemigos de ese tipo que van a aparecer y la frecuencia con la que aparecen.
- Una vez se llega a la mitad de los enemigos la frecuencia se reduce a la mitad.
- Se puede definir el tiempo entre waves una vez termine la wave configurada y cuanto tiempo tiene de retraso la wave que esta ciendo configurada una vez empezada la ronda
El sistema de Waves actualmente funciona como stacks first in first out, donde dependiendo del orden en el que se coloquen los enemigos en la configuracion de cada wave, spawnearan TODOS los enemigos del mismo tipo antes de que puedan aparecer los del siguiente tipo.

## Path
Este objeto tiene el codigo path, el cual revisa la cantidad de hijos que tiene el objeto para definir la cantidad de waypoints que existen en el camino fijo a seguir por los enemigos y que asi el Spawner les pueda asignar dicho camino.

## DeathZone
Es Donde el jugador pierde vida si el enemigo llega al final del camino y posteriormente son destruidos.

## Build zone
Estan dentro del objeto Casillas, donde cada uno tienen el codigo Build Zone. En este codigo se evalua si el jugador esta lo suficientemente cerca para poder interactuar, cambiando de color para indicar que es interactuable y asi permitir al jugador colocar defensas llamando a BuildManager en el LevelManager.

## Level Manager
Este objeto tiene las funciones generales del videojuego. Se compone de 2 scripts:
LevelManager
- Este codigo define como el jugador puede volver al menu desde el menu de pausa y es como se le asigna al jugador el dinero, pudiendo agregar dinero al derrotar un enemigo, y quitar cuando se construlle una defensa. Se le puede asignar en los atributos una cantidad de dinero inicial.
BuildManger
- Este es el codigo que permite al jugador construir defensas en la Build zone, teniendo que asignarle los diferentes tipos de defensas que existen para que este los guarde en una lista que sera llamada por la tienda para definir cual es la defensa que se desea construir. En los atributos se debe asignar cuanto cuesta construir cada defensa.

## Defensas
Estas son las que atacana a los enemigos gracias a la bala. Las defensas solo se pueden colocar en las BuildZone gracias al BuildManager si el jugador tiene el suficiente dinero. Estas constantemente estan revisando si hay algun enemigo en rango para atacar con la etiqueta de layer "Enemigos". Tienen un objeto hijo llamado firepoint que es donde aparece la bala cuando se llama a la instruccion "Disparar".

A cada defensa se le debe asignar lo siguiente en los atributos:
- El prefab de la bala
- El objeto firepoint
- Que mascara va a buscar en los objetos para atacarlos (Enemigos en este caso).
- La vida maxima de la defensa.
- El rango de ataque y la frecuencia de ataque
El daño a los enemigos se define en la bala

## Bala 
Esta es quien ataca a los enemigos cuando una defensa los detecta. La bala sigue de manera teledirigida al enemigo que persigue. Si este es elimnado por otra bala, la bala se destruye para evitar que quede flotando infinitamente. La velocidad de la bala debe ser alta para que no se quede persiguiendo a los enemigos y que simplemente les pegue una vez sale de la defensa.

Se le debe asignar en atributos: su propio RigidBody, la velocidad a la que va y cuanto daño le hace a los enemigos.

## Menu
Esta esta como hijo de canvas. Tiene el codigo Tienda y es donde se crea la interfas de la tienda como objetos hijos de menu.
El codigo Tienda funciona de la siguiente manera:
- Tienda llama al level Manger para obtener las monedas actuales del jugador y asi el objeto hijo de este muestre cuantas monedas tiene el jugador.
- Tambien llama al BuildManager para saber el arreglo de las defensas y asi determinar cual defensa es que el jugador quiere construir.
- Solamente sele debe asignar el objeto textmesh que muestra las monedas actuales del jugador.

Menu tiene como hijos los objetos que forman la interfaz de la tienda, destacando el objeto llamado tienda que tiene como objetos hijos a los botones que definen que tipo de defensa quiere construir el jugador, teniendo que asignarle que por click llame a la funcion de Builmanager SetSelectedDefensa, asignandole el objeto levelmanger y escribiendo el numero al que pertenece la defensa en el arreglo de defensas para asi el boton saber que defensa es que el jugador quiere comprar.
EL arreglo de las defensas inicia en 0, por lo que la primera defensa asignada al levelmanger en el boton se le asigna como 0.


