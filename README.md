<p align="center">
 <img width="1049" height="587" alt="image" src="https://github.com/user-attachments/assets/a05be72f-3e4e-4ad3-8bf3-1750391d8dca" />
</p>

# 🛡️ NOSBOR Security System

<p align="center">
  <img src="https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white" alt="C#">
  <img src="https://img.shields.io/badge/.NET-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET">
  <img src="https://img.shields.io/badge/UI-Ubuntu_Design-E95420?style=for-the-badge&logo=ubuntu&logoColor=white" alt="Ubuntu UI">
</p>

---

## 📝 Sobre o Projeto
O **NOSBOR Security System** é uma solução de antivírus leve e customizada desenvolvida em C# (Windows Forms). O projeto foca na detecção de ameaças através de verificação de integridade por Hash (SHA256) e análise heurística básica, apresentando uma interface moderna inspirada na estética do sistema operacional Ubuntu.

---

### 🚀 Funcionalidades Principais
* **Varredura Personalizada:** Seleção manual de pastas ou definição de um diretório alvo fixo para análises recorrentes.
* **Motor de Detecção por Hash:** Identificação precisa de malwares conhecidos utilizando um banco de assinaturas (`signatures.json`).
* **Gestão de Ameaças:** Painel de decisão para mover arquivos suspeitos para a **Quarentena** ou realizar a **Exclusão Permanente**.
* **Interface Dark Mode:** Layout otimizado com logs em tempo real, barra de progresso e scroll horizontal para caminhos longos.
* **Controle de Fluxo:** Funções de Pausar/Retomar e exportação automática de relatórios de varredura em `.txt`.
* **Segurança de Execução:** Tratamento de permissões para evitar erros de acesso em pastas protegidas do Windows.

---

### 🛠 Tecnologias Utilizadas
| Tecnologia | Descrição |
| :--- | :--- |
| **C# / .NET** | Linguagem principal e framework de desenvolvimento. |
| **WinForms** | Criação da interface gráfica (GUI). |
| **SHA256** | Algoritmo de criptografia para geração de hashes únicos. |
| **JSON** | Armazenamento e leitura do banco de assinaturas. |

---

### 📋 Como Utilizar

> [!IMPORTANT]
> **Privilégios Elevados:** Para escanear diretórios do sistema (como `C:\Program Files`), execute o aplicativo como **Administrador**.

1.  **Clone o repositório:**
    ```bash
    git clone [https://github.com/SEU_USUARIO/Robson-Security.git](https://github.com/SEU_USUARIO/Robson-Security.git)
    ```
2.  **Configuração:** Verifique se o arquivo `signatures.json` está na mesma pasta do executável (`/bin/Debug`).
3.  **Execução:** Compile o projeto no Visual Studio e inicie a varredura.

---

### 📷 Screenshots
<div align="center">
  <table>
    <tr>
      <td>      <img width="663" height="711" alt="image" src="https://github.com/user-attachments/assets/b4aaea22-bc91-4ddd-8488-ead27b428c1e" /><br>Painel Principal</td>
      <td><img width="899" height="741" alt="image" src="https://github.com/user-attachments/assets/8775bbf1-6783-418a-94f3-62d54ec250c4" />
<br>Ações de Ameaça</td>
    </tr>
  </table>
</div>

---

<p align="center">
  Desenvolvido com ❤️ por Robson
</p>
