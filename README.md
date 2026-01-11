# Chat P2P em C#
Uma aplicação de chat peer-to-peer simples desenvolvida em C# para comunicação direta entre dois usuários através da rede local.
📋 Sobre o Projeto
Este é um chat de console que permite comunicação em tempo real entre duas pessoas sem necessidade de servidor dedicado. Um usuário cria uma "sala" (atua como servidor) e o outro se conecta a ela (atua como cliente).
✨ Funcionalidades

Comunicação P2P (peer-to-peer) direta
Detecção automática de IP local
Mensagens com timestamp
Conexão via rede local ou localhost
Interface simples de console
Threads separadas para envio e recebimento de mensagens

🚀 Como Usar
Pré-requisitos

.NET 6.0 ou superior
Duas máquinas na mesma rede (ou use localhost para teste)

Executando a Aplicação

Usuário 1 (Criador da Sala)

   Executar a aplicação
   Escolher opção: 1
   Anotar o IP exibido
   Aguardar conexão

Usuário 2 (Convidado)

   Executar a aplicação
   Escolher opção: 2
   Digitar o IP fornecido pelo Usuário 1
   Começar a conversar
Comandos

Digite suas mensagens normalmente e pressione Enter
Digite sair para encerrar a conexão
Para teste local, use localhost ou 127.0.0.1 como IP

🔧 Detalhes Técnicos
Porta Utilizada

Porta: 5000 (TCP)

Tecnologias

System.Net.Sockets - Para comunicação TCP/IP
Threading - Para recebimento assíncrono de mensagens
NetworkStream - Para transmissão de dados

Arquitetura
Servidor (Sala)          Cliente (Convidado)
     │                          │
     ├─ Aguarda conexão         │
     │  (porta 5000)            │
     │                          ├─ Conecta ao IP:5000
     │◄─────────────────────────┤
     │                          │
     ├─ Thread Receiver   ◄───► ├─ Thread Receiver
     ├─ Thread Sender     ◄───► ├─ Thread Sender
     │                          │
📝 Estrutura do Código

StartServer() - Inicializa o servidor e aguarda conexões
StartClient() - Conecta-se a um servidor existente
ReceiverMessage() - Thread dedicada para receber mensagens
SendMessage() - Thread dedicada para enviar mensagens
GetLocalIP() - Obtém o endereço IP local da máquina

⚠️ Considerações de Segurança

Este é um projeto educacional/demonstrativo
Não possui criptografia de mensagens
Não possui autenticação de usuários
Recomendado apenas para redes confiáveis

🔒 Firewall
Certifique-se de que a porta 5000 está liberada no firewall para permitir conexões.
Windows:
netsh advfirewall firewall add rule name="Chat P2P" dir=in action=allow protocol=TCP localport=5000
🐛 Solução de Problemas
ProblemaSolução"Erro de conexão"Verifique se o IP está correto e se ambos estão na mesma rede"Porta em uso"Feche outras instâncias da aplicação ou altere a porta no código"Firewall bloqueando"Libere a porta 5000 nas configurações do firewall
