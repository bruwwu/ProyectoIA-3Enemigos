# ProyectoIA-3Enemigos
 Hacer un mini juego de tipo Binding of Isaac (vista 2D desde arriba) (ejemplo https://youtu.be/PXD1kIZr9iA?si=wojZEEI4q-1rNe6z&t=181) que tenga: 
 Un personaje jugable, controlable por el jugador:
 A) Que se pueda mover de arriba a abajo y de izquierda a derecha, y las diagonales. 
 B) Debe voltear hacia la dirección que se esté moviendo.
 C) Que pueda disparar hacia la dirección en la que está volteando (por ahora con eso basta) al presionar un botón/tecla.
 D) Que la bala se vea en la escena y detecta la colisión de manera adecuada con los enemigos y con los obstáculos y paredes del nivel. 
 E) Cuando una bala choca, se destruye. (o se desactiva, para poder reutilizarla en otro disparo, sin tener que crear otro gameobject). Cualquiera de las dos soluciones tendrá el mismo valor de calificación. 
 F) Que tenga HP, y si su HP llega a 0 o menos, se muera.
 G) Para la parte visual, sí pueden usar dibujos prehechos, modelos 3D (por ejemplo, de Mixamo https://www.youtube.com/playlist?list=PLwyUzJb_FNeTQwyGujWRLqnfKpV-cj-eO), o, en el peor de los casos, un mono tipo el Amogus que usamos en clase.
 
 Descripción del cuarto (área de combate) y escenario en general:
 A) Tiene que tener muros de los cuales no puedan pasar ni el jugador ni los enemigos. Tal cual chocan y no pueden seguir hacia esa dirección. 
 B) Tiene que tener al menos unos cuantos obstáculos, para que el enemigo escapista pueda hacer obstacle avoidance. Tip: Recuerden usar Layers para identificar los obstáculos, players, enemigos, balas y paredes fácilmente en los OnTrigger y OnCollision Enter/Stay/Exit.
 C) Puntos extra: Que haya algún tipo de obstáculo que se pueda destruir con balas tras X daño. 
 D) Puede ser todo en 3D o todo en 2D o una mezcla de ambos. Como les convenga o se las haga más bonito o más adecuado a los assets que encontraron. Con que puedan tener la estructura similar al de binding of isaac, con eso me basta. 
 E) Prohibido usar NavigationMesh ahorita. Lo vamos a ver el parcial que viene.      
 
 Base enemy: Todos los enemigos tienen en común lo siguiente, por lo que les recomiendo que hagan una clase base de enemigo y de ahí hereden los otros 3 tipos de enemigos: 
 A) Tienen una vida/HP, la cual se reduce cuando se les hace daño. Si llega a 0 o menos, mueren. 
 B) Todos le hacen daño al personaje jugable si los toca. 
 C) Puntos extra: el enemigo que sufre daño brilla poquito de color rojo por un instante tras sufrir el daño.
 D) Puntos extra: si le ponen una animación o efecto de morir a cada personaje. 
 E) Ninguno de los enemigos tiene que girar o tener animaciones de giro ni nada por el estilo, pueden estar siempre volteando al frente como en Binding of Isaac o similares. Pero si se los añaden, son puntos extra.
