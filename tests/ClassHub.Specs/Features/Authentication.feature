Feature: Autenticación de usuarios
  Como usuario de ClassHub
  Quiero poder iniciar y cerrar sesión con mis credenciales institucionales
  Para acceder al contenido según mi rol

  Background:
    Given existe el alumno "Laura" con email "laura@classhub.com" y contraseña "alumno123"
    And existe el profesor "Carlos" con email "carlos@classhub.com" y contraseña "prof456"

  Scenario: Un alumno inicia sesión con credenciales correctas
    When se validan las credenciales "laura@classhub.com" y "alumno123"
    Then el usuario retornado es "Laura" con rol Alumno

  Scenario: Un profesor inicia sesión con credenciales correctas
    When se validan las credenciales "carlos@classhub.com" y "prof456"
    Then el usuario retornado es "Carlos" con rol Profesor

  Scenario: Credenciales incorrectas devuelven null
    When se validan las credenciales "laura@classhub.com" y "wrongpass"
    Then el resultado de validación es nulo

  Scenario: Email inexistente devuelve null
    When se validan las credenciales "noexiste@classhub.com" y "cualquiera"
    Then el resultado de validación es nulo

  Scenario: Un alumno no puede acceder a funciones de profesor
    When se validan las credenciales "laura@classhub.com" y "alumno123"
    Then el usuario retornado NO tiene el rol Profesor
