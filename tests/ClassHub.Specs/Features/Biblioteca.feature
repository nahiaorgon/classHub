Feature: Gestión de la biblioteca
  Como profesor de ClassHub
  Quiero poder subir y eliminar libros en la biblioteca
  Para que los alumnos tengan acceso al material del programa

  Background:
    Given existe el profesor "Ana" con id 10
    And existe el alumno "Pedro" con id 20

  Scenario: Un profesor agrega un libro
    When el profesor con id 10 agrega el libro "Clean Code" de "Robert C. Martin" con archivo "clean_code.pdf"
    Then la biblioteca tiene 1 libro con título "Clean Code"

  Scenario: La biblioteca lista todos los libros disponibles
    Given el profesor con id 10 agrega el libro "Clean Code" de "Robert C. Martin" con archivo "clean_code.pdf"
    And el profesor con id 10 agrega el libro "The Pragmatic Programmer" de "Hunt & Thomas" con archivo "pragmatic.pdf"
    When se consultan todos los libros
    Then el resultado contiene 2 libros

  Scenario: Un profesor elimina un libro
    Given el profesor con id 10 agrega el libro "Clean Code" de "Robert C. Martin" con archivo "clean_code.pdf"
    When el profesor elimina el libro "Clean Code"
    Then la biblioteca tiene 0 libros

  Scenario: No se puede agregar un libro sin título
    When el profesor con id 10 agrega el libro "" de "Algún Autor" con archivo "libro.pdf"
    Then el libro no se agrega a la biblioteca
