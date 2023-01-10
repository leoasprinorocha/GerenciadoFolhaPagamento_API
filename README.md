# GerenciadorFolhaPagamento_API

A solução do Gerenciador de Folha de Pagamentos consiste em duas aplicações, uma web api em .NET 5 e uma frontend também em .NET5 porém arquitetura MVC.

![image](https://user-images.githubusercontent.com/45474333/211453816-5d4222e5-0081-487f-ac05-ee7a3a96e146.png)


O desenvolvimento da API foi baseado na abordagem de Clean Archicteture, segmentando a aplicação em camadas com os seguintes papeis:

> API_Presentation: Camada mais externa da API, responsável por receber as requisições e devolver os dados solicitados.

> API_Data: Camada responsável por toda a comunicação com o banco de dados, tratando apenas de receber as entidades devidamente validadas para persistência destas, utilizando-se do padrão UnitOfWork para tornar as sessões transacionadas.

> API_Domain: Núcleo mais importante da solução pois define todas as regras de negócio das entidades pertencentes ao sistema. Essa camada não possuí depedência externa alguma, e abrange todas as interfaces, builders, ViewModels/Dtos, enumadores da solução. Dentro desta camada ainda existe um projeto de teste cobrindo algumas regras de negócio mais complexas das entidades.

> API_Application: A camada Application podemos fazer uma analogia ao papel da camada viewmodel em uma arquitetura MVVM, porque esta trata da interligação da camada de apresentação com a camada de dados. Nela também são usadas as regras de negócio do sistema para construir os objetos de maneira consistente para a camada de repositório.

>API_Infrastructure: Confesso que a primeira vez que implemento essa camada em alguma aplicação, estava estudando recentemente o papel dela e não foi codificado muitas coisas nessa camada. Apenas contém a implementação do padrão UnitOfWork tratando das sessões de acesso ao banco pela API.
