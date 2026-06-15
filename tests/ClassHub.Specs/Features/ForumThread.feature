Feature: Gestión de hilos en el foro
  Como usuario de ClassHub
  Quiero poder crear y participar en hilos de discusión por unidad
  Para que la información no se pierda como pasa en Discord

  Background:
    Given existe la unidad "Integración Continua" con id 1
    And existe el alumno "Lucía" con id 2
    And existe el profesor "Martín" con id 3

  Scenario: Un alumno crea un nuevo hilo
    When el alumno con id 2 crea un hilo con título "¿Cuál es la diferencia entre IC e ID?" en la unidad 1
    Then el hilo "¿Cuál es la diferencia entre IC e ID?" existe en la unidad 1

  Scenario: Un alumno responde en un hilo
    Given existe el hilo "¿Cuál es la diferencia entre IC e ID?" en la unidad 1 creado por el usuario 2
    When el alumno con id 2 responde "IC integra código frecuentemente; ID lleva eso hasta producción."
    Then el hilo tiene 1 respuesta

  Scenario: Un profesor fija un hilo importante
    Given existe el hilo "Recursos del parcial" en la unidad 1 creado por el usuario 3
    When el profesor con id 3 fija el hilo
    Then el hilo "Recursos del parcial" aparece primero en la lista de la unidad 1

  Scenario: Un hilo vacío no se puede crear
    When el alumno con id 2 crea un hilo con título "" en la unidad 1
    Then el hilo no se crea
