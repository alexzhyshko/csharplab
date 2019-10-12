# C# labs repos
Project has three separated projects, which are connected between each other by adding dependencies. 
- Library: main project, .NET Core 3.0, uses ***Library.Domain*** and ***Library.Model***
  - Contains: 
  1. Program.cs, 
  2. RentalController.cs
- Library.Domain: class library, .NET Standard Library, doesn't use any of the projects
  - Contains: 
  1. Person.cs, 
  2. Reader.cs, 
  3. Author.cs, 
  4. Admin.cs, 
  5. Book.cs
- Library.Model: emulator of DB model type, .NET Standard Library, uses ***Library.Domain***
  - Contains: 
  1. AdminModel.cs, 
  2. BookModel.cs, 
  3. OptionalGuid.cs, 
  4. ReaderModel.cs, 
  5. RentalModel.cs
