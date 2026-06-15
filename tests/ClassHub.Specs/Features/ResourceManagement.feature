Feature: Gestión de recursos por unidad
  Como profesor de ClassHub
  Quiero poder subir recursos organizados por unidad
  Para que los alumnos encuentren el material fácilmente

  Background:
    Given existe la unidad "Control de Versiones" con id 2
    And existe el profesor "Ana" con id 10

  Scenario: Un profesor agrega un link a una unidad
    When el profesor con id 10 agrega el link "https://git-scm.com/doc" con título "Git Docs" a la unidad 2
    Then la unidad 2 tiene 1 recurso de tipo Link

  Scenario: Un profesor agrega un PDF a una unidad
    When el profesor con id 10 agrega el archivo "git_intro.pdf" con título "Intro a Git" a la unidad 2
    Then la unidad 2 tiene 1 recurso de tipo Pdf

  Scenario: Los recursos de una unidad no aparecen en otra
    Given la unidad 2 tiene el link "https://git-scm.com" con título "Git"
    And existe la unidad "Build Automatizada" con id 3
    When se consultan los recursos de la unidad 3
    Then el resultado está vacío
