# deg-toolbelt

## Usage

```
> deg generate-classes -d test -n Deg.Models -t Customer
```

This generates a class from Customer table from test database with Deg.Models namespace. When there's no argument -t, it will generate all the tables into classes for that database. No argument -n means it'll use the database name as namespace of the class.

Note: Please note that it only supports SQL server for now and the generated classes are C#.
