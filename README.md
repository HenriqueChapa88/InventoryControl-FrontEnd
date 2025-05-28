# InventoryControl - Backend
Projeto com backend implementado

  O Inventory Control System é uma solução robusta para gestão de estoques desenvolvida em ASP.NET Core, seguindo os princípios da arquitetura em camadas. O backend foi implementado oferecendo endpoints RESTful para todas as operações de CRUD de itens e fornecedores, além de funcionalidades específicas como controle de movimentação de estoque, alertas de baixo estoque. A segurança é feita por um sistema de autenticação JWT com hash de senhas BCrypt e autorização baseada em roles (Admin, Manager, User), enquanto a integridade dos dados é mantida através de validações de domínio rigorosas para CNPJ, SKU e regras de negócio. Essa aplicação utiliza Entity Framework Core para persistência em SQL Server com migrações automatizadas, padrão Repository para isolamento do acesso a dados, e tratamento centralizado de exceções com respostas padronizadas.

