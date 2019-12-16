# C# labs repos


Console Application based on **`.NET Core 3.0`**.
Solution has three separated projects, which are connected between each other by adding dependencies.


------
- Library: main project, **`.NET Core 3.0`**, uses `Library.Domain` and `Library.Managers`
  #### Contains: 
  ```
  Program.cs
  ```
------
- Library.Application: controller for whole program, **`.NET Core 3.0`**, uses `Library.Domain` and `Library.Managers`
  #### Contains: 
  ```
  RentalController.cs
  ```
------
- Library.Managers: DB access classes, **`.NET Standard Library 2.0`**, uses `Library.Domain`
  #### Contains: 
  ```
  AdminManager.cs
  BookManager.cs
  OptionalGuid.cs
  ReaderManagerl.cs
  RentalManager.cs
  ```
------
- Library.Domain: class library, **`.NET Standard Library 2.0`**, *doesn't use any of the projects*
  #### Contains: 
  ```
  Person.cs
  Reader.cs
  Author.cs
  Admin.cs
  Book.cs
  ```
------
