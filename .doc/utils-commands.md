```
docker compose -f docker-compose.yml up -d --force-recreate     

// adicionar comando no arquivo ~/.bashrc com alias 'docker-reset'
docker stop $(docker ps -a -q) 2>/dev/null; docker rm $(docker ps -a -q) 2>/dev/null; docker rmi -f $(docker images -aq) 2>/dev/null; docker system prune -a -f; docker volume prune -a -f

docker stop auth_api 2>/dev/null; docker rm auth_api 2>/dev/null; docker rmi -f dotnet/aspnet:8.0 2>/dev/null    
docker compose -f docker-compose.yml up -d --force-recreate auth_api     

```

```
docker login 
docker logout      
```

```
docker build -t auth-api-image -f Dockerfile .     
docker run -p 5300:8080 auth-api-image  
```  

```
docker pull diegoferreirax/auth-api:xx     
docker run -p 5000:8080 diegoferreirax/auth-api:xx 
```  

```
dotnet ef migrations add InitialCreate -- Dev            
dotnet ef database update     
```

```
docker tag abc95440395b diegoferreirax/auth-api-github:1    
docker push diegoferreirax/auth-api-github:1    
```