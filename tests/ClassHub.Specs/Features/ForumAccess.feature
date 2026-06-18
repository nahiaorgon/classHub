Feature: Control de acceso al foro por rol
  Como sistema ClassHub
  Quiero que las acciones del foro respeten el rol de cada usuario
  Para mantener la integridad del contenido académico

  Background:
    Given existe la unidad "Integración Continua" con id 1
    And existe el alumno "Lucía" con id 2
    And existe el profesor "Martín" con id 3

  Scenario: Solo un profesor puede fijar un hilo
    Given existe el hilo "Recursos del parcial" en la unidad 1 creado por el usuario 3
    When el profesor con id 3 fija el hilo
    Then el hilo "Recursos del parcial" está fijado

  Scenario: Un hilo fijado aparece antes que los no fijados
    Given existe el hilo "Hilo normal" en la unidad 1 creado por el usuario 2
    And existe el hilo "Hilo fijado" en la unidad 1 creado por el usuario 3
    When el profesor con id 3 fija el hilo "Hilo fijado" en la unidad 1
    Then el primer hilo de la unidad 1 es "Hilo fijado"

  Scenario: Un alumno puede crear hilos pero no fijarlos
    When el alumno con id 2 crea un hilo con título "Mi consulta" en la unidad 1
    Then el hilo "Mi consulta" existe en la unidad 1
    And el hilo "Mi consulta" no está fijado

  Scenario: Un alumno puede responder en cualquier hilo abierto
    Given existe el hilo "Consultas generales" en la unidad 1 creado por el usuario 3
    When el alumno con id 2 responde "Gracias por la info!" en el hilo "Consultas generales"
    Then el hilo "Consultas generales" tiene al menos 1 respuesta
