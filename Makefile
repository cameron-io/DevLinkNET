
.PHONY: build
build: env
	dotnet build

.PHONY: env
env:
	cp .env.tmpl .env

.PHONY: dev
dev:
	dotnet watch --project API --no-hot-reload

.PHONY: migrate
migrate:
	rm -f Infrastructure/Data/Migrations/*.cs
	dotnet ef migrations add InitialCreate \
		-s API \
		-p Infrastructure \
		-o Data/Migrations
	dotnet ef --project API database update
