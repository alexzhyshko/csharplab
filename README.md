# C# labs repos
Solution has three separated projects, which are connected between each other by adding dependencies. 
- Library: main project, **`.NET Core 3.0`**, uses `Library.Domain` and `Library.Model`
------
  #### Contains: 
  ```
  Program.cs
  RentalController.cs
  ```
------
- Library.Model: emulator of DB model type, **`.NET Standard Library`**, uses `Library.Domain`
  #### Contains: 
  ```
  AdminModel.cs
  BookModel.cs
  OptionalGuid.cs
  ReaderModel.cs
  RentalModel.cs
  ```
- Library.Domain: class library, **`.NET Standard Library`**, * *doesn't use any of the projects* *
  #### Contains: 
  ```
  Person.cs
  Reader.cs
  Author.cs
  Admin.cs
  Book.cs
  ```
