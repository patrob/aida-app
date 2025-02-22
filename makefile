run:
	dotnet run --project src/Aida.Api/Aida.Api.csproj

up:
	docker compose up -d

down:
	docker compose down

reset:
	make down && make up