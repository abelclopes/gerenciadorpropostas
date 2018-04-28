# Cliente Gerenciador Propostas


## Development server
* cd cliente/
* iniciar o servidor use o comando `ng serve`. Navegar na url `http://localhost:4200/`.

## Build
* Iniciar o build `ng build`. o arquivo para distribuição fica no diretorio `dist/`. Use o parametro  `-prod` para iniciar o build de produção.

## Running unit tests
cd /server
* Iniciando testes `ng test` para executar os testes via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

executar `ng e2e` para executar os testes end-to-end via [Protractor](http://www.protractortest.org/).

## Further help

Para obter mais ajuda sobre o uso do CLI Angular `ng help` ou vá conferir o [Angular CLI README](https://github.com/angular/angular-cli/blob/master/README.md).




# swagger auto generater api

- download swagger-codegen-cli-2.3.1.jar (https://github.com/swagger-api/swagger-codegen)
- update the file path\to\SRC\API\UI\swagger.json
- go to path\to\SRC\API\UI\src\app\LegislacaoAPIClient
- java -jar D:\projetos\dotnetcore\swagger-codegen-cli-2.3.1.jar generate -i http://localhost:5000/swagger/v1/swagger.json -l typescript-angular
