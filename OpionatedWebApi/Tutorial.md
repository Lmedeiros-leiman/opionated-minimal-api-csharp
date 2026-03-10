# Tutorial a projeto organizado C# .net 10.

Codar não é o desafio, é codar algo que não vai te fazer querer pular pela janela.
então organizei este projeto para ajudar com essa tarefa.

A ideia é ter um projeto que esta receptivo a código separado por funcionalidade, focando nas endpoints a serem criadas e interfaces com o EF. 

ATENÇÃO: se tu ta fazendo AOT, talves vala mais a pena criar um DTO compartilhado por endpoint. pois vai precisar dizer que a classe é serializavel pro `AppJsonSerializerContext.cs`

MAS o template se mantem limpo o suficiente para quem quiser seguir com outra estrutura: que siga com força.