## EntityDbExtensions
### Simplify Entity Framework Delete Children Operations


## Overview

Tired of manual and repetitive code when it comes to deleting child and subsequent objects with validations and nested loops? 
The EntityDbExtensions library simplifies the process for you. 
By simply invoking the method UpdateAndHandleDeletedChildren in the .Update of the root class, it will automatically handle the recursive deletion of all child, grandchild, and subsequent objects that have been removed from your root class.

The method will inspect classes containing a virtual child property with a List type, such as IEnumerable<YourClass>, ICollection<YourClass> or List<YourClass>. 
```bash
 public virtual IEnumerable<YourChild> YourChild { get; private set; }
```
It performs a validation check to determine if the object has been deleted, marking it as removed. 
This marking signals Entity Framework to handle the deletion seamlessly. 
The validation process is recursive, ensuring that all child and subsequent objects undergo thorough validation for removal.


## Features

- **Effortless Database Operations:** Streamline your Entity Framework database operations with simplified methods.

- **Extension Methods for DbContext:** Easily perform common database operations using intuitive extension methods on your DbContexts.

- **Update and Handle Deleted Children:** Handle the validation and removal of all deleted children from your root class with just a single command.


## Getting Started

1. **Installation:**
   - Install the EntityDbExtensions NuGet package in your .NET project.

```bash
<PackageReference Include="EntityDbExtensions" Version="1.2.0" />
```

Usage:
Add the EntityDbExtensions using statement to your code.
Start using the extension methods on your Entity Framework entities.

```bash
using EntityDbExtensions;

 public async Task<bool> Update(YourEntity entity)
 {
    var entityDb = await Get(entity.Id);
    if (entityDb == null) throw new InvalidOperationException($"{nameof(YourEntity)} not found.");
	
    await _yourDbContext.UpdateAndHandleDeletedChildren(entity, entityDb);  // Here the magic happens.

    return await _yourDbContext.SaveChangesAsync() > 0;
 }
```


#### Contribution
Contributions are welcome! If you find any issues or have suggestions for improvements, feel free to open an issue or create a pull request.

#### License	
This project is licensed under the MIT License.



LinkedIn - ( https://www.linkedin.com/in/alan-evandro-barboza/ )
