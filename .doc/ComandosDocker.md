cd ../../mnt/c/Users/diego.xavier/Documents/1\ -\ Dev/zPessoal/auth-api/AuthApi/src/AuthApi.WebApi

-----

docker compose -f docker-compose.yml up -d --force-recreate

docker stop $(docker ps -a -q)
docker rm $(docker ps -a -q)
docker rmi -f $(docker images -aq)
docker system prune -a
docker volume prune -a

----- 

docker login 
docker logout  

----- LOCAL

docker build -t auth-api-image -f Dockerfile .
docker run -p 5300:8080 auth-api-image

----- IMAGEM DOCKERHUB

docker pull diegoferreirax/auth-api:xx
docker run -p 5000:8080 diegoferreirax/auth-api:xx