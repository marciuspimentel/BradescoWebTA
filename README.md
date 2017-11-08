# BradescoWebTA
Repositorio que contem arquivos que facilitam o processo de Integração para Transmissão de Arquivos de CNAB e CNAB400 do Banco Bradesco.

No repositorio existem 2 projetos 1 em Java e outro em C#.

No projeto em Java criei 2 arquivos .JAR, um que realiza o Encrypt dos Arquivos para envio e outro que Realiza o Decrypt dos arquivos de Retorno.

#Exemplo de Chamada do Encrypt:

1º Abrir o Prompt de Comando.

2º Validar se o Java SE está instalado.
  
  Ex: java -version
  
  Resultado:
  
  java version "1.8.0_151"
  
  Java(TM) SE Runtime Environment (build 1.8.0_151-b12)
  
  Java HotSpot(TM) 64-Bit Server VM (build 25.151-b12, mixed mode)
  
3º Realizar a chamada do script para fazer o Encrypt do Arquivo de Remessa:

  java -jar WebTAEncrypt.jar "ARQUIVOORIGINAL" "ARQUIVOCRIPTOGRAFADO" "DIRETORIOORIGEM" "DIRETORIODESTINO" "DIRETORIOCHAVECRIPTOGRAFIA(.bin)" "SENHACHAVECRIPTO"
  

#Exemplo de Chamada do Decrypt:

1º Realizar a chamada do script para fazer o Decrypt do Arquivo de Retorno:

  java -jar WebTADecrypt.jar "ARQUIVOORIGINAL" "ARQUIVODESCRIPTOGRAFADO" "DIRETORIOORIGEM" "DIRETORIODESTINO" "DIRETORIOCHAVECRIPTOGRAFIA(.bin)" "SENHACHAVECRIPTO"
  

O projeto em C# está mais ou menos estruturado, porém não conseguir fazer funcionar o Encrypt corretamente, de forma que fosse validado pelo banco na transmissão.
