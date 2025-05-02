# Ecommerce.Net

Project Title: Microservices-Based CRUD Application
Tech Stack: .NET Core, Docker, Apache Kafka, SQL Server, REST API, Entity Framework Core

Description:
Developed a basic microservices application that performs CRUD (Create, Read, Update, Delete) operations across independently deployed services using Docker containers and Apache Kafka for asynchronous communication.

Key Features:

    Microservices Architecture: Each module (e.g., User, Product, Order) was built as an independent service with its own database and API.

    Kafka Integration: Used Kafka to send and receive messages between services for operations like creating new records, updates, and deletion events.

    Containerization with Docker: Each microservice was containerized using Docker, enabling easy deployment and environment setup.

    Database-Per-Service: Used separate SQL Server databases for each microservice following microservices best practices.

    RESTful APIs: Built clean REST APIs for client interaction and service-to-service communication.

Role & Responsibilities:

    Implemented Kafka producers and consumers in each service for event publishing and listening.

    Developed CRUD endpoints and integrated with Entity Framework Core for data access.

    Wrote Dockerfiles for each service and configured Docker Compose for orchestration.

    Tested services individually and together to ensure smooth communication via Kafka.
