# 🛒 Ecommerce Factory IV

> Uma plataforma completa de comércio eletrônico focada em hardware e periféricos, desenvolvida com arquitetura moderna e boas práticas.

![Status](https://img.shields.io/badge/STATUS-CONCLUÍDO-brightgreen)
![NET](https://img.shields.io/badge/.NET-8-purple)
![Vue](https://img.shields.io/badge/Vue.js-3-4FC08D)

## 📸 Demonstração (Screenshots)

### 1. Página Inicial (Vitrine)
<img width="1406" height="696" alt="image" src="https://github.com/user-attachments/assets/37d67363-6a6a-4464-9c96-a8b33fe18dac" />


### 2. Detalhes do Produto
<img width="1406" height="696" alt="image" src="https://github.com/user-attachments/assets/cc3a4972-f528-40e3-a92e-ac09aed6efb9" />

### 3. Paginação de Produtos
<img width="1406" height="696" alt="image" src="https://github.com/user-attachments/assets/b70295ac-c439-491d-b404-fb4265a7a7fa" />

### 4. Carrinho de Compras
<img width="1406" height="696" alt="image" src="https://github.com/user-attachments/assets/8f048ddd-13d9-4665-ad64-23d53c924442" />


### 5. Painel Administrativo
<img width="1600" height="694" alt="image" src="https://github.com/user-attachments/assets/097f2267-d73f-49c5-b193-ad67a0b06cbb" />


---

## 🚀 Tecnologias Utilizadas

### Backend (API)
* **C# / .NET 8**: Core da aplicação.
* **Entity Framework**: ORM para gestão de dados.
* **xUnit**: Testes unitários (`Ecommerce.Tests`).

### Frontend (Client)
* **Vue.js 3**: Framework reativo.
* **Vite**: Build tool.
* **Axios**: Integração com a API.

---

## ⚙️ Funcionalidades

* [x] Catálogo de Produtos com filtros.
* [x] Carrinho de compras em tempo real.
* [x] Autenticação de Usuários.
* [x] Painel Admin (CRUD de Marcas, Categorias e Produtos).

---

## 📦 Como Rodar

1.  **Clone o repositório**
    ```bash
    git clone [https://github.com/takiguchii/ecommerce-project-factory-iv.git](https://github.com/takiguchii/ecommerce-project-factory-iv.git)
    ```

2.  **Backend**
    ```bash
    cd backend/Ecommerce
    dotnet restore
    dotnet ef database update
    dotnet run
    ```

3.  **Frontend**
    ```bash
    cd frontend/Ecommerce
    npm install
    npm run dev
    ```

