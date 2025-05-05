🚀 Demonstration of a modern microservices architecture, separated by responsibilities with secure data access!

Recently, I developed a functional demo application for user management, showcasing how an enterprise-grade system can be structured by following modern architecture best practices.

🔧 Key technologies and architectural patterns used:
🧱 Independent microservices, each with a clear responsibility
 🧠 Domain-Driven Design (DDD) and CQRS to separate commands from queries
 📨 RabbitMQ as a message broker for asynchronous communication between services
 🛡️ JWT with Refresh Tokens for secure and scalable authentication
 🗄️ Multiple separated databases to ensure modularity, data isolation, and controlled access

🔐 Communication structure:
The AuthService handles user login and registration using a dedicated database for credentials. Upon successful login, it generates a JWT token.
 The UserManagementService verifies the token and grants access to a separate database for managing user details and operations.
🎯 This demonstration reflects a clean microservices separation and secure multi-database integration within a distributed, scalable, and well-organized system — aligned with the requirements of modern enterprise applications.
📎 The project is open-source on GitHub:
 🔗 https://lnkd.in/ddsTe3WM
✨ Feel free to check it out and follow me on GitHub for more projects!
 🔔 github.com/erhankrasniqi
