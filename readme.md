# Todo Task

This project will auto migrate database on startup. 

Todo Model is like code below
```
public class Todo
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Title { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    public decimal CompletePercentage { get; set; }

    [Required]
    public string Status { get; set; }

    [Required]
    public DateTime ExpiredAt { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime UpdatedAt { get; set; }
}
```

## Usage

### API Endpoints:
```
GET : /api/todo
GET : /api/todo/{id}
GET : /api/todo/{expiredAt}
GET : /api/todo/{expiredFrom}/{expiredTo}
PUT : /api/todo/{id}
POST : /api/todo/{todo}
DELETE : /api/todo/{id}

```


### Get all todos

```
GET: /api/todo
```

### Get todo by id
Example using id = 1
```
GET: /api/todo/1
```

### Get todos by date
Example using expiredAt = 2020-04-05 
```
GET: /api/todo/2020-04-05
```

### Get todos by datetime range

Example using expiredFrom = 2020-04-05 and expiredTo = 2020-04-09
```
GET: /api/todo/2020-04-05/2020-04-09
```

### Delete todo by id
Example using id = 1
```
DELETE: /api/todo/1
```

### Create todo 
```
POST: /api/todo

Request data :
{
    "Title": "Development Logistic Phase 3",
    "Description": "Development Logistic Phase 3",
    "CompletePercentage": 20,
    "Status": "On Progress",
    "ExpiredAt" : "2020-05-20"
}
```
Value of Id is Auto Increment.

Value of CreatedAt and UpdatedAt is generated in controller

### Update todo by id

Example using id = 1
```
PUT: /api/todo/1

Basic update request data example:
{
    "Title": "Development Logistic Phase 3",
    "Description": "Development Logistic Phase 3",
    "CompletePercentage": 50,
    "Status": "On Progress",
    "ExpiredAt" : "2020-05-20"
}

Mark todo as done request data example:
{
    "CompletePercentage": 100
}

Or

{
    "Status": "Complete"
}

```
Update todo will update value of UpdatedAt with current date time

