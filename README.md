# Incrementer

This demo application increments a number and stores it in a database in a scalable architecture.

## Installation and use

`git pull` this repo

`cd Incrementer/App`

`docker-compose up --build`

This will take a while, especially the first time when the images download.

When `docker-compose` is running, you will see errors as the app tries to connect to RabbitMQ.
When you see these two services (`api`, `incrementer`) reconnecting after the service `rabbit`, the app is running:

![image](https://user-images.githubusercontent.com/10968503/111924779-86f96080-8a7c-11eb-8545-b8222008ed18.png)

To test that the load balancer is up, go to `http://localhost:3333` in a browser and you should see:

![image](https://user-images.githubusercontent.com/10968503/111924837-b60fd200-8a7c-11eb-9b68-18505aea2ea0.png)

To use the app:

`POST: http://localhost:3333/api/Increment`
Body:
`
  {
    "Key":string,
    "Value":number
}
`

![image](https://user-images.githubusercontent.com/10968503/111924872-e48dad00-8a7c-11eb-8ffc-d45d7010a3f5.png)

`GET: http://localhost:3333/api/Increment/{key}`

![image](https://user-images.githubusercontent.com/10968503/111924956-42ba9000-8a7d-11eb-821c-7bb890569aa1.png)

To shut down, `CTRL-C` in the `docker-compose` window and then `docker-compose down`

If you want to delete all images from your machine, `docker system prune -a`. Be careful.

## Running Tests

You can run the test project only if you have the .NET 5 SDK on your machine, which you can find [here](https://dotnet.microsoft.com/download/dotnet).

Once you have that, to run the tests do the following:

`cd ../LoadBalancer.Tests`
`dotnet test -v normal`

![image](https://user-images.githubusercontent.com/10968503/111925162-12272600-8a7e-11eb-8f97-e24a535f6a49.png)




