## Av_Suite - Suite for filescanning and packet sniffing[Upcoming!] ##

#Tech Stack : 
  • .NET 8,C#, abuse.ch for scanning file-hashes
  • WPF[Upcoming], cli 

#Key features
  • Protection againts zip attacks eg. zip-slip and, zip bomming.
  • File Scanning functionality

#Upcoming features
    • Determine File Signature for safer and more accurate file scanning.
    • Own MSSQL Database to store users and file signatures
    • Role-based access control (RBAC)

#Architecture

```
|———Clients
|      |——AvCli
|      |   |——Graphics/
|      |       |——UserPanel/
|      |          
|      |    
|      |——AvWPF    
|       
|———AvCore
|    |——Application/
|    |  |——DTOs/
|    |  |——Interfaces/
|    |  |——Services/
|    |——Domain/
|    |  |——Entities/
|    |  |   |——Policies/
|    |  |——Scans/
|    |——Infrastructure
|        |——Repositories/
|        |——Security/
|        |——Services/
|    
|    
|    
|
```
#Lessons learned
```
Through this project i have learned about security, about defenses and attacks that Hackers might use to harm someone or something.
Also this project has taught me a lot about safe file handling and more about data structures eg. The stack. 
```
    
    
