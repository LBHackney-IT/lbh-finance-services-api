.PHONY: setup
setup:
	docker compose build

.PHONY: build
build:
	docker compose build base-api

.PHONY: serve
serve:
	docker compose build base-api && docker compose up base-api

.PHONY: shell
shell:
	docker compose run base-api bash

.PHONY: clean
clean:
	docker compose stop finance-services-api-test && docker compose rm -f finance-services-api-test && docker rmi $$(docker images --filter "dangling=true" -q)

.PHONY: test
test:
	-docker compose build finance-services-api-test && docker compose run finance-services-api-test
	-make clean

.PHONY: lint
lint:
	-dotnet tool install -g dotnet-format
	dotnet tool update -g dotnet-format
	dotnet format

.PHONY: restart-db
restart-db:
	docker stop $$(docker ps -q --filter ancestor=test-database -a)
	-docker rm $$(docker ps -q --filter ancestor=test-database -a)
	docker rmi test-database
	docker compose up -d test-database
