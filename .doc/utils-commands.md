```
docker compose -f docker-compose.yml up -d --force-recreate     

docker stop $(docker ps -a -q)     
docker rm $(docker ps -a -q)     
docker rmi -f $(docker images -aq)     
docker system prune -a     
docker volume prune -a     

docker compose -f docker-compose.yml up -d --force-recreate auth_api     
docker stop auth_api     
docker rm auth_api     
docker rmi -f dotnet/aspnet:8.0     
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