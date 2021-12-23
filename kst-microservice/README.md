## Running application in docker containers:
#### Using Docker CLI
```
docker network ls
docker network create --driver bridge micro_network (skip if already created)
docker build -t kst-srv .
docker run -p 5003:5003 --detach --name kst-service --net=micro_network order-srv
```